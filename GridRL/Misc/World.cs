using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GridRL {
    /// <summary> The type of world to be generate. </summary>
    public enum WorldType { Dungeon }

    /// <summary> A class containing an array of tiles and a level counter. </summary>
    public partial class World : Sprite {
        #region Constructors

        /// <summary> Creates the world. </summary>
        public World() { }

        #endregion
        #region Properties

        /// <summary> The array of tiles representing the current floor of the game. </summary>
        public Tile[,] Data { get; set; } = new Tile[Program.tilesHigh - 4, Program.tilesWide - 15];


        /// <summary> The current vertical level of the game. </summary>
        public int Level { get; set; } = 1;

        /// <summary> Returns a specific tile in the world. </summary>
        public Tile this[int y, int x] {
            get { return Data[y, x]; }
            set { Data[y, x] = value; }
        }

        /// <summary> Used to determine the worldgen code to be run. </summary>
        public WorldType WorldType { get; set; }

        /// <summary> World-specific list of all room locations. </summary>
        public List<List<int>> RoomPoints { get; set; }

        /// <summary> The last region to be placed. </summary>
        /// <remarks> -1 is considered placeholder, 0 is for final floodfill. </remarks>
        public int LastRegionID { get; set; } = 0;

        /// <summary> List of all creatures located in the level. </summary>
        public List<Creature> Creatures { get; set; }

        /// <summary> List of creatures to remove. </summary>
        public List<Creature> CreaturesToRemove { get; set; }

        /// <summary> List of all items located in the level. </summary>
        public List<Item> Items { get; set; }

        /// <summary> List of effects that exist in the world. </summary>
        public List<Effect> Effects { get; set; }

        /// <summary> List of effects to remove. </summary>
        public List<Effect> EffectsToRemove { get; set; }

        #endregion
        #region Methods

        /// <summary> Deletes the current level data and creates a new one. </summary>
        /// TODO: extensive unit testing. If you find a bug, make an issue and give us any and all information to debug it.
        public virtual void GenerateLevel() {
            // Clean up the old world data
            Program.canvas.Remove(this);
            for(int y = 0; y < Data.GetLength(0); ++y) {
                for(int x = 0; x < Data.GetLength(1); ++x) {
                    Data[y, x] = null;
                }
            }
            Creatures = new List<Creature>();
            CreaturesToRemove = new List<Creature>();
            Items = new List<Item>();
            RoomPoints = new List<List<int>>();
            Effects = new List<Effect>();
            EffectsToRemove = new List<Effect>();
            // Generate the new world
            if(WorldType == WorldType.Dungeon) {
                GenerateDungeon();
            }
            // Add the world to the canvas
            Program.canvas.Add(this);
        }

        public void UpdateVisibles() {
            bool playerIsInRoom = false;
            for(int y = 1; y < Data.GetLength(0) - 1; ++y) {
                for(int x = 1; x < Data.GetLength(1) - 1; ++x) {
                    if(Data[y, x] != null) {
                        if(Data[y, x].Visibility == Vis.Visible) {
                            Data[y, x].Visibility = Vis.Memory;
                        }
                    }
                }
            }
            foreach(List<int> points in RoomPoints) {
                if(Program.player.CoordY >= points[0] && Program.player.CoordY < points[2]
                && Program.player.CoordX >= points[1] && Program.player.CoordX < points[3]) {
                    playerIsInRoom = true;
                    for (int y = points[0] - 1; y <= points[2]; ++y) {
                        for(int x = points[1] - 1; x <= points[3]; ++x) {
                            if(Data[y, x] != null) {
                                // This needs  a flood fill for the whole room area.
                                Data[y, x].Visibility = Vis.Visible;
                            }
                        }
                    }
                    foreach(Creature c in Creatures) {
                        if(c.CoordY >= points[0] && c.CoordY < points[2] && c.CoordX >= points[1] && c.CoordX < points[3]) {
                            c.Visibility = Vis.Visible;
                        }
                        foreach(Effect e in Effects) {
                            if(e.CoordY >= points[0] && e.CoordY < points[2] && e.CoordX >= points[1] && e.CoordX < points[3]) {
                                e.Visibility = Vis.Visible;
                            }
                        }
                    }
                }
                else {
                    for(int y = points[0]; y < points[2]; ++y) {
                        for(int x = points[1]; x < points[3]; ++x) {
                            if(Data[y, x] != null && Data[y, x].Visibility == Vis.Visible) {
                                Data[y, x].Visibility = Vis.Memory;
                            }
                        }
                    }
                    foreach(Creature c in Creatures) {
                        if(c.CoordY >= points[0] && c.CoordY < points[2] && c.CoordX >= points[1] && c.CoordX < points[3]) {
                            c.Visibility = Vis.Unseen;
                        }
                        foreach(Effect e in Effects) {
                            if(e.CoordY >= points[0] && e.CoordY < points[2] && e.CoordX >= points[1] && e.CoordX < points[3]) {
                                e.Visibility = Vis.Unseen;
                            }
                        }
                    }
                }
            }
            if(!playerIsInRoom) {
                var dirs = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
                foreach(Direction d in dirs) {
                    for(int i = 1; i < 4; ++i) {
                        List<int> points = Program.player.DirectionToPoints(d, i);
                        if(points[0] < 0 || points[0] >= Data.GetLength(0) || points[1] < 0 || points[1] >= Data.GetLength(1)
                        || Data[points[0], points[1]] == null) {
                            break;
                        }
                        if(Data[points[0], points[1]].BlocksLight || !Data[points[0], points[1]].BlocksLight) {
                            Data[points[0], points[1]].Visibility = Vis.Visible;
                            break;
                        }
                        foreach(Creature c in Creatures) {
                            if(c.CoordY == points[0] && c.CoordX == points[1]) {
                                c.Visibility = Vis.Visible;
                            }
                        }
                        foreach(Effect e in Effects) {
                            if(e.CoordY == points[0] && e.CoordX == points[1]) {
                                e.Visibility = Vis.Visible;
                            }
                        }
                    }
                }
            }
        }

        #endregion
        #region Overrides

        /// <summary> Updates the world. </summary>
        protected override void Act() {
            // Collides creatures and effects. 
            foreach(Creature c in Creatures) {
                foreach(Effect e in Effects) {
                    if(c.CoordX == e.CoordX && c.CoordY == e.CoordY) {
                        e.OnCollide(c);
                    }
                }
            }
            // Update tiles
            for(int y = 0; y < Data.GetLength(0); ++y) {
                for(int x = 0; x < Data.GetLength(1); ++x) {
                    if(Data[y, x] != null) {
                        Data[y, x].Update();
                    }
                }
            }
            // Update creatures
            foreach(Creature c in Creatures) {
                c.Update();
            }
            // Update effects
            foreach(Effect e in Effects) {
                e.Update();
                if(e.TurnsLeft < 0) {
                    EffectsToRemove.Add(e);
                }
            }
            // Remove any effects to remove 
            foreach(Effect e in EffectsToRemove) {
                Effects.Remove(e);
            }
            // Remove an creatures to remove
            foreach(Creature c in CreaturesToRemove) {
                Creatures.Remove(c);
            }
        }

        /// <summary> Draws the world. </summary>
        /// <param name="g"> The graphics doohicky. </param>
        protected override void Paint(Graphics g) {
            for(int y = 0; y < Data.GetLength(0); ++y) {
                for(int x = 0; x < Data.GetLength(1); ++x) {
                    if(Data[y, x] != null) {
                        Data[y, x].Render(g);
                        Item i = Data[y, x].Inventory.Items.FirstOrDefault();
                        if(i != null) {
                            i.Render(g);
                        }
                    }
                }
            }
            foreach(Creature c in Creatures) {
                c.Render(g);
            }
            foreach(Effect e in Effects) {
                e.Render(g);
            }
            Program.player.Render(g);
        }

        #endregion
    }
}

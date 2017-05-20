using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GridRL {

    public enum WorldType { Dungeon }

    /// <summary> A class containing an array of tiles and a level counter. </summary>
    public partial class World : Sprite {
        /* Constructors */
        public World() { }

        /* Properties */
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
        public List<Creature> CreaturesToRemove { get; set; }

        /// <summary> List of all items located in the level. </summary>
        public List<Item> Items { get; set; }

        public List<Effect> Effects { get; set; }
        public List<Effect> EffectsToRemove { get; set; }

        /* Methods */
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


        /* Overrides */

        protected override void Act() {
            foreach(Creature c in Creatures) {
                foreach(Effect e in Effects) {
                    if(c.CoordX == e.CoordX && c.CoordY == e.CoordY) {
                        e.OnCollide(c);
                    }
                }
            }
            for(int y = 0; y < Data.GetLength(0); ++y) {
                for(int x = 0; x < Data.GetLength(1); ++x) {
                    if(Data[y, x] != null) {
                        Data[y, x].Update();
                    }
                }
            }
            foreach(Creature c in Creatures) {
                c.Update();
            }
            foreach(Effect e in Effects) {
                e.Update();
                if(e.TurnsLeft < 0) {
                    EffectsToRemove.Add(e);
                }
            }
            foreach(Effect e in EffectsToRemove) {
                Effects.Remove(e);
            }
            foreach(Creature c in CreaturesToRemove) {
                Creatures.Remove(c);
            }
        }

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
    }
}

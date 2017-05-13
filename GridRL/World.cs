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

        public WorldType WorldType { get; set; }

        public List<List<int>> RoomPoints { get; set; }

        public int LastRegionID { get; set; } = 0;

        public List<Creature> Creatures { get; set; }

        public List<Item> Items { get; set; }

        /* Methods */
        /// <summary> Deletes the current level data and creates a new one. </summary>
        /// TODO: extensive unit testing. If you find a bug, make an issue and give us any and all information to debug it.
        public virtual void GenerateLevel() {
            Program.canvas.Remove(this);
            for(int y = 0; y < Data.GetLength(0); ++y) {
                for(int x = 0; x < Data.GetLength(1); ++x) {
                    Data[y, x] = null;
                }
            }
            Creatures = new List<Creature>();
            Items = new List<Item>();
            RoomPoints = new List<List<int>>();
            if(WorldType == WorldType.Dungeon) {
                GenerateDungeon();
            }
            Creatures.Add(Program.player);
            Program.canvas.Add(this);
        }


        /* Overrides */

        protected override void Act() {
            for(int y = 0; y < Data.GetLength(0); ++y) {
                for(int x = 0; x < Data.GetLength(1); ++x) {
                    if(Data[y, x] != null) {
                        Data[y, x].Update();
                    }
                }
            }
        }

        protected override void Paint(Graphics g) {
            for(int y = 0; y < Data.GetLength(0); ++y) {
                for(int x = 0; x < Data.GetLength(1); ++x) {
                    if(Data[y, x] != null) {
                        Data[y, x].Render(g);
                    }
                }
            }
            foreach(Creature c in Creatures) {
                c.Render(g);
            }
            foreach(Item i in Items) {
                i.Render(g);
            }
        }
    }
}

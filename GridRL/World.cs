using System;
using System.Drawing;

namespace GridRL {

    /// <summary> A class containing an array of tiles and a level counter. </summary>
    public class World : Sprite{
        /* Constructors */
        public World() { GenerateLevel(); }

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

        /* Methods */
        /// <summary> Deletes the current level data and creates a new one. </summary>
        /// TODO: slightly unsafe if lots of generation happens.
        public virtual void GenerateLevel() {
            Program.canvas.Remove(this);
            int roomCount = (int)Math.Ceiling((4 * Level * Math.Sqrt(Level)) + Engine.rand.Next(5, 9)) / Level;
            Data = new Tile[Data.GetLength(0), Data.GetLength(1)];
            for(int i = 0; i < roomCount; ++i) {
                int roomX = (Engine.rand.Next(0, 26) * 2) + 1; // max width of room is 10. std rand limit is 26
                int roomY = (Engine.rand.Next(0, 15) * 2) + 1; // max height of room is 10. std rand limit is 15
                int roomW = (Engine.rand.Next(2, 4) * 2) + 1;
                int roomH = (Engine.rand.Next(2, 4) * 2) + 1;
                if(Data[roomY, roomX] == null && Data[roomY + roomH - 1, roomX] == null && Data[roomY, roomX + roomW - 1] == null && Data[roomY + roomH - 1, roomX + roomW - 1] == null) {
                    for(int y = roomY; y < roomY + roomH; ++y) {
                        for(int x = roomX; x < roomX + roomW; ++x) {
                            Tile floor = new Tile(Properties.Resources.Empty, x, y);
                            Data[y, x] = floor;
                        }
                    }
                }
                else {
                    if(Engine.rand.NextDouble() < .1) {
                        for(int y = roomY; y < roomY + roomH; ++y) {
                            for(int x = roomX; x < roomX + roomW; ++x) {
                                Tile floor = new Tile(Properties.Resources.Empty, x, y);
                                Data[y, x] = floor;
                            }
                        }
                    }
                }
            }
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
        }
    }
}

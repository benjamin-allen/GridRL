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
                int roomX = (Engine.rand.Next(0, 27) * 2) + 1; // max width of room is 10. std rand limit is 26 but rand is exclusive
                int roomY = (Engine.rand.Next(0, 16) * 2) + 1; // max height of room is 10. std rand limit is 15 but rand is exclusive
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
            int mazeY = 0;
            int mazeX = 0;
            for(int y = 1; y < Data.GetLength(0) - 1; y += 2) {
                for(int x = 1; x < Data.GetLength(1) - 1; x += 2) {
                    if(Data[y, x] == null) {
                        mazeY = y;
                        mazeX = x;
                        break;
                    }
                }
                if(mazeX != 0) {
                    break;
                }
            }
            int[] directions = { 1, 2, 3, 4 }; // 1N, 2E, 3S, 4W
            carve(mazeX, mazeY, directions, 10);
            Program.canvas.Add(this);
        }

        internal void carve(int startX, int startY, int[] directions, int depth) {
            if(depth == 0)
                return;
            int index = (Engine.rand.Next(0, 4));
            for(int i = 0; i < 4; ++i) {
                index = (index + 1) % 4;
                int[] nextStep = new int[2];
                nextStep = dydx(directions[index]);
                int nextY = startY + (2 * nextStep[0]);
                int nextX = startX + (2 * nextStep[1]);
                int interY = startY + nextStep[0];
                int interX = startX + nextStep[1];
                if(nextY < 1 || nextY > Data.GetLength(0) - 1 || nextX < 1 || nextX > Data.GetLength(1) - 1) {
                    continue;
                }
                if(Data[nextY, nextX] == null) {
                    Tile corridor = new Tile(Properties.Resources.At, interX, interY);
                    Tile corridor2 = new Tile(Properties.Resources.At, nextX, nextY);
                    Data[interX, interY] = corridor;
                    Data[nextX, nextY] = corridor2;
                    carve(nextX, nextY, directions, depth - 1);
                }
            }
            return;
        }

        internal int[] dydx(int direction) {
            int[] output = { 0, 0 };
            switch(direction) {
                case 1:
                    output[0] = 1; return output;
                case 2:
                    output[1] = 1; return output;
                case 3:
                    output[0] = -1; return output;
                case 4:
                    output[1] = -1; return output;
                default: return output;
            }
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

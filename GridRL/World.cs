using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GridRL {
    /// <summary> A class containing an array of tiles and a level counter. </summary>
    public class World : Sprite {
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
            int[] directions = { 1, 2, 3, 4 }; // 1N, 2S, 3W, 4E
            carve(mazeX, mazeY, directions);
            Program.canvas.Add(this);
        }

        internal void carve(int startX, int startY, int[] directions) {
            List<int> validDirs = getValidDirectionsFrom(startX, startY);
            if(validDirs.Count == 0) {
                return;
            }
            foreach(int i in validDirs) {
                List<int> newValidDirs = getValidDirectionsFrom(startX, startY);
                if(!newValidDirs.Contains(i)) {
                    continue;
                }
                int rando = Engine.rand.Next(0, newValidDirs.Count);
                int[] direction = dydx(directions[newValidDirs[rando]]);
                int nextY = startY + (2 * direction[0]);
                int nextX = startX + (2 * direction[1]);
                int interY = startY + direction[0];
                int interX = startX + direction[1];
                Tile corridor = new Tile(Properties.Resources.At, interX, interY);
                Tile corridor2 = new Tile(Properties.Resources.At, nextX, nextY);
                if(Data[startY, startX] == null) {
                    Data[startY, startX] = new GridRL.Tile(Properties.Resources.At, startX, startY);
                }
                Data[interY, interX] = corridor;
                Data[nextY, nextX] = corridor2;
                carve(nextX, nextY, directions);
            }
        }

        internal int[] dydx(int direction) {
            int[] output = { 0, 0 };
            switch(direction) {
                case 1:
                    output[0] = -1;
                    return output; // North
                case 2:
                    output[0] = 1;
                    return output; // South
                case 3:
                    output[1] = -1;
                    return output; // West
                case 4:
                    output[1] = 1;
                    return output; // East
                default:
                    return output;
            }
        }

        internal List<int> getValidDirectionsFrom(int testX, int testY) {
            List<int> output = new List<int>();
            if(testY - 2 > 0) {
                if(Data[testY - 2, testX] == null) {
                    output.Add(0); // North
                }
            }
            if(testY + 2 < Data.GetLength(0)) {
                if(Data[testY + 2, testX] == null) {
                    output.Add(1); // South
                }
            }
            if(testX - 2 > 0) {
                if(Data[testY, testX - 2] == null) {
                    output.Add(2); // East
                }
            }
            if(testX + 2 < Data.GetLength(1)) {
                if(Data[testY, testX + 2] == null) {
                    output.Add(3); // West
                }
            }
            return output;
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

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
        /// TODO: extensive unit testing. If you find a bug, make an issue and give us any and all information to debug it.
        public virtual void GenerateLevel() {
            DateTime start = DateTime.Now;
            Program.canvas.Remove(this);
            int roomCount = (int)Math.Ceiling((4 * Level * Math.Sqrt(Level)) + Engine.rand.Next(5, 9)) / Level;
            Data = new Tile[Data.GetLength(0), Data.GetLength(1)];
            int regionID = 0;
            List<List<int>> roomPoints = new List<List<int>>();
            for(int i = 0; i < roomCount; ++i) {
                List<int> points = new List<int>();
                int roomX = (Engine.rand.Next(1, 26) * 2) + 1; // max width of room is 10. std rand limit is 26 but rand is exclusive
                int roomY = (Engine.rand.Next(1, 15) * 2) + 1; // max height of room is 10. std rand limit is 15 but rand is exclusive
                int roomW = (Engine.rand.Next(2, 4) * 2) + 1;
                int roomH = (Engine.rand.Next(2, 4) * 2) + 1;
                if(Data[roomY, roomX] == null && Data[roomY + roomH - 1, roomX] == null && Data[roomY, roomX + roomW - 1] == null && Data[roomY + roomH - 1, roomX + roomW - 1] == null) {
                    regionID++;
                    for(int y = roomY; y < roomY + roomH; ++y) {
                        for(int x = roomX; x < roomX + roomW; ++x) {
                            Data[y, x] = new RoomFloor(x, y, regionID);
                        }
                    }
                    points.Add(roomY);
                    points.Add(roomX);
                    points.Add(roomY + roomH);
                    points.Add(roomX + roomW);
                    roomPoints.Add(points);
                }
                else {
                    if(Engine.rand.NextDouble() < .1) {
                        for(int y = roomY; y < roomY + roomH; ++y) {
                            for(int x = roomX; x < roomX + roomW; ++x) {
                                Data[y, x] = new RoomFloor(x, y, regionID);
                            }
                        }
                        points.Add(roomY);
                        points.Add(roomX);
                        points.Add(roomY + roomH);
                        points.Add(roomX + roomW);
                        roomPoints.Add(points);
                    }
                }
            }
            int mazeY = 0;
            int mazeX = 0;
            int[] directions = { 1, 2, 3, 4 }; // 1N, 2S, 3W, 4E
            while(true) {
                int[] mazeCoords = getMazeStartPoints();
                mazeY = mazeCoords[0];
                mazeX = mazeCoords[1];
                if(mazeY == 0 || mazeX == 0) {
                    break;
                }
                else {
                    regionID++;
                    carve(mazeX, mazeY, directions, regionID);
                }
            }

            foreach(List<int> pointSet in roomPoints) {
                makeDoor(pointSet);
            }

            for(int i = 0; i < 50; i++) {
                List<List<int>> mazeEnds = getMazeEndPoints();
                foreach(List<int> endPoints in mazeEnds) {
                    Data[endPoints[0], endPoints[1]] = null;
                }
            }

            Program.canvas.Add(this);
        }

        #region Worldgen Functions

        
        private void carve(int startX, int startY, int[] directions, int region) {
            if(Data[startY, startX] == null) {
                Data[startY, startX] = new Corridor(startX, startY, region);
            }
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
                Data[interY, interX] = new Corridor(interX, interY, region);
                Data[nextY, nextX] = new Corridor(nextX, nextY, region);
                carve(nextX, nextY, directions, region);
            }
        }

        private int[] dydx(int direction) {
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

        private List<int> getValidDirectionsFrom(int testX, int testY) {
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

        private int[] getMazeStartPoints() {
            int[] output = { 0, 0 };
            for(int y = 1; y < Data.GetLength(0); y += 2) {
                for(int x = 1; x < Data.GetLength(1); x += 2) {
                    if(Data[y, x] == null) {
                        output[0] = y;
                        output[1] = x;
                        return output;
                    }
                }
            }
            return output;
        }

        private void makeDoor(List<int> points) {
            int roomY = points[0];
            int roomX = points[1];
            int room2Y = points[2];
            int room2X = points[3];
            bool isConnected = false;
            int wallToCarve = Engine.rand.Next(0, 4);
            int wallsTried = 0;
            // dis needs safety checks
            // an infinite loop fixes
            while(wallsTried < 4 && !isConnected) {
                if(wallToCarve == 0) {
                    // carve on north wall
                    int dY = roomY - 1;
                    int dX = roomX + Engine.rand.Next(0, room2X - roomX);
                    int firstDX = dX;
                    while(!isConnected) {
                        if(Data[dY, dX] == null && Data[dY - 1, dX] != null) {
                            isConnected = true;
                            int overrideRegion = Data[dY - 1, dX].Region;
                            Data[dY, dX] = new Door(dX, dY, overrideRegion);
                            for(int y = roomY; y < room2Y; ++y) {
                                for(int x = roomX; x < room2X; ++x) {
                                    Data[y, x].Region = overrideRegion;
                                }
                            }
                        }
                        else {
                            dX++;
                            if(dX >= room2X) {
                                dX = roomX;
                            }
                        }
                        if(firstDX == dX) {
                            wallsTried++;
                            wallToCarve = (wallToCarve + 1) % 4;
                            break;
                        }
                    }
                    if(Engine.rand.NextDouble() < .25) {
                        isConnected = false;
                        wallToCarve = (wallToCarve + 1) % 4;
                    }
                }
                if(wallToCarve == 1) {
                    // carve on south wall
                    int dY = room2Y;
                    int dX = roomX + Engine.rand.Next(0, room2X - roomX);
                    int firstDX = dX;
                    while(!isConnected) {
                        if(Data[dY, dX] == null && Data[dY + 1, dX] != null) {
                            isConnected = true;
                            int overrideRegion = Data[dY + 1, dX].Region;
                            Data[dY, dX] = new Door(dX, dY, overrideRegion);
                            for(int y = roomY; y < room2Y; ++y) {
                                for(int x = roomX; x < room2X; ++x) {
                                    Data[y, x].Region = overrideRegion;
                                }
                            }
                        }
                        else {
                            dX++;
                            if(dX >= room2X) {
                                dX = roomX;
                            }
                        }
                        if(firstDX == dX) {
                            wallsTried++;
                            wallToCarve = (wallToCarve + 1) % 4;
                            break;
                        }
                    }
                    if(Engine.rand.NextDouble() < .25) {
                        isConnected = false;
                        wallToCarve = (wallToCarve + 1) % 4;
                    }
                }
                if(wallToCarve == 2) {
                    // carve on west wall
                    int dY = roomY + Engine.rand.Next(0, room2Y - roomY);
                    int dX = roomX - 1;
                    int firstDY = dY;
                    while(!isConnected) {
                        if(Data[dY, dX] == null && Data[dY, dX - 1] != null) {
                            isConnected = true;
                            int overrideRegion = Data[dY, dX - 1].Region;
                            Data[dY, dX] = new Door(dX, dY, overrideRegion);
                            for(int y = roomY; y < room2Y; ++y) {
                                for(int x = roomX; x < room2X; ++x) {
                                    Data[y, x].Region = overrideRegion;
                                }
                            }
                        }
                        else {
                            dY++;
                            if(dY >= room2Y) {
                                dY = roomY;
                            }
                        }
                        if(firstDY == dY) {
                            wallsTried++;
                            wallToCarve = (wallToCarve + 1) % 4;
                            break;
                        }
                    }
                    if(Engine.rand.NextDouble() < .25) {
                        isConnected = false;
                        wallToCarve = (wallToCarve + 1) % 4;
                    }
                }
                if(wallToCarve == 3) {
                    // carve on east wall
                    int dY = roomY + Engine.rand.Next(0, room2Y - roomY);
                    int dX = roomX + 1;
                    int firstDY = dY;
                    while(!isConnected) {
                        if(Data[dY, dX] == null && Data[dY, dX + 1] != null) {
                            isConnected = true;
                            int overrideRegion = Data[dY, dX + 1].Region;
                            Data[dY, dX] = new Door(dX, dY, overrideRegion);
                            for(int y = roomY; y < room2Y; ++y) {
                                for(int x = roomX; x < room2X; ++x) {
                                    Data[y, x].Region = overrideRegion;
                                }
                            }
                        }
                        else {
                            dY++;
                            if(dY >= room2Y) {
                                dY = roomY;
                            }
                        }
                        if(firstDY == dY) {
                            wallsTried++;
                            wallToCarve = (wallToCarve + 1) % 4;
                            break;
                        }
                    }
                    if(Engine.rand.NextDouble() < .25) {
                        isConnected = false;
                        wallToCarve = (wallToCarve + 1) % 4;
                    }
                } 
            }
        }

        private List<List<int>> getMazeEndPoints() {
            List<List<int>> output = new List<List<int>>();
            for(int y = 1; y < Data.GetLength(0) - 1; y++) {
                for(int x = 1; x < Data.GetLength(1) - 1; x++) {
                    int notNullNeighbors = 0;
                    if(Data[y - 1, x] != null) {
                        notNullNeighbors++;
                    }
                    if(Data[y + 1, x] != null) {
                        notNullNeighbors++;
                    }
                    if(Data[y, x - 1] != null) {
                        notNullNeighbors++;
                    }
                    if(Data[y, x + 1] != null) {
                        notNullNeighbors++;
                    }
                    if(notNullNeighbors <= 1) {
                        output.Add(new List<int>(new int[] { y, x }));
                    }
                }
            }
            return output;
        }
        #endregion

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

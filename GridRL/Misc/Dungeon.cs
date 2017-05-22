using System;
using System.Collections.Generic;

namespace GridRL {
    public partial class World {
        #region Methods

        /// <summary> Populates a dungeon-like map. </summary>
        private void GenerateDungeon() {
            GenerateRooms();
            GenerateMaze();
            foreach(List<int> pointSet in RoomPoints) {
                MakeDoor(pointSet);
            }
            // Retract the maze starting at the dead ends
            for(int i = 0; i < 50; i++) {
                List<List<int>> mazeEnds = GetMazeEndPoints();
                foreach(List<int> endPoints in mazeEnds) {
                    Data[endPoints[0], endPoints[1]] = null;
                }
            }
            // Ensure that all regions are connected (i.e. only one region)
            List<List<int>> regionLocations = GetRegionLocations();
            while(regionLocations.Count > 1) {
                ConnectRegions(regionLocations);
                FloodFill(regionLocations[0][0], regionLocations[0][1]);
                regionLocations = GetRegionLocations();
            }
            // Place the stairs to get out of the dungeon
            int entryRoom = Engine.rand.Next(0, RoomPoints.Count);
            int exitRoom = Engine.rand.Next(0, RoomPoints.Count);
            if(entryRoom == exitRoom && RoomPoints.Count > 1) {
                exitRoom = (exitRoom + 1) % RoomPoints.Count;
            }
            int entryY = Engine.rand.Next(RoomPoints[entryRoom][0], RoomPoints[entryRoom][2]);
            int entryX = Engine.rand.Next(RoomPoints[entryRoom][1], RoomPoints[entryRoom][1]);
            int exitY = Engine.rand.Next(RoomPoints[exitRoom][0], RoomPoints[exitRoom][2]);
            int exitX = Engine.rand.Next(RoomPoints[exitRoom][1], RoomPoints[exitRoom][3]);
            Data[entryY, entryX] = new Stair(entryY, entryX, StairType.Up);
            Data[exitY, exitX] = new Stair(exitY, exitX, StairType.Down);

            foreach(List<int> points in RoomPoints) {
                for(int y = points[0] - 1; y < points[2] + 1; ++y) {
                    for(int x = points[1] - 1; x < points[3] + 1; ++x) {
                        if(Data[y, x] == null) {
                            Data[y, x] = new Wall(y, x);
                        }
                    }
                }
            }

            Program.player.CoordX = entryX;
            Program.player.CoordY = entryY;
            DummyCreature dummy = new DummyCreature(entryY + 3, entryX + 3);
            Creatures.Add(dummy);
        }

        /// <summary> Builds the rooms of the dungeon. </summary>
        private void GenerateRooms() {
            // This is a curve so that more rooms are built as the level increases. 
            int roomCount = (int)Math.Ceiling((8 * Level * Math.Sqrt(Level)) + Engine.rand.Next(5, 9)) / Level;
            LastRegionID = 0;
            for(int i = 0; i < roomCount; ++i) {
                List<int> points = new List<int>();
                points.Add((Engine.rand.Next(1, 15) * 2) + 1); // max height of room is 10. std rand limit is 15 but rand is exclusive
                points.Add((Engine.rand.Next(1, 26) * 2) + 1); // max width of room is 10. std rand limit is 26 but rand is exclusive
                points.Add(points[0] + ((Engine.rand.Next(2, 4) * 2) + 1));
                points.Add(points[1] + ((Engine.rand.Next(2, 4) * 2) + 1));
                // Place a room if there's no overlap
                if(!CheckForRoomOverlap(points)) {
                    LastRegionID++;
                    BuildRoom(points, LastRegionID);
                    RoomPoints.Add(points);
                }
                // Otherwise place it 1/5 of the time even if there is an overlap
                else {
                    if(Engine.rand.NextDouble() < .2 || i == roomCount - 1) {
                        LastRegionID++;
                        BuildRoom(points, LastRegionID);
                        RoomPoints.Add(points);
                    }
                }
            }
        }

        /// <summary> Generates a new maze at the new maze location. </summary>
        private void GenerateMaze() {
            List<int> mazePoints = new List<int>();
            while(true) {
                mazePoints = GetMazeStartPoints();
                // if there's nowhere to start a maze
                if(mazePoints[0] == 0 || mazePoints[1] == 0) {
                    break;
                }
                // otherwise, start a maze
                else {
                    LastRegionID++;
                    Carve(mazePoints, LastRegionID);
                }
            }
        }

        /// <summary> Places a room at the specified points. </summary>/summary>
        /// <param name="points"> The points of the room. </param>
        /// <param name="region"> The region to be used for the new room. </param>
        private void BuildRoom(List<int> points, int region) {
            for(int y = points[0]; y < points[2]; ++y) {
                for(int x = points[1]; x < points[3]; ++x) {
                    Data[y, x] = new RoomFloor(y, x, region);
                }
            }
        }

        /// <summary> RBT maze generation function. </summary>
        /// <param name="points"> Points to carve. </param>
        /// <param name="region"> Region to use for this maze. </param>
        private void Carve(List<int> points, int region) {
            // Make a corridor at the points if it's null
            if(Data[points[0], points[1]] == null) {
                Data[points[0], points[1]] = new Corridor(points[0], points[1], region);
            }
            // Check to see if any carving can be done
            List<Direction> validDirs = GetValidDirectionsFrom(points[0], points[1]);
            if(validDirs.Count == 0) {
                return;
            }
            // If so, shuffle directions, pick each direction, place corridors, and call Carve()
            Program.Shuffle(validDirs);
            foreach(Direction d in validDirs) {
                if(Data[points[0], points[1]].CanAccess(d, 2)) {
                    continue;
                }
                List<int> interPoints = Data[points[0], points[1]].DirectionToPoints(d);
                List<int> nextPoints = Data[points[0], points[1]].DirectionToPoints(d, 2);
                Data[interPoints[0], interPoints[1]] = new Corridor(interPoints[0], interPoints[1], region);
                Data[nextPoints[0], nextPoints[1]] = new Corridor(nextPoints[0], nextPoints[1], region);
                Carve(nextPoints, region);
            }
        }
        /// <summary> Helper function for room placement. Checks if the edges of the room made by the points overlap
        ///           with any other room. </summary>
        /// <param name="points"> The points of the room to be tested</param>
        /// <returns> A boolean indicating whether rooms overlap or not. </returns>
        private bool CheckForRoomOverlap(List<int> points) {
            // Check the horizontal edges
            for(int i = points[0]; i < points[2]; i += 2) {
                if(Data[i, points[1]] == null && Data[i, points[3]] == null) {
                    continue;
                }
                else {
                    return true;
                }
            }
            // Check the vertical edges
            for(int i = points[1]; i < points[3]; i += 2) {
                if(Data[points[0], i] == null && Data[points[2], i] == null) {
                    continue;
                }
                else {
                    return true;
                }
            }
            return false;
        }

        /// <summary> Finds the first available maze start location. </summary>
        /// <returns> The points to begin the new maze at. </returns>
        private List<int> GetMazeStartPoints() {
            List<int> output = new List<int>(new int[] { 0, 0 });
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

        /// <summary> Helper function for Carve. Takes input points and checks for empty data.  </summary>
        /// <param name="testY"> Y coordinate of current Carve call. </param>
        /// <param name="testX"> X coordinate of current Carve call. </param>
        /// <returns> List of valid directions to carve in. </returns>
        private List<Direction> GetValidDirectionsFrom(int testY, int testX) {
            List<Direction> output = new List<Direction>();
            if(testY - 2 > 0) {
                if(Data[testY - 2, testX] == null) {
                    output.Add(Direction.Up); // North
                }
            }
            if(testY + 2 < Data.GetLength(0)) {
                if(Data[testY + 2, testX] == null) {
                    output.Add(Direction.Down); // South
                }
            }
            if(testX - 2 > 0) {
                if(Data[testY, testX - 2] == null) {
                    output.Add(Direction.Left); // East
                }
            }
            if(testX + 2 < Data.GetLength(1)) {
                if(Data[testY, testX + 2] == null) {
                    output.Add(Direction.Right); // West
                }
            }
            return output;
        }

        /// <summary> Helper function for MakeDoor(). Calculates a position for attaching a door to a room. </summary>
        /// <param name="d"> The direction being tested. </param>
        /// <param name="points"> The RoomPoints data for this room. </param>
        /// <returns> The two points to place the door at. </returns>
        private List<int> DirectionToDoorPoints(Direction d, List<int> points) {
            int roomH = points[2] - points[0];
            int roomW = points[3] - points[1];
            int testY = 0;
            int testX = 0;
            bool upOrDown = false;
            if(d == Direction.Down || d == Direction.Up) {
                upOrDown = true;
                testY = points[0];
                if(d == Direction.Down) { testY = points[2] - 1; }
                testX = points[1] + Engine.rand.Next(0, roomW);
            }
            else if(d == Direction.Left || d == Direction.Right) {
                testX = points[1];
                if(d == Direction.Right) { testX = points[3] - 1; }
                testY = points[0] + Engine.rand.Next(0, roomH);
            }
            // If a door connection would not actually connect two areas.
            while(!Data[testY, testX].CanAccess(d, 2)) {
                if(upOrDown) {
                    testX++;
                    if(testX >= points[3]) {
                        testX = points[1];
                    }
                }
                else {
                    testY++;
                    if(testY >= points[2]) {
                        testY = points[0];
                    }
                }
            }
            return Data[testY, testX].DirectionToPoints(d);
        }

        /// <summary> Places a door on the side of a room. </summary>
        /// <param name="points"> The RoomPoints of the room. </param>
        private void MakeDoor(List<int> points) {
            int roomY = points[0];
            int roomX = points[1];
            int room2Y = points[2];
            int room2X = points[3];
            bool isConnected = false;
            // Try each wall randomly
            List<Direction> walls = new List<Direction>(new Direction[] { Direction.Up, Direction.Down, Direction.Left, Direction.Right });
            Program.Shuffle(walls);
            foreach(Direction d in walls) {
                if(!isConnected) {
                    List<int> doorPoints = DirectionToDoorPoints(d, points);
                    if(doorPoints.Contains(0)) {
                        continue;
                    }
                    isConnected = true;
                    Data[doorPoints[0], doorPoints[1]] = new Door(doorPoints[0], doorPoints[1], -1);
                    List<int> otherArea = Data[doorPoints[0], doorPoints[1]].DirectionToPoints(d);
                    int overrideRegion = Data[otherArea[0], otherArea[1]].Region;
                    Data[doorPoints[0], doorPoints[1]].Region = overrideRegion;
                    // Once the door is in place, flood the room with the new region to represent attachment
                    for(int y = roomY; y < room2Y; ++y) {
                        for(int x = roomX; x < room2X; ++x) {
                            Data[y, x].Region = overrideRegion;
                        }
                    }
                    // Have a small chance to place another door
                    if(Engine.rand.NextDouble() < .1) {
                        isConnected = false;
                    }
                }
            }
        }

        /// <summary> Locates parts of the maze to be deleted. </summary>
        /// <returns> List of all points in the maze to be removed. </returns>
        private List<List<int>> GetMazeEndPoints() {
            List<List<int>> output = new List<List<int>>();
            var dirs = Enum.GetValues(typeof(Direction));
            for(int y = 1; y < Data.GetLength(0) - 1; y++) {
                for(int x = 1; x < Data.GetLength(1) - 1; x++) {
                    if(Data[y, x] != null) {
                        int notNullNeighbors = 0;
                        // Corridors can be deleted if they are a dead end (i.e. have more than 2 nulls surrounding them)
                        foreach(Direction d in dirs) {
                            if(Data[y, x].CanAccess(d)) {
                                notNullNeighbors++;
                            }
                        }
                        if(notNullNeighbors <= 1) {
                            output.Add(new List<int>(new int[] { y, x }));
                        }
                    }
                }
            }
            return output;
        }

        /// <summary> Retrieves all individual regions left on the map. </summary>
        /// <returns> A list of the upper left locations of all individual regions. </returns>
        private List<List<int>> GetRegionLocations() {
            List<List<int>> regionLocs = new List<List<int>>();
            List<int> regionIDs = new List<int>();
            for(int y = 1; y < Data.GetLength(0) - 1; y += 2) {
                for(int x = 1; x < Data.GetLength(1) - 1; x += 2) {
                    if(Data[y, x] != null) {
                        int region = Data[y, x].Region;
                        if(!regionIDs.Contains(region)) {
                            regionIDs.Add(region);
                            regionLocs.Add(new List<int>(new int[] { y, x, region }));
                        }
                    }
                }
            }
            return regionLocs;
        }

        /// <summary> Attaches each region to a nearby one. </summary>
        /// <param name="regionLocations"> The list of all individual region locations. </param>
        private void ConnectRegions(List<List<int>> regionLocations) {
            List<Direction> dirs = new List<Direction>();
            foreach(List<int> points in regionLocations) {
                if(points[2] != regionLocations[0][2]) {
                    int y = points[0];
                    int x = points[1];
                    bool ySafe = y > 3 && y < Data.GetLength(0) - 3;
                    bool xSafe = x > 3 && x < Data.GetLength(1) - 3;
                    int currentRegion = Data[y, x].Region;
                    int masterRegion = regionLocations[0][2];
                    if(ySafe && xSafe) {
                        dirs = new List<Direction>(new Direction[] { Direction.Up, Direction.Down, Direction.Left, Direction.Right });
                    }
                    else if(ySafe) {
                        dirs = new List<Direction>(new Direction[] { Direction.Up, Direction.Down });
                    }
                    else if (xSafe) {
                        dirs = new List<Direction>(new Direction[] { Direction.Left, Direction.Right });
                    }
                    Program.Shuffle(dirs);
                    foreach(Direction d in dirs) {
                        List<int> masterRegionPoints = Data[y, x].DirectionToPoints(d, 2);
                        List<int> connectPoints = Data[y, x].DirectionToPoints(d, 1);
                        if(!Data[y,x].CanAccess(d, 1) && Data[y,x].CanAccess(d, 2) && Data[masterRegionPoints[0], masterRegionPoints[1]].Region != currentRegion) {
                            Data[connectPoints[0], connectPoints[1]] = new Door(connectPoints[0], connectPoints[1], currentRegion);
                            continue;
                        }
                    }
                    FloodFill(points[0], points[1]);
                }
            }
        }

        /// <summary> Recursive function that modifies regions of all connected tiles. </summary>
        /// <param name="y"> The Y coordinate of this call. </param>
        /// <param name="x"> The X coordinate of this call. </param>
        private void FloodFill(int y, int x) {
            List<Direction> validDirs = new List<Direction>();
            var dirs = Enum.GetValues(typeof(Direction));
            foreach(Direction d in dirs) {
                List<int> points = Data[y, x].DirectionToPoints(d);
                if(Data[points[0], points[1]] != null && Data[points[0], points[1]].Region != 0) {
                    validDirs.Add(d);
                }
            }
            foreach(Direction dir in validDirs) {
                List<int> points = Data[y, x].DirectionToPoints(dir);
                Data[y, x].Region = 0;
                Data[points[0], points[1]].Region = 0;
                FloodFill(points[0], points[1]);
            }
        }

        #endregion
    }
}

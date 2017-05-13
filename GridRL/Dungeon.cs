using System;
using System.Collections.Generic;

namespace GridRL {
    public partial class World {
        /* Methods */
        private void GenerateDungeon() {
            GenerateRooms();
            GenerateMaze();
            foreach(List<int> pointSet in RoomPoints) {
                makeDoor(pointSet);
            }

            for(int i = 0; i < 50; i++) {
                List<List<int>> mazeEnds = GetMazeEndPoints();
                foreach(List<int> endPoints in mazeEnds) {
                    Data[endPoints[0], endPoints[1]] = null;
                }
            }
            // floodfill checks
            List<List<int>> regionLocations = getRegionLocations();

            while(regionLocations.Count > 1) {
                connectRegions(regionLocations);
                floodFill(regionLocations[0][0], regionLocations[0][1], directions);
                regionLocations = getRegionLocations();
            }
            // populate stairs
        }


        protected void GenerateRooms() {
            int roomCount = (int)Math.Ceiling((4 * Level * Math.Sqrt(Level)) + Engine.rand.Next(5, 9)) / Level;
            LastRegionID = 0;
            for(int i = 0; i < roomCount; ++i) {
                List<int> points = new List<int>();
                points.Add((Engine.rand.Next(1, 15) * 2) + 1); // max height of room is 10. std rand limit is 15 but rand is exclusive
                points.Add((Engine.rand.Next(1, 26) * 2) + 1); // max width of room is 10. std rand limit is 26 but rand is exclusive
                points.Add(points[0] + ((Engine.rand.Next(2, 4) * 2) + 1));
                points.Add(points[1] + ((Engine.rand.Next(2, 4) * 2) + 1));
                if(!CheckForRoomOverlap(points)) {
                    LastRegionID++;
                    BuildRoom(points, LastRegionID);
                    RoomPoints.Add(points);
                }
                else {
                    if(Engine.rand.NextDouble() < .1) {
                        BuildRoom(points, LastRegionID);
                        RoomPoints.Add(points);
                    }
                }
            }
        }

        protected void GenerateMaze() {
            List<int> mazePoints = new List<int>();
            while(true) {
                mazePoints = GetMazeStartPoints();
                if(mazePoints[0] == 0 || mazePoints[1] == 0) {
                    break;
                }
                else {
                    LastRegionID++;
                    Carve(mazePoints, LastRegionID);
                }
            }
        }


        private void BuildRoom(List<int> points, int region) {
            for(int y = points[0]; y < points[2]; ++y) {
                for(int x = points[1]; x < points[3]; ++x) {
                    Data[y, x] = new RoomFloor(y, x, region);
                    Data[y, x].IsVisible = true;
                }
            }
        }

        private void Carve(List<int> points, int region) {
            if(Data[points[0], points[1]] == null) {
                Data[points[0], points[1]] = new Corridor(points[0], points[1], region);
                Data[points[0], points[1]].IsVisible = true;
            }
            List<Direction> validDirs = GetValidDirectionsFrom(points[0], points[1]);
            if(validDirs.Count == 0) {
                return;
            }
            foreach(Direction d in validDirs) {
                if(Data[points[0], points[1]].CanAccess(d, 2)) {
                    continue;
                }
                List<int> interPoints = Data[points[0], points[1]].DirectionToPoints(d);
                List<int> nextPoints = Data[points[0], points[1]].DirectionToPoints(d, 2);
                Data[interPoints[0], interPoints[1]] = new Corridor(interPoints[0], interPoints[1], region);
                Data[interPoints[0], interPoints[1]].IsVisible = true;
                Data[nextPoints[0], nextPoints[1]] = new Corridor(nextPoints[0], nextPoints[1], region);
                Data[nextPoints[0], nextPoints[1]].IsVisible = true;
                Carve(nextPoints, region);
            }
        }

        private bool CheckForRoomOverlap(List<int> points) {
            for(int i = points[0]; i < points[2]; ++i) {
                if(Data[i, points[1]] != null || Data[i, points[3]] != null) {
                    return true;
                }
            }
            for(int i = points[1]; i < points[3]; ++i) {
                if(Data[points[0], i] != null || Data[points[3], i] != null) {
                    return true;
                }
            }
            return false;
        }

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

        private List<int> DirectionToDoorPoints(Direction d, List<int> points) {
            int roomH = points[2] - points[0];
            int roomW = points[3] - points[1];
            int doorY = 0;
            int doorX = 0;
            bool upOrDown = false;
            if(d == Direction.Down || d == Direction.Up) {
                upOrDown = true;
                if(d == Direction.Down) { doorY = points[2]; }
                doorX = points[1] + Engine.rand.Next(0, roomW);
            }
            else if(d == Direction.Left || d == Direction.Right) {
                if(d == Direction.Right) { doorX = points[3]; }
                doorY = points[0] + Engine.rand.Next(0, roomH);
            }
            while(!Data[doorY, doorX].CanAccess(d)) {
                if(upOrDown) {
                    doorX++;
                    if(doorX > points[3]) {
                        doorX = points[1];
                    }
                }
                else {
                    doorY++;
                    if(doorY > points[2]) {
                        doorY = points[0];
                    }
                }
            }
            return new List<int>(new int[] { doorY, doorX });
        }

        private void MakeDoor(List<int> points) {
            int roomY = points[0];
            int roomX = points[1];
            int room2Y = points[2];
            int room2X = points[3];
            bool isConnected = false;
            List<Direction> walls = new List<Direction>(new Direction[] { Direction.Up, Direction.Down, Direction.Left, Direction.Right });
            Program.Shuffle(walls);
            foreach(Direction d in walls) {
                if(!isConnected) {
                    List<int> doorPoints = DirectionToDoorPoints(d, points);
                    if(doorPoints.Contains(0)) {
                        continue;
                    }
                    isConnected = true;
                    List<int> otherArea = Data[doorPoints[0], doorPoints[1]].DirectionToPoints(d);
                    int overrideRegion = Data[otherArea[0], otherArea[1]].Region;
                    Data[doorPoints[0], doorPoints[1]] = new Door(doorPoints[0], doorPoints[1], overrideRegion);
                    Data[doorPoints[0], doorPoints[1]].IsVisible = true;
                    for(int y = roomY; y < room2Y; ++y) {
                        for(int x = roomX; x < room2X; ++x) {
                            Data[y, x].Region = overrideRegion;
                        }
                    }
                    if(Engine.rand.NextDouble() < .1) {
                        isConnected = false;
                    }
                }
            }
        }

        private List<List<int>> GetMazeEndPoints() {
            List<List<int>> output = new List<List<int>>();
            var dirs = Enum.GetValues(typeof(Direction));
            for(int y = 1; y < Data.GetLength(0) - 1; y++) {
                for(int x = 1; x < Data.GetLength(1) - 1; x++) {
                    if(Data[y, x] != null) {
                        int notNullNeighbors = 0;
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

        private List<List<int>> getRegionLocations() {
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

        private void connectRegions(List<List<int>> regionLocations) {
            foreach(List<int> points in regionLocations) {
                if(points[2] != regionLocations[0][2]) {
                    int y = points[0];
                    int x = points[1];
                    bool ySafe = y > 3 && y < Data.GetLength(0) - 3;
                    bool xSafe = x > 3 && x < Data.GetLength(1) - 3;
                    int currentRegion = Data[y, x].Region;
                    int masterRegion = regionLocations[0][2];
                    //List<int> possibleConnections = new List<int>();
                    if(ySafe && xSafe) {
                        // check all directions
                        if(Data[y - 1, x] == null && Data[y - 2, x] != null && Data[y - 2, x].Region != currentRegion) {
                            Data[y - 1, x] = new Door(y - 1, x, currentRegion);
                            Data[y - 1, x].IsVisible = true;
                            continue;
                        }
                        else if(Data[y + 1, x] == null && Data[y + 2, x] != null && Data[y + 2, x].Region != currentRegion) {
                            Data[y + 1, x] = new Door(y + 1, x, currentRegion);
                            Data[y + 1, x].IsVisible = true;
                            continue;
                        }
                        else if(Data[y, x - 1] == null && Data[y, x - 2] != null && Data[y, x - 2].Region != currentRegion) {
                            Data[y, x - 1] = new Door(y, x - 1, currentRegion);
                            Data[y, x - 1].IsVisible = true;
                            continue;
                        }
                        else if(Data[y, x + 1] == null && Data[y, x + 2] != null && Data[y, x + 2].Region != currentRegion) {
                            Data[y, x + 1] = new Door(y, x + 1, currentRegion);
                            Data[y, x + 1].IsVisible = true;
                            continue;
                        }
                    }
                    else if(ySafe) {
                        // check only Y
                        if(Data[y - 1, x] == null && Data[y - 2, x] != null && Data[y - 2, x].Region != currentRegion) {
                            Data[y - 1, x] = new Door(y - 1, x, currentRegion);
                            Data[y - 1, x].IsVisible = true;
                            continue;
                        }
                        else if(Data[y + 1, x] == null && Data[y + 2, x] != null && Data[y + 2, x].Region != currentRegion) {
                            Data[y + 1, x] = new Door(y + 1, x, currentRegion);
                            Data[y + 1, x].IsVisible = true;
                            continue;
                        }
                    }
                    else if(xSafe) {
                        // check only x
                        if(Data[y, x - 1] == null && Data[y, x - 2] != null && Data[y, x - 2].Region != currentRegion) {
                            Data[y, x - 1] = new Door(y, x - 1, currentRegion);
                            Data[y, x - 1].IsVisible = true;
                            continue;
                        }
                        else if(Data[y, x + 1] == null && Data[y, x + 2] != null && Data[y, x + 2].Region != currentRegion) {
                            Data[y, x + 1] = new Door(y, x + 1, currentRegion);
                            Data[y, x + 1].IsVisible = true;
                            continue;
                        }
                    }
                    floodFill(points[0], points[1], new int[] { 1, 2, 3, 4 });
                }
            }
        }

        private void floodFill(int y, int x, int[] directions) {
            if(y == 13 && x == 42) { }
            List<int> validDirs = new List<int>();
            if(Data[y - 1, x] != null && Data[y - 1, x].Region != 0) {
                validDirs.Add(0);
            }
            if(Data[y + 1, x] != null && Data[y + 1, x].Region != 0) {
                validDirs.Add(1);
            }
            if(Data[y, x - 1] != null && Data[y, x - 1].Region != 0) {
                validDirs.Add(2);
            }
            if(Data[y, x + 1] != null && Data[y, x + 1].Region != 0) {
                validDirs.Add(3);
            }
            foreach(int dir in validDirs) {
                int[] direction = dydx(directions[dir]);
                int nextY = y + direction[0];
                int nextX = x + direction[1];
                if(nextX == 41 && x == 42 && y == 13) { }
                Data[y, x].Region = 0;
                Data[nextY, nextX].Region = 0;
                floodFill(nextY, nextX, directions);
            }
        }
    }
}

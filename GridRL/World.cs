namespace GridRL {

    /// <summary> A class containing an array of tiles and a level counter. </summary>
    public class World {
        /* Constructors */
        public World() { GenerateLevel(); }

        /* Properties */
        /// <summary> The array of tiles representing the current floor of the game. </summary>
        public Tile[,] Data { get; set; } = new Tile[Program.tilesHigh - 5, Program.tilesWide - 16];
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
            for(int y = 0; y < Program.tilesHigh - 5; ++y) {
                for(int x = 0; x < Program.tilesWide - 16; ++x) {
                    Program.canvas.Remove(Data[y, x]);
                }
            }
            for(int y = 0; y < Program.tilesHigh - 5; ++y) {
                for(int x = 0; x < Program.tilesWide - 16; ++x) {
                    Tile empty = new Tile(Properties.Resources.Empty, x * Program.spriteWidth, y * Program.spriteHeight);
                    Data[y, x] = empty;
                }
            }
            for(int i = 0; i < 10; ++i) {
                int roomX = Engine.rand.Next(0, 53); // max width of room is 10
                int roomY = Engine.rand.Next(0, 29); // max height of room is 10
                int roomW = Engine.rand.Next(4, 10);
                int roomH = Engine.rand.Next(4, 10);
                for(int y = roomY; y <= roomY + roomH; ++y) {
                    for(int x = roomX; x <= roomX + roomW; ++x) {
                        if((y != roomY && y != roomY + roomH) && (x != roomX && x != roomX + roomW)) {
                            Tile floor = new Tile(Properties.Resources.Empty, x * Program.spriteWidth, y * Program.spriteHeight);
                            Data[y, x] = floor;
                            continue;
                        }
                        Tile wall = new GridRL.Tile(Properties.Resources.At, x * Program.spriteWidth, y * Program.spriteHeight);
                        Data[y, x] = wall;
                    }
                }
            }
            for(int y = 0; y < Program.tilesHigh - 5; ++y) {
                for(int x = 0; x < Program.tilesWide - 16; ++x) {
                    Program.canvas.Add(Data[y, x]);
                }
            }
        }
    }
}

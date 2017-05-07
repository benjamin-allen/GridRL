namespace GridRL {

    /// <summary> A class containing an array of tiles and a level counter. </summary>
    public class World {
        /* Constructors */
        public World() { }

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
        public virtual void GenerateLevel() {
            // TODO: Add worldgen logic here
        }
    }
}

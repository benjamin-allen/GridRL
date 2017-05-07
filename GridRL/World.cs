namespace GridRL {
    public class World {
        /* Constructors */
        public World() { }

        /* Properties */
        public Tile[,] Data { get; set; } = new Tile[Program.tilesHigh - 5, Program.tilesWide - 16];
        public int Level { get; set; } = 1;

        public Tile this[int y, int x] {
            get { return Data[y, x]; }
            set { Data[y, x] = value; }
        }

        /* Methods */
        public virtual void GenerateLevel() {
            // TODO: Add worldgen logic here
        }

        /* Overrides */
    }
}

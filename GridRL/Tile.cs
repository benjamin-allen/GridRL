using System.Drawing;

namespace GridRL {

    /// <summary> The basic definition of tiles, which make up a world. </summary>
    public class Tile : ImageSprite{
        /* Constructors */
        public Tile() { }
        
        /// <summary> Constructs a tile with the given image. </summary>
        /// <param name="image"> The image used for this tile's sprite. </param>
        public Tile(Image image) : base(image) { }

        /// <summary> Constructs a tile with the given image and position. </summary>
        /// <param name="image">The image used for this tile's sprite. </param>
        /// <param name="x"> The horizontal position of the tile in the world data. </param>
        /// <param name="y"> The vertical position of the tile in the world data. </param>
        public Tile(Image image, int x, int y) : base(image) {
            CoordX = x;
            CoordY = y;
        }


        /* Properties */
        /// <summary> Property representing whether creatures and items can be on the tile. </summary>
        public bool IsWalkable { get; set; }

        /// <summary> Property representing whether the tile can be seen by the player. </summary>
        public bool IsVisible { get; set; }

        /// <summary> The name of the tile. </summary>
        public string Name { get; set; }

        /// <summary> An extended flavor description of the tile. </summary>
        public string Description { get; set; }

        /// <summary> Identifies tiles connected to each other. </summary>
        public int Region { get; set; }

        private int coordX;
        private int coordY;

        /// <summary> The vertical index of the tile in the world's data.</summary>
        public int CoordX {
            get { return coordX; }
            set { coordX = value; X = value * Engine.spriteWidth; }
        }

        /// <summary> The vertical index of the tile in the world's data.</summary>
        public int CoordY {
            get { return coordY; }
            set { coordY = value; Y = value * Engine.spriteHeight; }
        }


        /* Methods */
        /// <summary> Called when a tile is stepped on. </summary>
        /// <param name="s">The sprite that stepped on this tile. </param>
        public virtual void OnStepOn(Sprite s) { }
    }

    public class Corridor : Tile {
        /* Constructors */
        public Corridor(int x, int y, int region) : base(Properties.Resources.At, x, y) {
            Name = "Corridor";
            Description = "A darkened hallway that connects the many rooms of the dungeon.";
            IsWalkable = true;
            Region = region;
        }
    }

    public class RoomFloor : Tile {
        /* Constructors */
        public RoomFloor(int x, int y, int region) : base(Properties.Resources.Empty, x, y) {
            Name = "Floor";
            Description = "A tiled floor, cracked and worn from years of neglect.";
            IsWalkable = true;
            Region = region;
        }
    }
}

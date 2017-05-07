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
        /// <param name="x"> The horizontal position of the tile. </param>
        /// <param name="y"> The vertical position of the tile. </param>
        public Tile(Image image, float x, float y) : base(image, x, y) { }


        /* Properties */
        /// <summary> Property representing whether creatures and items can be on the tile. </summary>
        public bool IsWalkable { get; set; }

        /// <summary> Property representing whether the tile can be seen by the player. </summary>
        public bool IsVisible { get; set; }

        /// <summary> The name of the tile. </summary>
        public string Name { get; set; }

        /// <summary> An extended flavor description of the tile. </summary>
        public string Description { get; set; }


        /* Methods */
        /// <summary> Called when a tile is stepped on. </summary>
        /// <param name="s">The sprite that stepped on this tile. </param>
        public virtual void OnStepOn(Sprite s) { }
    }
}

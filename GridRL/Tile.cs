using System.Drawing;

namespace GridRL {
    public class Tile : ImageSprite{
        /* Constructors */
        public Tile() { }
        
        public Tile(Image image) : base(image) { }

        public Tile(Image image, float x, float y) : base(image, x, y) { }


        /* Properties */
        public bool IsWalkable { get; set; }
        public bool IsVisible { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        /* Methods */
        public virtual void OnStepOn(Sprite s) { }

        /* Overrides */
    }
}

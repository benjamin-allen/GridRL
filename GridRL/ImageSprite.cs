using System.Drawing;

namespace GridRL {
    public class ImageSprite : Sprite {
        /* Constructors */
        public ImageSprite() : base() { }
        public ImageSprite(Image image) : base() {
            Image = image;
            Width = Image.Width;
            Height = Image.Height;
        }
        public ImageSprite(Image image, double x, double y) : base() {
            Image = image;
            X = x;
            Y = y;
            Width = Image.Width;
            Height = Image.Height;
        }


        /* Properties */
        public Image Image { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }


        /* Overrides */
        protected override void Paint(Graphics g) {
            g.DrawImage(Image, 0, 0);
        }
    }
}

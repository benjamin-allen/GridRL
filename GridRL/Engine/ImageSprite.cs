using System.Drawing;

namespace GridRL {
    /// <summary> An extension of <c>Sprite</c> with support for images. </summary>
    public class ImageSprite : Sprite {
        #region Constructors

        public ImageSprite() : base() { }

        /// <summary> Constructs a new ImageSprite with a known image. </summary>
        /// <param name="image"> The image of the sprite. </param>
        public ImageSprite(Image image) : base() {
            Image = image;
            Width = Image.Width;
            Height = Image.Height;
        }

        /// <summary> Constructs a new ImageSprite with a known image and position. </summary>
        /// <param name="image"> The image of the sprite. </param>
        /// <param name="x"> The horizontal position of the sprite. </param>
        /// <param name="y"> The vertical position</param>
        public ImageSprite(Image image, float x, float y) : base() {
            Image = image;
            X = x;
            Y = y;
            Width = Image.Width;
            Height = Image.Height;
        }

        #endregion
        #region Properties

        /// <summary> The image representing this sprite. </summary>
        public Image Image { get; set; }

        /// <summary> The width of this sprite. Set automatically. </summary>
        public int Width { get; set; }

        /// <summary> The height of this sprite. Set automatically. </summary>
        public int Height { get; set; }

        #endregion
        #region Overrides

        protected override void Paint(Graphics g) {
            g.DrawImage(Image, 0, 0);
        }

        #endregion
    }
}

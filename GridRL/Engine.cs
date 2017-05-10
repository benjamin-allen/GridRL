using System;
using System.Windows.Forms;

namespace GridRL {
    public partial class Engine : Form {
        public static Form form;
        public static Sprite canvas = new Sprite();
        public static int spriteWidth = 16;
        public static int spriteHeight = 16;
        public static int tilesWide = 80;
        public static int tilesHigh = 45;
        public static Random rand = new Random(1);

        public Engine() {
            form = this;
            InitializeComponent();
            DoubleBuffered = true;
            form.ClientSize = new System.Drawing.Size(spriteWidth * tilesWide, spriteHeight * tilesHigh);

            // main menu function goes here
            // if main menu returns something, start the game loop
            // otherwise spawn some help
            // otherwise exit the game
        }

        protected override void OnPaint(PaintEventArgs e) {
            canvas.Render(e.Graphics);
        }
    }
}

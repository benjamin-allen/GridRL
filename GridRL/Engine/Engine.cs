using System;
using System.Windows.Forms;

namespace GridRL {
    public partial class Engine : Form {
        #region Constructors

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

        #endregion
        #region Properties

        public static Form form;
        public static Sprite canvas = new Sprite();
        public static int spriteWidth = 16;
        public static int spriteHeight = 16;
        public static int tilesWide = 80;
        public static int tilesHigh = 45;
        public static Random rand = new Random(1);
        public static int turnCount = 0;
        public static bool start = false;

        #endregion
        #region Overrides

        protected override void OnPaint(PaintEventArgs e) {
            if(!start) {
                e.Graphics.DrawImage(Properties.Resources.StartMenu,0,0, ClientSize.Width, ClientSize.Height);
            }
            else {
                canvas.Render(e.Graphics);
            }
        }

        #endregion
    }
}

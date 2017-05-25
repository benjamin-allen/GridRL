using System;
using System.Drawing;
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
        public static bool WinGame = false;
        public static bool LoseGame = false;

        #endregion
        #region Overrides

        protected override void OnPaint(PaintEventArgs e) {
            if(!start) {
                e.Graphics.DrawImage(Properties.Resources.StartMenu,0,0, ClientSize.Width, ClientSize.Height);
            }
            else if(!WinGame && !LoseGame){
                canvas.Render(e.Graphics);
            }
            else {
                e.Graphics.DrawImage(Properties.Resources.Credits, 0, 0, ClientSize.Width, ClientSize.Height);
                Font font = new Font("Algerian", 100);
                if(WinGame) {
                    e.Graphics.DrawString("You Win!",font, Brushes.Black, ClientSize.Width/4 + 50, 200);
                }
                else {
                    e.Graphics.DrawString("You Lose!", font, Brushes.Black, ClientSize.Width / 4 + 10, 200);
                }
            }
        }

        #endregion
    }
}

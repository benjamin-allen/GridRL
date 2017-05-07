using System.Windows.Forms;

namespace GridRL {
    public partial class Engine : Form {
        public static Form form;
        public static Sprite canvas = new Sprite();
        public static int spriteWidth = 16;
        public static int spriteHeight = 16;
        public static int tilesWide = 80;
        public static int tilesHigh = 45;
        public static World world = new World();

        public Engine() {
            form = this;
            InitializeComponent();
            DoubleBuffered = true;
            form.ClientSize = new System.Drawing.Size(spriteWidth * tilesWide, spriteHeight * tilesHigh);
            for(int y = 0; y < tilesHigh - 5; ++y) {
                for(int x = 0; x < tilesWide - 16; ++x) {
                    world.Data[y, x] = new Tile(Properties.Resources.at, x * spriteWidth, y * spriteHeight);
                    canvas.Add(world.Data[y, x]);
                }
            }
            // main menu function goes here
            // if main menu returns something, start the game loop
            // otherwise spawn some help
            // otherwise exit the game
        }

        public static void gameLoop() {
            // call on player input
            // update player
            // update enemies
            // render

            canvas.Update();
            form.Refresh();
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            if(e.KeyCode == Keys.Escape)
                Application.Exit();
            gameLoop();
        }

        protected override void OnPaint(PaintEventArgs e) {
            canvas.Render(e.Graphics);
        }
    }
}

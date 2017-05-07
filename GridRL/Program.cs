using System.Windows.Forms;

namespace GridRL {
    public class Program : Engine {
        public static World world = new World();

        static void Main() {
            for(int y = 0; y < tilesHigh - 5; ++y) {
                for(int x = 0; x < tilesWide - 16; ++x) {
                    world.Data[y, x] = new Tile(Properties.Resources.at, x * spriteWidth, y * spriteHeight);
                    canvas.Add(world.Data[y, x]);
                }
            }
            Application.Run(new Program());
        }

        public static void GameLoop() {
            // call on player input
            // update player
            // update enemies
            // render

            canvas.Update();
            form.Refresh();
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            if(e.KeyCode == Keys.Escape) { Sprite innerCanvas = new Sprite(canvas); }
            GameLoop();
        }
    }
}

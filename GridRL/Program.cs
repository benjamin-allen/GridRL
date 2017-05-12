using System.Windows.Forms;
using System;

namespace GridRL {
    public class Program : Engine {
        public static Player player = new Player(0, 0);
        public static World world = new World();

        static void Main() {
            Application.Run(new Program());
        }

        public static void GameLoop() {
            turnCount++;
            // update enemies
            // render
            Console.WriteLine(turnCount);
            canvas.Update();
            form.Refresh();
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            if(e.KeyCode == Keys.Escape) { world.GenerateLevel(); }
            if(player.HandleGameInput(e)) {
                GameLoop();
            }
        }
    }
}

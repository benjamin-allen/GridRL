using System.Windows.Forms;
using System.Collections.Generic;
using System;

namespace GridRL {
    public class Program : Engine {
        public static Player player = new Player(0, 0);
        public static World world = new World();

        static void Main() {
            world.GenerateLevel();
            Application.Run(new Program());
        }

        public static void GameLoop() {
            turnCount++;
            // update enemies
            // render
            canvas.Update();
            form.Refresh();
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            if(e.KeyCode == Keys.Escape) { world.GenerateLevel(); GameLoop(); }
            if(player.HandleGameInput(e)) {
                GameLoop();
            }
        }

        public static void Shuffle<T>(List<T> list) {
            int n = list.Count;
            for(int i = 0; i < n; i++) {
                int r = i + (int)(rand.NextDouble() * (n - i));
                T t = list[r];
                list[r] = list[i];
                list[i] = t;
            }
        }
    }
}

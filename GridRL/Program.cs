﻿using System.Windows.Forms;

namespace GridRL {
    public class Program : Engine {
        public static Player player = new Player(0, 0);
        public static World world = new World();

        static void Main() {
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
            if(e.KeyCode == Keys.Escape) { world.GenerateLevel(); }
            if(player.HandleGameInput(e)) {
                GameLoop();
            }
        }
    }
}

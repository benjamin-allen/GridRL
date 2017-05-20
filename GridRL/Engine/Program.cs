using System.Windows.Forms;
using System.Collections.Generic;
using System;

namespace GridRL {
    public class Program : Engine {
        public static Player player = new Player(0, 0);
        public static World world = new World();
        public static int waitState = 0;
        public static Direction lastDirection = Direction.None;

        static void Main() {
            world.GenerateLevel();
            Application.Run(new Program());
        }

        public static void GameLoop() {
            turnCount++;
            canvas.Update();
            form.Refresh();
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            Keys kc = e.KeyCode;
            if(waitState != 0) {
                if(kc == Keys.Up || kc == Keys.Down || kc == Keys.Left || kc == Keys.Right || 
                kc == Keys.NumPad8 || kc == Keys.NumPad2 || kc == Keys.NumPad4 || kc == Keys.NumPad6) {
                    waitState = 0;
                    Direction d = player.KeyPressToDirection(e);
                    lastDirection = d;
                }
                else if(kc == Keys.Escape) {
                    waitState = -1;
                }
            }
            else if(player.HandleGameInput(e)) {
                GameLoop();
            }
            else {
                waitState = 0;
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

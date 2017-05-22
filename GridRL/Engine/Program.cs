using System.Windows.Forms;
using System.Collections.Generic;
using System;

namespace GridRL {
    public class Program : Engine {
        #region Properties

        public static Player player = new Player(0, 0);
        public static ScreenOutput console = new ScreenOutput();
        public static Sidebar sidebar = new Sidebar();
        public static World world = new World();
        public static int[] GridMouseCoords = new int[2];
        public static int waitState = 0;
        public static Direction lastDirection = Direction.None;

        #endregion
        #region Methods

        static void Main() {
            world.GenerateLevel();
            canvas.Add(console);
            canvas.Add(sidebar);
            Application.Run(new Program());
        }

        public static void GameLoop() {
            turnCount++;
            canvas.Update();
            form.Refresh();
        }

        public static int[] GetMouseGridCoords(MouseEventArgs g) {

            int[] coords = new int[2];
            //X position checks
            if(g.X < 1160) {
               coords[1] = 0;
            }
            else if(g.X < 1240) {
                coords[1] = 1;
            }
            else {
                coords[1] = 2;
            }

            //Y position checks
            if(g.Y < 530) {
                coords[0] = 0;
            }
            else if(g.Y < 610) {
                coords[0] = 1;
            }
            else {
                coords[0] = 2;
            }
            return coords;
        }

        #endregion
        #region Overrides

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

        protected override void OnMouseClick(MouseEventArgs e) {
            base.OnMouseClick(e);
            if(e.X > 1080 && e.X < 1320 && e.Y > 450 && e.Y < 690) {
                GridMouseCoords = GetMouseGridCoords(e);
            }
        }

        #endregion
        #region Utilities

        public static void Shuffle<T>(List<T> list) {
            int n = list.Count;
            for(int i = 0; i < n; i++) {
                int r = i + (int)(rand.NextDouble() * (n - i));
                T t = list[r];
                list[r] = list[i];
                list[i] = t;
            }
        }

        #endregion
    }
}

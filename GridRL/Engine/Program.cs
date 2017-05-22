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
        public static int[] PlrInvMouseCoords = new int[2];
        public static int[] TileInvMouseCoords = new int[2];
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

        public static int[] GetMouseGridCoords(MouseEventArgs g, int option) {
            int[] coords = new int[2];
            if(option == 0) {
                //Ability Grid Checks
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
            }

            if(option == 1) {
                //Player Inventory Check
                //X coord checks
                if(g.X < 1112) {
                    coords[1] = 0;
                }
                else if(g.X < 1144) {
                    coords[1] = 1;
                }
                else if(g.X < 1176) {
                    coords[1] = 2;
                }
                else if(g.X < 1208) {
                    coords[1] = 3;
                }
                else if(g.X < 1240) {
                    coords[1] = 4;
                }
                else if(g.X < 1272) {
                    coords[1] = 5;
                }
                else if(g.X < 1304) {
                    coords[1] = 6;
                }
                else if(g.X < 1336) {
                    coords[1] = 7;
                }
                else if(g.X < 1368) {
                    coords[1] = 8;
                }
                else{
                    coords[1] = 9;
                }

                //Y coord checks
                if(g.Y < 202) {
                    coords[0] = 0;
                }
                else {
                    coords[0] = 1;
                }

            }

            if(option == 2) {
                //Tile Inventory Check
                //X coord checks
                if(g.X < 1112) {
                    coords[1] = 0;
                }
                else if(g.X < 1144) {
                    coords[1] = 1;
                }
                else if(g.X < 1176) {
                    coords[1] = 2;
                }
                else if(g.X < 1208) {
                    coords[1] = 3;
                }
                else if(g.X < 1240) {
                    coords[1] = 4;
                }
                else if(g.X < 1272) {
                    coords[1] = 5;
                }
                else if(g.X < 1304) {
                    coords[1] = 6;
                }
                else if(g.X < 1336) {
                    coords[1] = 7;
                }
                else if(g.X < 1368) {
                    coords[1] = 8;
                }
                else {
                    coords[1] = 9;
                }

                //Y coord checks
                if(g.Y < 202) {
                    coords[0] = 0;
                }
                else {
                    coords[0] = 1;
                }
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
            //Check Ability Grid clicks
            if(e.X > 1080 && e.X < 1320 && e.Y > 450 && e.Y < 690) {
                GridMouseCoords = GetMouseGridCoords(e, 0);
            }

            //Check Player inventory clicks
            if(e.X > 1080 && e.X < 1320 && e.Y > 170 && e.Y < 234) {
                PlrInvMouseCoords = GetMouseGridCoords(e, 1);
                Console.WriteLine("Mouse Coords: (" + PlrInvMouseCoords[0] + "," + PlrInvMouseCoords[1] + ")");
            }

            //Check Tile inventory clicks
            if(e.X > 1080 && e.X < 1320 && e.Y > 270 && e.Y < 334) {
                TileInvMouseCoords = GetMouseGridCoords(e, 2);
                Console.WriteLine("Mouse Coords: (" + TileInvMouseCoords[0] + "," + TileInvMouseCoords[1] + ")");
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

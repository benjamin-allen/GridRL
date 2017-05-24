using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;

namespace GridRL {
    public enum MouseArea { Hidden, Sidebar, HoldBox, WearBox, WieldBox, PlayerInv, TileInv, World, Grid, Console}

    public class Program : Engine {
        #region Properties

        public static Player player = new Player(0, 0);
        public static ScreenOutput console = new ScreenOutput();
        public static Sidebar sidebar = new Sidebar();
        public static World world = new World();
        public static int[] MouseCoords = new int[2];
        public static int[] GridMouseCoords = new int[2];
        public static int[] PlrInvMouseCoords = new int[2];
        public static int[] TileInvMouseCoords = new int[2];
        public static int waitState = 0;
        public static Direction lastDirection = Direction.None;
        public static ColorMatrix grayMatrix = new ColorMatrix(new float[][] { new float[]{1, 0, 0, 0, 0},
                                                                               new float[]{0, 1, 0, 0, 0},
                                                                               new float[]{0, 0, 1, 0, 0},
                                                                               new float[]{0, 0, 0, .25f, 0},
                                                                               new float[]{0, 0, 0, 0, 1} });
        public static ImageAttributes gray = new ImageAttributes();
        public static MouseArea MA = MouseArea.Hidden;
        public static string HoverString;

        #endregion
        #region Methods

        static void Main() {
            gray.SetColorMatrix(grayMatrix);
            world.GenerateLevel();
            canvas.Add(console);
            canvas.Add(sidebar);
            world.UpdateVisibles();
            Application.Run(new Program());
        }

        public static void GameLoop() {
            turnCount++;
            canvas.Update();
            world.CreaturesToRemove = new List<Creature>();
            world.EffectsToRemove = new List<Effect>();
            world.UpdateVisibles();
            form.Refresh();
        }

        private static void setMouseArea() {
            int y = MouseCoords[0];
            int x = MouseCoords[1];
            if(x < 65) {
                if(y < 41) {
                    MA = MouseArea.World;
                }
                else {
                    MA = MouseArea.Console;
                }
            }
            else if(x >= sidebar.CellsX && x < sidebar.CellsX + 11) {
                if(x == 74) {
                    if(y == 18) {
                        MA = MouseArea.HoldBox;
                    }
                    else if(y == 20) {
                        MA = MouseArea.WearBox;
                    }
                    else if(y == 22) {
                        MA = MouseArea.WieldBox;
                    }
                }
                else {
                    MA = MouseArea.Sidebar;
                }
                if(y >= sidebar.CellsY && y < sidebar.CellsY + 2) {
                    MA = MouseArea.PlayerInv;
                    PlrInvMouseCoords[0] = y - sidebar.CellsY;
                    PlrInvMouseCoords[1] = x - sidebar.CellsX;
                }
                else if(y >= sidebar.CellsY2 && y < sidebar.CellsY2 + 2) {
                    MA = MouseArea.TileInv;
                    TileInvMouseCoords[0] = y - sidebar.CellsY2;
                    TileInvMouseCoords[1] = x - sidebar.CellsX;
                }
                else if(x >= sidebar.GridX && x < sidebar.GridX + 9 && y >= sidebar.GridY && y < sidebar.GridY + 9)  {
                    MA = MouseArea.Grid;
                    GridMouseCoords[0] = (y - sidebar.GridY) / 3;
                    GridMouseCoords[1] = (x - sidebar.GridX) / 3;
                }
            }
            else {
                MA = MouseArea.Sidebar;
            }
            HoverString = MA.ToString();
        }
        #endregion
        #region Overrides

        protected override void OnKeyDown(KeyEventArgs e) {
            Keys kc = e.KeyCode;
            // Do this if we're waiting for key input. 
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
            else if(player.HandleKeyInput(e)) {
                GameLoop();
            }
            else {
                waitState = 0;
            }
        }

        protected override void OnMouseClick(MouseEventArgs e) {
            MouseCoords[0] = (int)Math.Floor(e.Y / 16f);
            MouseCoords[1] = (int)Math.Floor(e.X / 16f);
            Console.WriteLine("(" + MouseCoords[0] + "," + MouseCoords[1] + ")");
            setMouseArea();
            form.Refresh();
            if(player.HandleMouseInput(e)) {
                GameLoop();
            }
        }

        #endregion
        #region Utilities

        /// <summary> Shuffles a list. </summary>
        /// <typeparam name="T"> Template type parameter. </typeparam>
        /// <param name="list"> The list to be shuffle. </param>
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

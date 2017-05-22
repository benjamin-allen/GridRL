using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing.Imaging;

namespace GridRL {
    public class Program : Engine {
        #region Properties

        public static Player player = new Player(0, 0);
        public static World world = new World();
        public static int waitState = 0;
        public static Direction lastDirection = Direction.None;
        public static ColorMatrix grayMatrix = new ColorMatrix(new float[][] { new float[]{1, 0, 0, 0, 0},
                                                                               new float[]{0, 1, 0, 0, 0},
                                                                               new float[]{0, 0, 1, 0, 0},
                                                                               new float[]{0, 0, 0, .25f, 0},
                                                                               new float[]{0, 0, 0, 0, 1} });
        public static ImageAttributes gray = new ImageAttributes();

        #endregion
        #region Methods

        static void Main() {
            gray.SetColorMatrix(grayMatrix);
            world.GenerateLevel();
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
            else if(player.HandleGameInput(e)) {
                GameLoop();
            }
            else {
                waitState = 0;
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

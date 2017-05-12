using System.Drawing;
using System.Windows.Forms;

namespace GridRL {
    /// <summary> Possible implementation: enumeration of creature's AI type. </summary>
    //public enum AIType { }

    /// <summary>A base class for all living things.</summary>
    public class Creature : Actor {
        /* Constructors */
        /// <summary> A constructor that creates a creature with a given image and x/y coordinates. </summary>
        /// <param name="image"> The image used for this creature's sprite. </param>
        /// <param name="y"> The creature's Y position on the world data. </param>
        /// <param name="x"> The creature's X position on the world data. </param>
        public Creature(Image image, int y, int x) : base(image, y, x) {
            Name = "Dummy Creature";
            Description = "If you can see this, file a bug report for an improperly initialized creature.";
            DeathMessage = "The " + Name + " dies!";
            IsCollidable = true;
        }


        /* Properties */

        /// <summary> Creature's HP stat. </summary>
        public int Health { get; set; } = 0;

        /// <summary> Creature's Attack stat. </summary>
        public int Attack { get; set; } = 0;

        /// <summary> Creature's Defense stat. </summary>
        public int Defense { get; set; } = 0;

        public string DeathMessage { get; set; }

        /* Methods */

        //Possible override base.Remove() for onDeath message of some kind.
    }


    public class Player : Creature {
        /* Constructors */
        public Player(int y, int x) : base(Properties.Resources.Player, y, x) {
            Name = "player";
            Description = "You don't have a mirror...";
            DeathMessage = "The " + Name + "dies!";
            Health = 10;
            Attack = 10;
            Defense = 10;
            IsVisible = true;
        }

        /* Methods */
        public bool HandleGameInput(KeyEventArgs e) {
            if(e.KeyCode == Keys.Up || e.KeyCode == Keys.NumPad8) {
                if(Program.world.Data[CoordY - 1, CoordX] != null && Program.world.Data[CoordY - 1, CoordX].IsWalkable) {
                    CoordY -= 1;
                    return true;
                }
            }
            else if(e.KeyCode == Keys.Down || e.KeyCode == Keys.NumPad2) {
                if(Program.world.Data[CoordY + 1, CoordX] != null && Program.world.Data[CoordY + 1, CoordX].IsWalkable) {
                    CoordY += 1;
                    return true;
                }
            }
            else if(e.KeyCode == Keys.Left || e.KeyCode == Keys.NumPad4) {
                if(Program.world.Data[CoordY, CoordX - 1] != null && Program.world.Data[CoordY, CoordX - 1].IsWalkable) {
                    CoordX -= 1;
                    return true;
                }
            }
            else if(e.KeyCode == Keys.Right || e.KeyCode == Keys.NumPad6) {
                if(Program.world.Data[CoordY, CoordX + 1] != null && Program.world.Data[CoordY, CoordX + 1].IsWalkable) {
                    CoordX += 1;
                    return true;
                }
            }
            return false;
        }
    }
}

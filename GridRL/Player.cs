using System.Windows.Forms;
using System.Collections.Generic;

namespace GridRL {
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
            if(e.KeyCode == Keys.Up || e.KeyCode == Keys.NumPad8 || e.KeyCode == Keys.Down || e.KeyCode == Keys.NumPad2
            || e.KeyCode == Keys.Left || e.KeyCode == Keys.NumPad4 || e.KeyCode == Keys.Right || e.KeyCode == Keys.NumPad6) {
                Direction dir = KeyPressToDirection(e);
                if(CanAccess(dir)) {
                    List<int> points = DirectionToPoints(dir);
                    foreach(Creature c in Program.world.Creatures) {
                        if(c != this && WillCollideWith(c) && c.CoordY == CoordY && c.CoordX == CoordX) {
                            c.OnCollide(this);
                            return true;
                        }
                    }
                    if(WillCollideWith(Program.world.Data[points[0], points[1]])) {
                        Program.world.Data[points[0], points[1]].OnCollide(this);
                        return true;
                    }
                    else if(Program.world.Data[points[0], points[1]].IsWalkable) {
                        CoordY = points[0];
                        CoordX = points[1];
                        Program.world.Data[points[0], points[1]].OnStepOn(this);
                        return true;
                    }
                }
            }
            return false;
        }


        private Direction KeyPressToDirection(KeyEventArgs e) {
            if(e.KeyCode == Keys.Up || e.KeyCode == Keys.NumPad8) {
                return Direction.Up;
            }
            else if(e.KeyCode == Keys.Down || e.KeyCode == Keys.NumPad2) {
                return Direction.Down;
            }
            else if(e.KeyCode == Keys.Left || e.KeyCode == Keys.NumPad4) {
                return Direction.Left;
            }
            else {
                return Direction.Right;
            }
        }
    }
}

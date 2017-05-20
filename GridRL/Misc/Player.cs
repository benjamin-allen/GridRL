using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System;

namespace GridRL {
    public class Player : Creature {
        #region Constructors

        /// <summary> Creates a controllable creature. </summary>
        /// <param name="y"> The Y coordinate to place the player in the world. </param>
        /// <param name="x"> The X coordinate to place the player in the world. </param>
        public Player(int y, int x) : base(Properties.Resources.Player, y, x) {
            Name = "player";
            Description = "You don't have a mirror...";
            DeathMessage = "The " + Name + "dies!";
            Health = 10;
            Attack = 10;
            Defense = 10;
            IsVisible = true;
            Abilities.Add(new Fireball());
        }

        #endregion
        #region Methods

        /// <summary> Top-level function to capture and pass input to their sub-functions. </summary>
        /// <param name="e"> The KeyEventArgs thing. </param>
        /// <returns> A boolean indicating whether to advance the game. </returns>
        public bool HandleGameInput(KeyEventArgs e) {
            if(e.KeyCode == Keys.Up || e.KeyCode == Keys.NumPad8 || e.KeyCode == Keys.Down || e.KeyCode == Keys.NumPad2
            || e.KeyCode == Keys.Left || e.KeyCode == Keys.NumPad4 || e.KeyCode == Keys.Right || e.KeyCode == Keys.NumPad6) {
                Direction dir = KeyPressToDirection(e);
                // Check if access is possible from this actor
                if(CanAccess(dir)) {
                    List<int> points = DirectionToPoints(dir);
                    // Check for collision with any creatures
                    foreach(Creature c in Program.world.Creatures) {
                        if(c != this && WillCollideWith(c) && c.CoordY == points[0] && c.CoordX == points[1]) {
                            //c.OnCollide(this);
                            PerformAttack(c);
                            return true;
                        }
                    }
                    // Check for collision with world structures
                    if(WillCollideWith(Program.world.Data[points[0], points[1]])) {
                        Program.world.Data[points[0], points[1]].OnCollide(this);
                        return true;
                    }
                    // Check to see if the world structure can be walked on
                    else if(Program.world.Data[points[0], points[1]].IsWalkable) {
                        CoordY = points[0];
                        CoordX = points[1];
                        Program.world.Data[points[0], points[1]].OnStepOn(this);
                        return true;
                    }
                }
            }
            else if(e.KeyCode == Keys.NumPad5 || e.KeyCode == Keys.OemPeriod) {
                return true;
            }
            else if(e.KeyCode == Keys.A) {
                return Abilities[0].Use(this);
            }
            else if(e.KeyCode == Keys.G) {
                Item i = Program.world[CoordY, CoordX].Inventory.Items.FirstOrDefault();
                if(i != null) {
                    PickUp(i);
                    Console.WriteLine("You pick up the " + i.Name + ".");
                }
            }
            return false;
        }

        /// <summary> Converts a keypress to a direction. </summary>
        /// <param name="e"> KeyEventArgs thingy. </param>
        /// <returns> The direction to go. </returns>
        public Direction KeyPressToDirection(KeyEventArgs e) {
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

        #endregion
    }
}

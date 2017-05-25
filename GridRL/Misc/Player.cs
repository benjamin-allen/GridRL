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
            Health = 30;
            Attack = 10;
            Defense = 10;
            Visibility = Vis.Visible;
            Abilities.Add(new FlameBurst());
            AttackBoost a = new AttackBoost(this);
            a.GridY = 1;
            a.GridX = 1;
            Abilities.Add(a);
        }

        #endregion
        #region Methods

        /// <summary> Top-level function to capture and pass input to their sub-functions. </summary>
        /// <param name="e"> The KeyEventArgs thing. </param>
        /// <returns> A boolean indicating whether to advance the game. </returns>
        public bool HandleKeyInput(KeyEventArgs e) {
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
            else if(e.KeyCode == Keys.B) {
                Abilities[1].OnAddToGrid();
                return false;
            }
            else if(e.KeyCode == Keys.C) {
                Abilities[1].OnRemoveFromGrid();
                return false;
            }
            else if(e.KeyCode == Keys.G) {
                Item i = Program.world[CoordY, CoordX].Inventory.Items.FirstOrDefault();
                if(i != null) {
                    PickUp(i);
                    Program.console.SetText("You pick up the " + i.Name + ".");
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

        public bool HandleMouseInput(MouseEventArgs e) {
            int pY = CoordY;
            int pX = CoordX;
            if(Program.MA == MouseArea.TileInv) {
                #region TileInv
                int index = (Program.TileInvClickCoords[0] * 11) + Program.TileInvClickCoords[1];
                // get the item from the inv
                Item toPickUp = Program.world.Data[pY, pX].Inventory.Items[index];
                // if it's not null, try to add
                if(toPickUp != null) {
                    if(Inventory.AddItem(toPickUp)) {
                        Program.world.Data[pY, pX].Inventory.RemoveItem(toPickUp);
                        Program.console.SetText("You pick up the " + toPickUp.Name + ".");
                        return true;
                    }
                }
                #endregion
            }
            else if(Program.MA == MouseArea.PlayerInv) {
                #region PlayerInv
                // check for item relevance
                int index = (Program.PlayerInvClickCoords[0] * 11) + Program.PlayerInvClickCoords[1];
                Item toWorkWith = Inventory.Items[index];
                if(toWorkWith == null) {
                    return false;
                }
                // Wait for the next mouseclick
                Program.waitState = 2;
                while(Program.waitState == 2) {
                    Application.DoEvents();
                }
                if(Program.waitState == -1) {
                    Program.LastMA = MouseArea.Hidden;
                    return false;
                }
                if(Program.MA == MouseArea.Grid || Program.MA == MouseArea.PlayerInv) {
                    Program.LastMA = MouseArea.Hidden;
                    return false;
                }
                if(Program.MA == MouseArea.TileInv) {  // DROP ITEM
                    if(Program.world[pY, pX].Inventory.AddItem(toWorkWith)) {
                        Inventory.RemoveItem(toWorkWith);
                        Program.console.SetText("You drop the " + toWorkWith.Name + ".");
                        return true;
                    }
                }
                else if(Program.MA == MouseArea.HoldBox) {
                    if(HeldItem == null) {
                        HeldItem = toWorkWith;
                        Inventory.RemoveItem(HeldItem);
                        Program.console.SetText("You grasp the " + HeldItem.Name + " in your off hand.");
                        return true;
                    }
                    else {
                        if(Inventory.AddItem(HeldItem)) {
                            HeldItem = toWorkWith;
                            Inventory.RemoveItem(HeldItem);
                            Program.console.SetText("You shuffle some items in your pack.");
                            return true;
                        }
                        else if(Program.world[pY, pX].Inventory.AddItem(HeldItem)) {
                            HeldItem = toWorkWith;
                            Inventory.RemoveItem(HeldItem);
                            Program.console.SetText("You lose your grip and some things tumble to the floor.");
                            return true;
                        }
                        else {
                            Program.console.SetText("You're without a place to put this item!");
                        }
                    }
                }
                else if(Program.MA == MouseArea.WearBox) {
                    if(toWorkWith is Armor) {
                        if(WornArmor == null) {
                            WornArmor = (Armor)toWorkWith;
                            Inventory.RemoveItem(WornArmor);
                            Program.console.SetText("You don your " + WornArmor.Name + ".");
                            return true;
                        }
                        else {
                            if(Inventory.AddItem(WornArmor)) {
                                WornArmor = (Armor)toWorkWith;
                                Inventory.RemoveItem(WornArmor);
                                Program.console.SetText("You shuffle some items in your pack.");
                                return true;
                            }
                            else if(Program.world[pY, pX].Inventory.AddItem(WornArmor)) {
                                WornArmor = (Armor)toWorkWith;
                                Inventory.RemoveItem(WornArmor);
                                Program.console.SetText("You lose your grip and some things tumble to the floor.");
                                return true;
                            }
                            else {
                                Program.console.SetText("You're without a place to put this item!");
                            }
                        }
                    }
                    else {
                        Program.console.SetText("You can't wear that!");
                    }
                }
                else if(Program.MA == MouseArea.WieldBox) {
                    if(toWorkWith is Weapon) {
                        if(HeldWeapon == null) {
                            HeldWeapon = (Weapon)toWorkWith;
                            Inventory.RemoveItem(HeldWeapon);
                            Program.console.SetText("You grasp your " + HeldWeapon.Name + ".");
                            return true;
                        }
                        else {
                            if(Inventory.AddItem(HeldWeapon)) {
                                HeldWeapon = (Weapon)toWorkWith;
                                Inventory.RemoveItem(HeldWeapon);
                                Program.console.SetText("You shuffle some items in your pack.");
                                return true;
                            }
                            else if(Program.world[pY, pX].Inventory.AddItem(HeldWeapon)) {
                                HeldWeapon = (Weapon)toWorkWith;
                                Inventory.RemoveItem(HeldWeapon);
                                Program.console.SetText("You lose your grip and some things tumble to the floor.");
                                return true;
                            }
                            else {
                                Program.console.SetText("You're without a place to put this item!");
                            }
                        }
                    }
                    else {
                        Program.console.SetText("You can't wield that!");
                    }
                }
                #endregion
            }
            else if(Program.MA == MouseArea.HoldBox) {
                #region HoldBox

                if(HeldItem == null) {
                    return false;
                }
                Program.waitState = 2;
                Program.Exception = MouseArea.HoldBox;
                while(Program.waitState == 2) {
                    Application.DoEvents();
                }
                if(Program.waitState == -1) {
                    Program.LastMA = MouseArea.Hidden;
                    return false;
                }
                if(Program.MA == MouseArea.Grid) {
                    Program.LastMA = MouseArea.Hidden;
                    return false;
                }
                else if(Program.MA == MouseArea.HoldBox) {
                    if(HeldItem.Activate(this)) {
                        HeldItem = null;
                        return true;
                    }
                }
                else if(Program.MA == MouseArea.PlayerInv) {
                    if(Inventory.AddItem(HeldItem)) {
                        Program.console.SetText("You put the " + HeldItem.Name + " into your pack.");
                        HeldItem = null;
                        return true;
                    }
                }
                else if(Program.MA == MouseArea.TileInv) {
                    if(Program.world[pY, pX].Inventory.AddItem(HeldItem)) {
                        Program.console.SetText("You place the " + HeldItem.Name + " on the floor below.");
                        HeldItem = null;
                        return true;
                    }
                }
                #endregion
            }
            else if(Program.MA == MouseArea.WearBox) {
                #region WearBox
                if(WornArmor == null) {
                    return false;
                }
                Program.waitState = 2;
                while(Program.waitState == 2) {
                    Application.DoEvents();
                }
                if(Program.waitState == -1) {
                    Program.LastMA = MouseArea.Hidden;
                    return false;
                }
                if(Program.MA == MouseArea.Grid || Program.MA == MouseArea.WearBox) {
                    Program.LastMA = MouseArea.Hidden;
                    return false;
                }
                else if(Program.MA == MouseArea.PlayerInv) {
                    if(Inventory.AddItem(WornArmor)) {
                        Program.console.SetText("You put the " + WornArmor.Name + " into your pack.");
                        WornArmor = null;
                        return true;
                    }
                }
                else if(Program.MA == MouseArea.TileInv) {
                    if(Program.world[pY, pX].Inventory.AddItem(WornArmor)) {
                        Program.console.SetText("You place the " + WornArmor.Name + " on the floor below.");
                        WornArmor = null;
                        return true;
                    }
                }
                else if(Program.MA == MouseArea.HoldBox) {

                }
                #endregion
            }
            else if(Program.MA == MouseArea.WieldBox) {
                #region WieldBox
                if(HeldWeapon == null) {
                    return false;
                }
                Program.waitState = 2;
                while(Program.waitState == 2) {
                    Application.DoEvents();
                }
                if(Program.waitState == -1) {
                    Program.LastMA = MouseArea.Hidden;
                    return false;
                }
                if(Program.MA == MouseArea.Grid || Program.MA == MouseArea.WieldBox) {
                    Program.LastMA = MouseArea.Hidden;
                    return false;
                }
                else if(Program.MA == MouseArea.PlayerInv) {
                    if(Inventory.AddItem(HeldWeapon)) {
                        Program.console.SetText("You put the " + HeldWeapon.Name + " into your pack.");
                        HeldWeapon = null;
                        return true;
                    }
                }
                else if(Program.MA == MouseArea.TileInv) {
                    if(Program.world[pY, pX].Inventory.AddItem(HeldWeapon)) {
                        Program.console.SetText("You place the " + HeldWeapon.Name + " on the floor below.");
                        HeldWeapon = null;
                        return true;
                    }
                }
                #endregion
            }
            else if(Program.MA == MouseArea.Grid) {
                #region Grid
                int y = Program.GridClickCoords[0];
                int x = Program.GridClickCoords[1];
                int index = -1;
                foreach(Ability a in Abilities) {
                    if(y >= a.GridY && y < a.GridY + a.GridHeight && x >= a.GridX && x < a.GridX + a.GridWidth) {
                        index = Abilities.IndexOf(a);
                    }
                }
                if(index != -1) {
                    if(Abilities[index].Use(this)) {
                        return true;
                    }
                }
                #endregion
            }
            return false;
        }
        #endregion
    }
}

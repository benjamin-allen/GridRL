using System.Windows.Forms;

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
            if(e.KeyCode == Keys.Up || e.KeyCode == Keys.NumPad8) {
                if(Program.world.Data[CoordY - 1, CoordX] != null) {
                    foreach(Creature c in Program.world.Creatures) {
                        if(c.CoordY == CoordY - 1 && c.CoordX == CoordX && c.IsCollidable) {
                            // Attack c if c is hostile + do not move
                            return true;
                        }
                    }
                    if(Program.world.Data[CoordY - 1, CoordX].IsCollidable) {
                        Program.world.Data[CoordY - 1, CoordX].OnCollide(this);
                        return true;
                    }
                    else if(Program.world.Data[CoordY - 1, CoordX].IsWalkable) {
                        CoordY -= 1;
                        Program.world.Data[CoordY, CoordX].OnStepOn(this);
                        return true;
                    }
                }
            }
            else if(e.KeyCode == Keys.Down || e.KeyCode == Keys.NumPad2) {
                if(Program.world.Data[CoordY + 1, CoordX] != null) {
                    foreach(Creature c in Program.world.Creatures) {
                        if(c.CoordY == CoordY + 1 && c.CoordX == CoordX && c.IsCollidable) {
                            // Attack c if c is hostile + do not move
                            return true;
                        }
                    }
                    if(Program.world.Data[CoordY + 1, CoordX].IsCollidable) {
                        Program.world.Data[CoordY + 1, CoordX].OnCollide(this);
                        return true;
                    }
                    else if(Program.world.Data[CoordY + 1, CoordX].IsWalkable) {
                        CoordY += 1;
                        Program.world.Data[CoordY, CoordX].OnStepOn(this);
                        return true;
                    }
                }
            }
            else if(e.KeyCode == Keys.Left || e.KeyCode == Keys.NumPad4) {
                if(Program.world.Data[CoordY, CoordX - 1] != null) {
                    foreach(Creature c in Program.world.Creatures) {
                        if(c.CoordY == CoordY && c.CoordX == CoordX - 1 && c.IsCollidable) {
                            // Attack c if c is hostile + do not move
                            return true;
                        }
                    }
                    if(Program.world.Data[CoordY, CoordX - 1].IsCollidable) {
                        Program.world.Data[CoordY, CoordX - 1].OnCollide(this);
                        return true;
                    }
                    else if(Program.world.Data[CoordY, CoordX - 1].IsWalkable) {
                        CoordX -= 1;
                        Program.world.Data[CoordY, CoordX].OnStepOn(this);
                        return true;
                    }
                }
            }
            else if(e.KeyCode == Keys.Right || e.KeyCode == Keys.NumPad6) {
                if(Program.world.Data[CoordY, CoordX + 1] != null) {
                    foreach(Creature c in Program.world.Creatures) {
                        if(c.CoordY == CoordY && c.CoordX == CoordX + 1 && c.IsCollidable) {
                            // Attack c if c is hostile + do not move
                            return true;
                        }
                    }
                    if(Program.world.Data[CoordY, CoordX + 1].IsCollidable) {
                        Program.world.Data[CoordY, CoordX + 1].OnCollide(this);
                        return true;
                    }
                    else if(Program.world.Data[CoordY, CoordX + 1].IsWalkable) {
                        CoordX += 1;
                        Program.world.Data[CoordY, CoordX].OnStepOn(this);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}

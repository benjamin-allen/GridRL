using System.Windows.Forms;
using System.Collections.Generic;

namespace GridRL {
    public class Fireball : Ability {
        #region Constructors

        public Fireball() {
            Name = "fireball";
            Description = "Big ball of gas";
            GridHeight = 2;
            GridWidth = 2;
        }

        #endregion
        #region Overrides

        public override bool Use(Creature user) {
            if(user == Program.player) {
                Program.waitState = 1;
                while(Program.waitState == 1) {
                    Application.DoEvents();
                }
                if(Program.waitState == -1) {
                    return false;
                }
                List<int> points = user.DirectionToPoints(Program.lastDirection);
                FireballEffect e = new FireballEffect(points[0], points[1]);
                Program.world.Effects.Add(e);
                Program.lastDirection = Direction.None;
            }
            return true;
        }

        #endregion
    }
}
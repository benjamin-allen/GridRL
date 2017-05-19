using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GridRL {

    public class Effect : Actor {
        public Effect(Image image, int y, int x) : base(image, y, x) {
            Activate(y, x);
        }

        public int TurnsLeft { get; set; }

        protected virtual void Activate(int y, int x) { }

        protected override void Act() {
            TurnsLeft--;
        }
    }

    class FireballEffect : Effect {
        public FireballEffect(int y, int x) : base(Properties.Resources.Fireball, y, x) { TurnsLeft = 1; }

        protected override void Activate(int y, int x) {
            CoordY = y;
            CoordX = x;
            IsVisible = true;
        }
    }

    public class Ability {
        /* Constructors */
        public Ability() { }

        /* Properties */
        public string Name { get; set; }

        public string Description { get; set; }

        public int GridWidth { get; set; }

        public int GridHeight { get; set; }

        public int GridY { get; set; }

        public int GridX { get; set; }


        /* Methods */
        public virtual bool Use(Creature user) { return false; }    
    }


    public class Fireball : Ability {
        public Fireball() {
            Name = "fireball";
            Description = "Big ball of gas";
            GridHeight = 2;
            GridWidth = 2;
        }

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
    }
}

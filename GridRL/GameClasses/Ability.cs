using System.Drawing;

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

        public override void OnCollide(Actor a) {
            Program.world.CreaturesToRemove.Add((Creature)a);
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
}

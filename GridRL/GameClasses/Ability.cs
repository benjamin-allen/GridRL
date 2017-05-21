using System.Drawing;

namespace GridRL {
    public class Effect : Actor {
        #region Constructors

        public Effect(Image image, int y, int x) : base(image, y, x) {
            Activate(y, x);
        }

        #endregion
        #region Properties

        public int TurnsLeft { get; set; }

        #endregion
        #region Methods
        protected virtual void Activate(int y, int x) { }

        protected override void Act() {
            TurnsLeft--;
        }

        public override void OnCollide(Actor a) {
            Program.console.SetText("The " + a.Name + " was destroyed!");
            Program.world.CreaturesToRemove.Add((Creature)a);
        }

        #endregion
    }

    public class Ability {
        #region Constructors

        public Ability() { }

        #endregion
        #region Properties

        public string Name { get; set; }

        public string Description { get; set; }

        public int GridWidth { get; set; }

        public int GridHeight { get; set; }

        public int GridY { get; set; }

        public int GridX { get; set; }

        #endregion
        #region Methods

        public virtual bool Use(Creature user) { return false; }

        #endregion
    }
}

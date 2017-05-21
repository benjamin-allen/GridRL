﻿namespace GridRL {
    class FlameBurstEffect : Effect {
        #region Constructors

        public FlameBurstEffect(int y, int x) : base(Properties.Resources.Fireball, y, x) { }

        public FlameBurstEffect() : base(Properties.Resources.Fireball) { }
        
        #endregion
        #region Overrides

        public override void Activate(int y, int x) {
            TurnsLeft = 1;
            CoordY = y;
            CoordX = x;
            IsVisible = true;
        }

        public override void OnCollide(Actor a) {
            Program.world.CreaturesToRemove.Add((Creature)a);
        }

        #endregion
    }
}
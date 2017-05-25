namespace GridRL {
    class FlameBurstEffect : Effect {
        #region Constructors

        public FlameBurstEffect(int y, int x) : base(Properties.Resources.Flame, y, x) { }

        public FlameBurstEffect() : base(Properties.Resources.Flame) { }
        
        #endregion
        #region Overrides

        public override void Activate(int y, int x) {
            TurnsLeft = 1;
            CoordY = y;
            CoordX = x;
            Visibility = Vis.Unseen;
        }

        public override void OnCollide(Actor a) {
            Program.world.CreaturesToRemove.Add((Creature)a);
        }

        #endregion
    }
    class FireWallEffect : Effect {
        #region Constructors

        public FireWallEffect(int y, int x) : base(Properties.Resources.Flame, y, x) { }

        public FireWallEffect() : base(Properties.Resources.Flame) { }

        #endregion
        #region Overrides

        public override void Activate(int y, int x, bool horizontal) {
            TurnsLeft = 1;
            CoordY = y;
            CoordX = x;
            if(horizontal) {
                int mag = 1;
                while(CanAccess(Direction.Left, mag)) {
                    FlameBurstEffect f = new FlameBurstEffect(y, x - mag);
                    Program.world.Effects.Add(f);
                    mag++;
                }

                mag = 1;
                while(CanAccess(Direction.Right, mag)) {
                    FlameBurstEffect f = new FlameBurstEffect(y, x + mag);
                    Program.world.Effects.Add(f);
                    mag++;
                }
            }
            else {
                int mag = 1;
                while(CanAccess(Direction.Up, mag)) {
                    FlameBurstEffect f = new FlameBurstEffect(y - mag, x);
                    Program.world.Effects.Add(f);
                    mag++;
                }

                mag = 1;
                while(CanAccess(Direction.Down, mag)) {
                    FlameBurstEffect f = new FlameBurstEffect(y + mag, x);
                    Program.world.Effects.Add(f);
                    mag++;
                }
            }
            Visibility = Vis.Unseen;
        }

        public override void OnCollide(Actor a) {
            Program.world.CreaturesToRemove.Add((Creature)a);
        }
        #endregion
    }
}
namespace GridRL {
    class FireballEffect : Effect {
        #region Constructors

        public FireballEffect(int y, int x) : base(Properties.Resources.Fireball, y, x) {
            TurnsLeft = 1;
        }
        
        #endregion
        #region Overrides

        protected override void Activate(int y, int x) {
            CoordY = y;
            CoordX = x;
            IsVisible = true;
        }

        #endregion
    }
}
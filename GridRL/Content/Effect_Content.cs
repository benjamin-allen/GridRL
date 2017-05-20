namespace GridRL {
    class FireballEffect : Effect {
        public FireballEffect(int y, int x) : base(Properties.Resources.Fireball, y, x) { TurnsLeft = 1; }

        protected override void Activate(int y, int x) {
            CoordY = y;
            CoordX = x;
            IsVisible = true;
        }
    }
}
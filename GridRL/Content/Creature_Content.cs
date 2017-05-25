namespace GridRL {
    public class DummyCreature : Creature {
        #region Constructors

        public DummyCreature(int y, int x) : base(Properties.Resources.Dummy, y, x) {
            Name = "Dummy";
            Description = "A mobile training dummy. ";
            DeathMessage = "The " + Name + "dies!";
            Health = 20;
            Attack = 10;
            Defense = 10;
            Visibility = Vis.Unseen;
        }

        #endregion
    }
}
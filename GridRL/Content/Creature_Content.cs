namespace GridRL {
    public class DummyCreature : Creature {
        /* Constructors */
        public DummyCreature(int y, int x) : base(Properties.Resources.Dummy, y, x) {
            Name = "Dummy";
            Description = "A mobile training dummy. ";
            DeathMessage = "The " + Name + "dies!";
            Health = 20;
            Attack = 0;
            Defense = 0;
            IsVisible = true;
        }
    }
}
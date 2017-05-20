namespace GridRL {
    public class Weapon : Item {
        /* Constructors */
        public Weapon(int y, int x) : base(Properties.Resources.Weap, y, x) {
            Name = "weapon";
            Description = "It's some kind of weapon. You can't make out what it is.";
            MaxStack = 1;
        }

        /* Properties */
        public int Attack { get; set; } = 3;

        public float EffectChance { get; set; } = .75f;

        /* Methods */
        public virtual void OnStrike(Creature owner, Creature struck) { }
    }
}
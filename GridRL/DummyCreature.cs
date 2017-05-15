using System.Drawing;

namespace GridRL {

    public class DummyCreature : Creature {
        /* Constructors */
        public DummyCreature(int y, int x) : base(Properties.Resources.Dummy, y, x) {
            Name = "Dummy";
            Description = "A training dummy. ";
            DeathMessage = "The " + Name + "dies!";
            Health = 20;
            Attack = 0;
            Defense = 0;
            IsVisible = true;
        }

        /* Overrides */
        protected override void OnAttack(Creature attacker) {
            int Damage = attacker.Attack - Defense;
            if(Damage > 0) {
                Health -= Damage;
            }
        }
    }
}

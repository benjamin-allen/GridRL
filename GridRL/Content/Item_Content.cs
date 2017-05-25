namespace GridRL {
    public class Sword : Weapon {
        public Sword(int y, int x) : base(Properties.Resources.Sword, y, x) {
            Name = "sword";
            Description = "A simple blade. It's worn with use but perfectly fine for stabbing.";
            MaxStack = 1;
        }
    }

    public class Shirt : Armor {
        public Shirt(int y, int x) : base(Properties.Resources.Shirt, y, x) {
            Name = "shirt";
            Description = "A cotton T-shirt. You're not sure why you have this.";
            MaxStack = 1;
        }
    }

    public class FlameBurstOrb : Orb {
        public FlameBurstOrb() : base(Properties.Resources.FBOrb) {
            Ability = new FlameBurst();
        }
    }

    public class AttackBoostOrb : PassiveOrb {
        public AttackBoostOrb(Creature owner) : base(Properties.Resources.FBOrb) {
            Ability = new AttackBoost(owner);
        }
    }
}
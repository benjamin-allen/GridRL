
namespace GridRL {

    public partial class Program : Engine {
        private static Weapon sword = new Weapon(Properties.Resources.Sword, 0, 0);
        private static Armor shirt = new Armor(Properties.Resources.Shirt, 0, 0);
        private static Orb fburst = new Orb(Properties.Resources.FBOrb); 

        static void InitializeWeapons() {
            sword.Name = "sword";
            sword.Description = "A simple blade. It's worn but still good for stabbity stabbity.";
            sword.Visibility = Vis.Unseen;
            sword.MaxStack = 1;
            sword.Attack = 3;
            MasterWeapons.Add(sword);
        }

        static void InitializeArmors() {
            shirt.Name = "shirt";
            shirt.Description = "A cotton T-shirt. It's surprisingly airy, but that may be the stab wounds.";
            shirt.MaxStack = 1;
            shirt.Defense = 1;
            MasterArmors.Add(shirt);
        }

        static void InitializeOrbs() {
            
            fburst.Ability = new FlameBurst();
            MasterOrbs.Add(fburst);
        }
    }
}
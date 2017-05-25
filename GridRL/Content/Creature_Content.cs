
namespace GridRL {

    public partial class Program : Engine {
        private static Creature dummy = new Creature(Properties.Resources.Dummy);
        private static Creature mrbones = new Creature(Properties.Resources.Skeleton);

        static void InitializeCreatures() {
            dummy.Name = "Dummy";
            dummy.Description = "A mobile training dummy. Like you, but nakeder.";
            dummy.DeathMessage = "The " + dummy.Name + "dies!";
            dummy.Health = 20;
            dummy.Visibility = Vis.Unseen;
            dummy.AI = AIType.Monster;
            MasterCreatures.Add(dummy);

            mrbones.Name = "Skeledoot";
            mrbones.Description = "I want to get off Mr Bones' Wild Ride.\n:(";
            mrbones.DeathMessage = "2spooky";
            mrbones.Health = 30;
            mrbones.Attack = 20;
            mrbones.Defense = 5;
            dummy.AI = AIType.Monster;
            MasterCreatures.Add(mrbones);
        }
    }
}
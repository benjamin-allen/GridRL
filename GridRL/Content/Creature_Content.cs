
namespace GridRL {

    public partial class Program : Engine {
        private static Creature dummy = new Creature(Properties.Resources.Dummy);

        static void InitializeCreatures() {
            dummy.Name = "Dummy";
            dummy.Description = "A mobile training dummy.";
            dummy.DeathMessage = "The " + dummy.Name + "dies!";
            dummy.Health = 20;
            dummy.Visibility = Vis.Unseen;
            MasterCreatures.Add(dummy);
        }
    }
}
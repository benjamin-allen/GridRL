namespace GridRL {
    public class Sword : Weapon {
        public Sword(int y, int x) : base(Properties.Resources.Sword, y, x) {
            Name = "sword";
            Description = "A simple blade. It's worn with use but perfectly fine for stabbing.";
            MaxStack = 1;
        }
    }
}
using System.Drawing;

namespace GridRL {
    public class Item : Actor {
        /* Constructors */
        public Item(Image image) : base(image) {
            Name = "Dummy Item";
            Description = "If you can see this, file a bug report for an improperly initialized item.";
        }

        public Item(Image image, int y, int x) : base(image, y, x) {
            Name = "Dummy Item";
            Description = "If you can see this, file a bug report for an improperly initialized item.";
        }

        /* Properties */


        /* Methods */
        public virtual void Activate(Actor activator) { }

        public virtual void PickUp(Creature grabber) { }

        /* Overrides */
    }
}

using System.Drawing;

namespace GridRL {
    public class Item : Actor {
        /* Constructors */
        public Item() : base() { }

        public Item(Image image) : base(image) { }

        public Item(Image image, int x, int y) : base(image, x, y) {
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

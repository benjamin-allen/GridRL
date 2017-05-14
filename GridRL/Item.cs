using System.Drawing;

namespace GridRL {
    public class Item : Actor {
        /* Constructors */
        /// <summary> Constructs a new item. </summary>
        /// <param name="image"></param>
        public Item(Image image) : base(image) {
            Name = "Dummy Item";
            Description = "If you can see this, file a bug report for an improperly initialized item.";
        }

        /// <summary> Constructs a new item at the given rendering location. </summary>
        /// <param name="image"> The sprite to use. </param>
        /// <param name="y"> The Y coordinate to render at. </param>
        /// <param name="x"> The X coordinate to render</param>
        public Item(Image image, int y, int x) : base(image, y, x) {
            Name = "Dummy Item";
            Description = "If you can see this, file a bug report for an improperly initialized item.";
        }

        /* Properties */


        /* Methods */
        /// <summary> Called when an actor uses this item. </summary>
        /// <param name="activator"> The actor that activated this item. </param>
        public virtual void Activate(Actor activator) { }

        /// <summary> Called when a creature picks up this item. </summary>
        /// <param name="grabber"> The creature that picked up this item.</param>
        public virtual void PickUp(Creature grabber) { }
    }
}

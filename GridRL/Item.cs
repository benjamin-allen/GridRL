using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace GridRL {
    public class Item : Actor {
        /* Constructors */
        /// <summary> Constructs a new item. </summary>
        /// <param name="image"></param>
        public Item(Image image) : base(image) {
            Name = "Dummy Item";
            Description = "If you can see this, file a bug report for an improperly initialized item.";
            IsCollidable = false;
        }

        /// <summary> Constructs a new item at the given rendering location. </summary>
        /// <param name="image"> The sprite to use. </param>
        /// <param name="y"> The Y coordinate to render at. </param>
        /// <param name="x"> The X coordinate to render</param>
        public Item(Image image, int y, int x) : base(image, y, x) {
            Name = "Dummy Item";
            Description = "If you can see this, file a bug report for an improperly initialized item.";
            IsCollidable = false;
        }

        /* Properties */
        public byte MaxStack { get; set; } = 100;

        /* Methods */
        /// <summary> Called when an actor uses this item. </summary>
        /// <param name="activator"> The actor that activated this item. </param>
        public virtual void Activate(Actor activator) { }

        /// <summary> Called when a creature picks up this item. </summary>
        /// <param name="grabber"> The creature that picked up this item.</param>
        public virtual void PickUp(Creature grabber) {

        }
    }


    public class Inventory {
        /* Constructors */
        public Inventory() {
            Items = new Item[20];
            for(int i = 0; i < 20; ++I) {
                Counts[i] = 0;
            }
        }

        /* Properties */
        public Item[] Items { get; set; }

        public byte[] Counts { get; set; }

        /* Methods */
        public bool AddItem(Item item) {
            int firstEmptySlot = -1;
            bool itemPlaced = false;
            for(int i = 0; i < Items.Length; ++i) {
                if(Items[i] == null && firstEmptySlot == -1) {
                    firstEmptySlot = i;
                }
                else if(Items[i] == item) {
                    if(Counts[i] < Items[i].MaxStack) {
                        Counts[i]++;
                        itemPlaced = true;
                        break;
                    }
                }
            }
            if(!itemPlaced && firstEmptySlot != -1) {
                Items[firstEmptySlot] = item;
                Counts[firstEmptySlot] = 1;
                itemPlaced = true;
            }
            return itemPlaced;
        }

        public bool RemoveItem(Item item) {
            bool itemRemoved = false;
            if(!Items.Contains(item)) {
                return false;
            }
            for(int i = 0; i < Items.Length; ++i) {
                if(Items[i] != null && Items[i] == item) {
                    if(Counts[i] > 1) {
                        Counts[i]--;
                        itemRemoved = true;
                        break;
                    }
                    else {
                        Counts[i] = 0;
                        Items[i] = null;
                        itemRemoved = true;
                        break;
                    }
                }
            }
            return itemRemoved;
        }
    }
}

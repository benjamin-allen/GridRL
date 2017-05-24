using System.Drawing;
using System.Linq;

namespace GridRL {
    /// <summary> Represents an actor that can exist in an inventory. </summary>
    public class Item : Actor {
        #region Constructors

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
        public Item(Image image, int y, int x) : base(image, 0, 0) {
            Name = "Dummy Item";
            Description = "If you can see this, file a bug report for an improperly initialized item.";
            IsCollidable = false;
            Visibility = Vis.Unseen;
        }

        #endregion
        #region Properties

        /// <summary> Represents the maximum allowed copies of an item in an inventory. </summary>
        public byte MaxStack { get; set; } = 255;

        #endregion
        #region Methods

        /// <summary> Called when an actor uses this item. </summary>
        /// <param name="activator"> The creature that activated this item. </param>
        public virtual bool Activate(Creature activator) { return true; }

        /// <summary> Called when a creature picks up this item. </summary>
        /// <param name="grabber"> The creature that picked up this item.</param>
        public virtual void OnPickUp(Creature grabber) { }

        /// <summary> Called when this item is activated. </summary>
        /// <param name="activator"> The creature that activated this. </param>
        public virtual void OnActivate(Creature activator) { }

        #endregion
    }

    /// <summary> Represents a collection of items for creatures and tiles. </summary>
    public class Inventory {
        #region Constructors

        /// <summary> Creates an empty inventory. </summary>
        public Inventory() {
            Items = new Item[22];
            Counts = new byte[22];
        }

        #endregion
        #region Properties

        /// <summary> The discrete items in the inventory. </summary>
        public Item[] Items { get; set; }

        /// <summary> The number of copies of an item in the inventory. </summary>
        /// <remarks> Counts[i] corresponds to the number of copies of Items[i]. </remarks>
        public byte[] Counts { get; set; }

        #endregion
        #region Methods

        /// <summary> Adds an item to this inventory. </summary>
        /// <param name="item"> The item to be added. </param>
        /// <returns> A boolean indicating whether addition was successful. </returns>
        public bool AddItem(Item item) {
            int firstEmptySlot = -1;
            bool itemPlaced = false;
            for(int i = 0; i < Items.Length; ++i) {
                // Store the location of the first empty slot in case there isn't
                // an instance of this item in the inventory. 
                if(Items[i] == null && firstEmptySlot == -1) {
                    firstEmptySlot = i;
                }
                // If there is an instance, increment count and stop looping. 
                else if(Items[i] == item) {
                    if(Counts[i] < Items[i].MaxStack) {
                        Counts[i]++;
                        itemPlaced = true;
                        break;
                    }
                }
            }
            // If no copy was found but there's an empty space, add it to that space. 
            if(!itemPlaced && firstEmptySlot != -1) {
                Items[firstEmptySlot] = item;
                Counts[firstEmptySlot] = 1;
                itemPlaced = true;
            }
            // If none of these work then we couldn't add the item. 
            return itemPlaced;
        }

        /// <summary> Removes an item from the inventory. </summary>
        /// <param name="item"> The item to be removed. </param>
        /// <returns> A boolean indicating whether removal was successful. </returns>
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

        #endregion
    }

    /// <summary> An item that can be wielded and used to strike enemies. </summary>
    public class Weapon : Item {
        #region Constructors

        /// <summary> Creates a new weapon with the given parameters. </summary>
        /// <param name="image"> The image to be used for this weapon. </param>
        /// <param name="y"> The y coordinate of this weapon. </param>
        /// <param name="x"> The x coordinate of this weapon. </param>
        public Weapon(Image image, int y, int x) : base(image, y, x) {
            Name = "Dummy Weapon";
            Description = "If you can see this, file a report for an improperly initialized weapon.";
            MaxStack = 1;
        }

        #endregion
        #region Properties

        /// <summary> The attack bonus from this weapon. </summary>
        public int Attack { get; set; } = 0;

        /// <summary> The chance this weapon has of performing any special effects. </summary>
        public float EffectChance { get; set; } = 0;

        #endregion
        #region Methods

        /// <summary> Called when the wielded weapon is used to strike another creature. </summary>
        /// <param name="owner"> The creature that wields this weapon. </param>
        /// <param name="struck"> The creature being struck. </param>
        public virtual void OnStrike(Creature owner, Creature struck) { }

        #endregion
    }

    /// <summary> An item that can be worn for defense. </summary>
    public class Armor : Item {
        #region Constructors

        /// <summary> Creates a new armor with the given parameters. </summary>
        /// <param name="image"> The image to be used for this armor. </param>
        /// <param name="y"> The y coordinate of this armor. </param>
        /// <param name="x"> The x coordinate of this armor. </param>
        public Armor(Image image, int y, int x) : base(image, y, x) {
            Name = "Dummy Armor";
            Description = "If you can see this, file a report for an improperly initialized armor.";
            MaxStack = 1;
        }

        #endregion
        #region Properties

        /// <summary> The defense bonus from this armor. </summary>
        public int Defense { get; set; } = 0;

        /// <summary> The chance this armor has of performing any special effects. </summary>
        public float EffectChance { get; set; } = 0;

        #endregion
        #region Methods

        /// <summary> Called when the creature wearing this armor is struck. </summary>
        /// <param name="striker"> The creature striking this one. </param>
        /// <param name="owner"> The creature which owns this armor. </param>
        public virtual void OnStrike(Creature striker, Creature owner) { }

        #endregion
    }

    public class Orb : Item {
        #region Constructors

        public Orb(Image image) : base(image) {
            Name = "Mysterious Orb";
            Description = "An orb of unknown origin. It confers new skills when used.";
            MaxStack = 1;
        }

        #endregion
        #region Properties

        public Ability Ability { get; set; }

        #endregion
        #region Methods

        public override bool Activate(Creature activator) {
            return activator.AddNewAbility(Ability);
        }

        #endregion
    }
}

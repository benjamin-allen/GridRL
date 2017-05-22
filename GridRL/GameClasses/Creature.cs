using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System;

namespace GridRL {
    /// <summary> Determines how a creature acts. </summary>
    public enum AIType { None, Monster, NPC }

    /// <summary>A base class for all living things.</summary>
    public partial class Creature : Actor {
        #region Constructors

        /// <summary> A constructor that creates a creature with a given image and x/y coordinates. </summary>
        /// <param name="image"> The image used for this creature's sprite. </param>
        /// <param name="y"> The creature's Y position on the world data. </param>
        /// <param name="x"> The creature's X position on the world data. </param>
        public Creature(Image image, int y, int x) : base(image, y, x) {
            Name = "Dummy Creature";
            Description = "If you can see this, file a bug report for an improperly initialized creature.";
            DeathMessage = "The " + Name + " dies!";
            IsCollidable = true;
        }

        #endregion
        #region Properties

        /// <summary> Creature's HP stat. </summary>
        public int Health { get; set; } = 0;

        /// <summary> Creature's Attack stat. </summary>
        public int Attack { get; set; } = 0;

        /// <summary> Creature's Defense stat. </summary>
        public int Defense { get; set; } = 0;

        /// <summary> A message to be printed when the creature dies. </summary>
        public string DeathMessage { get; set; }

        /// <summary> The items held by this creature. </summary>
        public Inventory Inventory { get; set; } = new Inventory();

        /// <summary> The kind of behavior followed by the creature. </summary>
        public AIType AI { get; set; } = AIType.Monster;

        /// <summary> The abilities currently known by this monster. </summary>
        public List<Ability> Abilities { get; set; } = new List<Ability>();

        public Weapon HeldWeapon { get; set; }

        public Armor WornArmor { get; set; }

        public Item HeldItem { get; set; }

        #endregion
        #region Methods

        /// <summary> Executes an attack on the given creature. </summary>
        /// <param name="attacked"> The creature to be attacked. </param>
        protected virtual void PerformAttack(Creature attacked) {
            attacked.OnAttack(this);
        }

        /// <summary> Attempt to add an item to the invetory of this creature. </summary>
        /// <param name="i"> The item being obtained. </param>
        /// <returns> A boolean indicating whether the item was successfully obtained. </returns>
        public bool PickUp(Item i) {
            if(Program.world.Data[CoordY, CoordX].Inventory.RemoveItem(i)) {
                Inventory.AddItem(i);
                return true;
            }
            return false;
        }

        /// <summary> Attempt to remove the item held by this creature. </summary>
        /// <param name="i"> The item being dropped. </param>
        /// <returns> A boolean indicating whether the item was successfully dropped. </returns>
        public bool Drop(Item i) {
            if(Program.world.Data[CoordY, CoordX].Inventory.AddItem(i)) {
                Inventory.RemoveItem(i);
                return true;
            }
            return false;
        }

        /// <summary> Called when a creature is attacked. </summary>
        /// <param name="attacker"> The creature attacking this. </param>
        protected virtual void OnAttack(Creature attacker) {
            int Damage = attacker.Attack - Defense;
            if(Damage > 0) {
                Health -= Damage;
            }
            if(Health <= 0) {
                Remove(this);
                Program.world.Creatures.Remove(this);
                Visibility = Vis.Unseen;
                IsCollidable = false;
            }
        }

        public virtual void AddNewAbility(Ability a) { }

        #endregion
        #region Overrides

        /// <summary> Makes decisions based on AI. </summary>
        protected override void Act() {
            base.Act();
            if(AI == AIType.Monster) {
                RandomWalk();
            }
        }

        #endregion
    }
}

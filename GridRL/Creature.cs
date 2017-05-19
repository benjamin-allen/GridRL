using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System;

namespace GridRL {
    /// <summary> Possible implementation: enumeration of creature's AI type. </summary>
    public enum AIType { None, Monster, NPC }

    /// <summary>A base class for all living things.</summary>
    public partial class Creature : Actor {
        /* Constructors */
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


        /* Properties */

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

        public AIType AI { get; set; } = AIType.Monster;

        public List<Ability> Abilities { get; set; } = new List<Ability>();

        /* Methods */
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

        protected virtual void OnAttack(Creature attacker) {
            int Damage = attacker.Attack - Defense;
            if(Damage > 0) {
                Health -= Damage;
            }
            Console.WriteLine(Name + " was hit!");
            if(Health <= 0) {
                Remove(this);
                IsVisible = false;
                IsCollidable = false;
            }
        }


        /* Overrides */
        protected override void Act() {
            base.Act();
            if(AI == AIType.Monster) {
                RandomWalk();
            }
        }

        //Possible override base.Remove() for onDeath message of some kind.
    }
}

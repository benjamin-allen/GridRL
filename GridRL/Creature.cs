using System.Drawing;

namespace GridRL {
    /// <summary> Possible implementation: enumeration of creature's AI type. </summary>
    //public enum AIType { }

    /// <summary>A base class for all living things.</summary>
    public class Creature : Actor {
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

        public string DeathMessage { get; set; }

        /* Methods */
        protected virtual void PerformAttack(Creature attacked) {
            attacked.OnAttack(this);
        }

        protected virtual void OnAttack(Creature attacker) {
            int Damage = attacker.Attack - Defense;
            if(Damage > 0) {
                Health -= Damage;
            }
        }
        //Possible override base.Remove() for onDeath message of some kind.
    }
}

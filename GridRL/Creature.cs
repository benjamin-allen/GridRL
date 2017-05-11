using System;
using System.Drawing;

namespace GridRL {
    /// <summary> Possible implementation: enumeration of creature's AI type. </summary>
    //public enum AIType { }

    /// <summary>A base class for all living things.</summary>
    public class Creature : Actor {
        /* Constructors */
        /// <summary> A constructor that creates a creature with a given image and x/y coordinates. </summary>
        /// <param name="image"> The image used for this creature's sprite. </param>
        /// <param name="x"> The creature's X position on the world data. </param>
        /// <param name="y"> The creature's Y position on the world data. </param>
        public Creature(Image image, int x, int y) : base(image, x, y) {
            Name = "Dummy Creature";
            Description = "If you can see this, file a bug report for an improperly initialized creature.";
            DeathMessage = "The " + Name + " dies!";
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

        //Possible override base.Remove() for onDeath message of some kind.
    }
}

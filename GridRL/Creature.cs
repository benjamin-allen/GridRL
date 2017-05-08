using System;
using System.Drawing;

namespace GridRL {

    /// <summary>A base class for all living things.</summary>
    public class Creature : ImageSprite {
        /* Constructors */
        public Creature() { }

        /// <summary> Constructs a creature with a given image. </summary>
        /// <param name="image"> The image used for this creature's sprite. </param>
        public Creature(Image image) : base(image) { }

        /// <summary> A constructor that creates a creature with a given image and x/y coordinates. </summary>
        /// <param name="image"> The image used for this creature sprite</param>
        /// <param name="x"> The creature's X position on the world data. </param>
        /// <param name="y"> The creature's Y position on the world data.</param>
        public Creature(Image image, float x, float y) : base(image, x, y) { }


        /* Properties */

        /// <summary> The name of the creature. </summary>
        public string Name { get; set; }

        /// <summary> Description of the creature. </summary>
        public string Description { get; set; }

        /* Methods */
        
        //Possible override base.Remove() for onDeath message of some kind.
    }
}

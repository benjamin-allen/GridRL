using System;
using System.Drawing;


namespace GridRL {
    /// <summary> A base class for anything that exists on the world. </summary>
    public class Actor : ImageSprite{

        /* Constructors */
        public Actor() { }

        public Actor(Image image) : base(image) { }

        /// <summary> A constructor that creates an actor with a given image and x/y coordinates. </summary>
        /// <param name="image"> The image used for this actor sprite</param>
        /// <param name="x"> The actor's X position on the world data. </param>
        /// <param name="y"> The actor's Y position on the world data.</param>
        public Actor(Image image, float x, float y) : base(image, x, y) { }


        /* Properties */

        /// <summary> The name of the actor. </summary>
        public string Name { get; set; } = "Actor";

        /// <summary> Description of the actor. </summary>
        public string Description { get; set; } = "Generic Actor";

        /// <summary> A boolean that determines if the actor can collide with other actors. </summary>
        public bool isCollidable { get; set; } = false;

        /// <summary> A boolean that determines if an actor is visible. </summary>
        public bool isVisible { get; set; } = true;

        /* Methods */


        /* Overrides */
    }
}

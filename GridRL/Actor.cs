using System;
using System.Drawing;


namespace GridRL {
    /// <summary> A base class for anything that exists on the world. </summary>
    public class Actor : ImageSprite {

        /* Constructors */
        public Actor() { }

        public Actor(Image image) : base(image) { }

        /// <summary> A constructor that creates an actor with a given image and x/y coordinates. </summary>
        /// <param name="image"> The image used for this actor sprite</param>
        /// <param name="y"> The actor's Y position on the world data.</param>
        /// <param name="x"> The actor's X position on the world data. </param>
        public Actor(Image image, int y, int x) : base(image) {
            CoordX = x;
            CoordY = y;
        }


        /* Properties */
        /// <summary> The name of the actor. </summary>
        public string Name { get; set; } = "Dummy Actor";

        /// <summary> Description of the actor. </summary>
        public string Description { get; set; } = "If you can see this, file a bug report for an improperly initialized actor.";

        /// <summary> A boolean that determines if the actor can collide with other actors. </summary>
        public bool IsCollidable { get; set; } = false;

        /// <summary> A boolean that determines if an actor is visible. </summary>
        public bool IsVisible { get; set; } = false;

        private int coordX;
        private int coordY;

        /// <summary>The actor's horizontal position in the world grid. </summary>
        public int CoordX {
            get { return coordX; }
            set { coordX = value; X = coordX * Engine.spriteWidth; }
        }

        /// <summary>The actor's vertical position in the world grid. </summary>
        public int CoordY {
            get { return coordY; }
            set { coordY = value; Y = coordY * Engine.spriteHeight; }
        }


        /* Methods */
        public virtual void OnCollide(Actor a) { }

        /* Overrides */
        protected override void Paint(Graphics g) {
            if(IsVisible) {
                base.Paint(g); 
            }
        }
    }
}

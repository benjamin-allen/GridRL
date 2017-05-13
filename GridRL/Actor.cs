using System;
using System.Drawing;
using System.Collections.Generic;


namespace GridRL {

    public enum Direction { Up, Down, Left, Right }

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

        public bool CanAccess(Direction d, int magnitude = 1) {
            bool output = false;
            magnitude = Math.Abs(magnitude);
            if(d == Direction.Up) {
                output = Program.world.Data[CoordY - (1 * magnitude), CoordX] == null ? false : true;
            }
            else if(d == Direction.Down) {
                output = Program.world.Data[CoordY + (1 * magnitude), CoordX] == null ? false : true;
            }
            else if(d == Direction.Left) {
                output = Program.world.Data[CoordY, CoordX - (1 * magnitude)] == null ? false : true;
            }
            else if(d == Direction.Right) {
                output = Program.world.Data[CoordY, CoordX + (1 * magnitude)] == null ? false : true;
            }
            return output;
        }

        public bool WillCollideWith(Actor a) {
            bool output = false;
            if(a.IsCollidable && IsCollidable) {
                output = true;
            }
            return output; 
        }

        public List<int> DirectionToPoints(Direction d, int magnitude = 1) {
            List<int> output = new List<int>(new int[] { CoordY, CoordX });
            magnitude = Math.Abs(magnitude);
            if(d == Direction.Up) {
                output[0] = output[0] - (1 * magnitude);
            }
            else if(d == Direction.Down) {
                output[0] = output[0] + (1 * magnitude);
                ;
            }
            else if(d == Direction.Left) {
                output[1] = output[1] - (1 * magnitude);
            }
            else if(d == Direction.Right) {
                output[1] = output[1] + (1 * magnitude);
            }
            return output;
        }

        /* Overrides */
        protected override void Paint(Graphics g) {
            if(IsVisible) {
                base.Paint(g); 
            }
        }
    }
}

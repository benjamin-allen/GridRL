using System;
using System.Drawing;
using System.Collections.Generic;


namespace GridRL {
    /// <summary> </summary>
    public enum Direction {None, Up, Down, Left, Right }

    /// <summary> A base class for anything that exists on the world. </summary>
    public class Actor : ImageSprite {
        #region Constructors

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

        #endregion
        #region Properties

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

        #endregion
        #region Methods
        /// <summary> Virtual function to be called when two actors collide. </summary>
        /// <param name="a"> The actor colliding with this actor. </param>
        public virtual void OnCollide(Actor a) { }

        /// <summary> Checks if this actor can access a position on the world's data array. </summary>
        /// <param name="d"> The direction to check. </param>
        /// <param name="magnitude"> The distance from this actor to check. </param>
        /// <returns> A bool representing whether this actor can access the data at the specified direction and magnitude. </returns>
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

        /// <summary> Checks if this actor will collide with the specified actor. Does not perform location checks. </summary>
        /// <param name="a"> The actor to check for collision against. </param>
        /// <returns> A bool representing whether these actors can collide with each other. </returns>
        public bool WillCollideWith(Actor a) {
            bool output = false;
            if(a.IsCollidable && IsCollidable) {
                output = true;
            }
            return output; 
        }

        /// <summary> Calculates coordinates. </summary>
        /// <param name="d"> The direction of movement. </param>
        /// <param name="magnitude"> The distance to calculate to. </param>
        /// <returns> The Y and X coordinates of the pathed point. </returns>
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

        #endregion
        #region Overrides

        /// <summary> Draws the actor if it is visible. </summary>
        /// <param name="g"> Graphics doohicky. </param>
        protected override void Paint(Graphics g) {
            if(IsVisible) {
                base.Paint(g); 
            }
        }

        #endregion
    }
}

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GridRL {
    public class Ability {
        #region Constructors

        /// <summary> An ability, which is shown on the grid and creates an effect. </summary>
        public Ability() { }

        #endregion
        #region Properties

        /// <summary> The name of this ability. </summary>
        public string Name { get; set; }

        /// <summary> This ability's description. </summary>
        public string Description { get; set; }

        /// <summary> This ability's width on the grid. </summary>
        public int GridWidth { get; set; }

        /// <summary> This ability's height on the grid. </summary>
        public int GridHeight { get; set; }

        /// <summary> This ability's location on the grid. </summary>
        public int GridY { get; set; }

        /// <summary> This ability's location on the grid. </summary>
        public int GridX { get; set; }

        /// <summary> The effect acvitated when this ability is used. </summary>
        public Effect Effect { get; set; }

        #endregion
        #region Methods

        /// <summary> Called when a creature activates this ability. Remove base when overriding. </summary>
        /// <param name="user"> The creature using this ability. </param>
        /// <returns> True: Ability used | False: Use abandoned (Prompt for direction abandoned) </returns>
        public virtual bool Use(Creature user) {
            return false;
        }

        #endregion
    }

    public class PassiveAbility : Ability {
        #region Constructors

        public PassiveAbility(Creature owner) : base() {
            Owner = owner;
        }

        #endregion
        #region Properties

        public Creature Owner { get; set; }

        #endregion
        #region Method

        public virtual void OnAddToGrid() { }

        public virtual void OnRemoveFromGrid() { }

        #endregion
    }

    public class DirectedAbility : Ability {
        #region Constructors

        public DirectedAbility() : base() { }

        #endregion
        #region Methods 

        public virtual void CreateEffect(Creature user, Direction dir) { }

        private bool promptDirection() {
            Program.waitState = 1;
            while(Program.waitState == 1) {
                Application.DoEvents();
            }
            if(Program.waitState == -1) {
                Program.lastDirection = Direction.None;
                return false;
            }
            return true;
        }

        #endregion
        #region Overrides

        public override bool Use(Creature user) {
            // if user is player, prompt for input
            if(user == Program.player) {
                if(promptDirection()) {
                    CreateEffect(user, Program.lastDirection);
                    Program.lastDirection = Direction.None;
                }
                else {
                    return false;
                }
            }
            else {
                Direction dir = Direction.None;
                if(Program.player.CoordX == user.CoordX) {
                    if(Program.player.CoordY < user.CoordY) {
                        dir = Direction.Up;
                    }
                    else {
                        dir = Direction.Down;
                    }
                }
                else if(Program.player.CoordY == user.CoordY) {
                    if(Program.player.CoordX < user.CoordX) {
                        dir = Direction.Left;
                    }
                    else {
                        dir = Direction.Right;
                    }
                }
                CreateEffect(user, dir);
            } 
            return true;
        }

        #endregion
    }

    public class TargetedAbility : Ability {
        #region Constructors

        public TargetedAbility() : base() { }

        #endregion
        #region Overrides

        public override bool Use(Creature user) {
            // if user is player, allow for a targeter to select the square
            // if user is not player, select the square of the player
            return false;
        }

        #endregion
    }

    public class Effect : Actor {
        #region Constructors

        /// <summary> Constructs a new effect. </summary>
        /// <param name="image"> The sprite to use for this effect. </param>
        /// <param name="y"> The y location of this effect. </param>
        /// <param name="x"> The x location of this effect. </param>
        public Effect(Image image, int y, int x) : base(image, y, x) {
            Activate(y, x);
        }

        public Effect(Image image) : base(image) { }

        #endregion
        #region Properties

        /// <summary> The number of turns remaining until this effect disappears. </summary>
        public int TurnsLeft { get; set; }

        #endregion
        #region Methods

        /// <summary> Called upon construction. </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        public virtual void Activate(int y, int x) { }

        /// <summary> Base implementation decrements the lifetime of the effect. </summary>
        protected override void Act() {
            TurnsLeft--;
        }

        #endregion
    }
}

using System;
using System.Drawing;

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

        #endregion
        #region Methods

        /// <summary> Called when a creature activates this ability. Remove base when overriding. </summary>
        /// <param name="user"> The creature using this ability. </param>
        /// <returns> True: Ability used | False: Use abandoned (Prompt for direction abandoned) </returns>
        public virtual bool Use(Creature user) { return false; }

        #endregion
    }

    public class PassiveAbility : Ability {
        #region Constructors

        public PassiveAbility() : base() { }

        #endregion
        #region Properties

        public PassiveEffect Effect { get; set; }

        #endregion
        #region Overrides

        public override bool Use(Creature user) {
            // create new passive and apply it to the user
            return true;
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

        #endregion
        #region Properties

        /// <summary> The number of turns remaining until this effect disappears. </summary>
        public int TurnsLeft { get; set; }

        #endregion
        #region Methods

        /// <summary> Called upon construction. </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        protected virtual void Activate(int y, int x) { }

        /// <summary> Base implementation decrements the lifetime of the effect. </summary>
        protected override void Act() {
            TurnsLeft--;
        }

        #endregion
    }

    public class PassiveEffect : Effect {
        public PassiveEffect() : base(null, -1, -1) { }
    }
}

using System.Windows.Forms;
using System.Collections.Generic;

namespace GridRL {
    public class FlameBurst : DirectedAbility {
        #region Constructors

        public FlameBurst() {
            Name = "Flame Burst";
            Description = "Blasts a small jet of flame from your hand.";
            GridHeight = 1;
            GridWidth = 2;
            Effect = new FlameBurstEffect();
        }

        #endregion
        #region Overrides

        public override void CreateEffect(Creature user, Direction dir) {
            List<int> points = user.DirectionToPoints(dir);
            Effect.CoordY = points[0];
            Effect.CoordX = points[1];
            Effect.Activate(Effect.CoordY, Effect.CoordX);
            Program.world.Effects.Add(Effect);
        }

        #endregion
    }

    public class AttackBoost : PassiveAbility {
        #region Constructors

        public AttackBoost(Creature owner) : base(owner) { }

        #endregion
        #region Overrides

        public override void OnAddToGrid() {
            Owner.Attack += 2;
        }

        public override void OnRemoveFromGrid() {
            Owner.Attack -= 2;
        }

        #endregion
    }
}
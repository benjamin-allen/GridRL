using System.Drawing;
using System;
using System.Linq;
using System.Collections.Generic;

namespace GridRL {

    public class DummyCreature : Creature {
        /* Constructors */
        public DummyCreature(int y, int x) : base(Properties.Resources.Dummy, y, x) {
            Name = "Dummy";
            Description = "A training dummy. ";
            DeathMessage = "The " + Name + "dies!";
            Health = 20;
            Attack = 0;
            Defense = 0;
            IsVisible = true;
        }

        /* Overrides */
        protected override void Act() {
            base.Act();
            var dirs = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
            Program.Shuffle(dirs);
            foreach(Direction dir in dirs) {
                if(CanAccess(dir)) {
                    List<int> points = DirectionToPoints(dir);
                    if(Program.player.CoordY == points[0] && Program.player.CoordX == points[1] && WillCollideWith(Program.player)) {
                        foreach(Creature c in Program.world.Creatures) {
                            if(c != this && WillCollideWith(c) && c.CoordY == points[0] && c.CoordX == points[1]) {
                                //c.OnCollide(this);
                                PerformAttack(c);
                            }
                        }
                    }
                    else {
                        // Check for collision with world structures
                        if(WillCollideWith(Program.world.Data[points[0], points[1]])) {
                            Program.world.Data[points[0], points[1]].OnCollide(this);
                        }
                        // Check to see if the world structure can be walked on
                        else if(Program.world.Data[points[0], points[1]].IsWalkable) {
                            CoordY = points[0];
                            CoordX = points[1];
                            Program.world.Data[points[0], points[1]].OnStepOn(this);
                        }
                        break;
                    }
                }
            }
        }
    }
}

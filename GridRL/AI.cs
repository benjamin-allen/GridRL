using System;
using System.Collections.Generic;
using System.Linq;

namespace GridRL {
    public partial class Creature {
        /* Methods */
        private void RandomWalk() {
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace GridRL {
    public partial class Creature {
        public static bool canSee = true;
        #region Methods

        /// <summary> Performs a random walk. </summary>
        private void RandomWalk() {
            var dirs = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
            Program.Shuffle(dirs);
            foreach(Direction dir in dirs) {
                if(CanAccess(dir)) {
                    List<int> points = DirectionToPoints(dir);
                    if(Program.player.CoordY == points[0] && Program.player.CoordX == points[1] && WillCollideWith(Program.player)) {
                        foreach(Creature c in Program.world.Creatures) {
                            if(c != this && WillCollideWith(c) && c.CoordY == points[0] && c.CoordX == points[1]) {
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

        private void FollowPlayer() {
            canSee = true;
            int y = (int)Y / 16;
            int x = (int)X / 16;
            int px = (int)Program.player.X / 16;
            int py = (int)Program.player.Y / 16;
            Point creature = new Point(x, y);
            Point player = new Point(px, py);
            Line line = new Line(creature, player);
            double distance = GetDistance(creature, player);
            List<Point> points = new List<Point>((int)Math.Floor(distance));
            int loops = 5;
            if(distance < 5 && distance > 0) {
                points = line.getPoints((int)Math.Ceiling(distance));
                loops = (int)Math.Ceiling(distance);

                for(int i = 0 ; i < loops ; i++) {
                    if(Program.world.Data[points[i].Y, points[i].X] != null && Program.world.Data[points[i].Y, points[i].X].BlocksLight) {
                        canSee = false;
                    }
                }
            }
            if(distance < 5 && canSee) {
                if(points.Capacity > 1) {
                    if(points[1].X == px && points[1].Y == py && distance < 2) {
                        PerformAttack(Program.player);
                    }
                    else if(points[1].X != px && points[1].Y != py) {
                        if(Program.world.Data[points[1].Y, points[1].X] != null && canSee) {
                            CoordX = points[1].X;
                            CoordY = points[1].Y;
                        }
                    }
                }
                if(distance < 2) {
                    if(points[0].X == px && points[0].Y == py && distance < 2) {
                        PerformAttack(Program.player);
                    }
                }
            }
            else {
                RandomWalk();
            }
        }
        private static double GetDistance(Point point1, Point point2) {
            double a = (double)(point2.X - point1.X);
            double b = (double)(point2.Y - point1.Y);

            return Math.Sqrt(a * a + b * b);
        }
    }
    class Line {
        public Point p1, p2;

        public Line(Point p1, Point p2) {
            this.p1 = p1;
            this.p2 = p2;
        }

        public List<Point> getPoints(int quantity) {
            List<Point> points = new List<Point>(quantity);
            int ydiff = p2.Y - p1.Y, xdiff = p2.X - p1.X;
            double slope = (double)(p2.Y - p1.Y) / (p2.X - p1.X);
            double x, y;

            --quantity;

            for(double i = 0 ; i < quantity ; i++) {
                y = slope == 0 ? 0 : ydiff * (i / quantity);
                x = slope == 0 ? xdiff * (i / quantity) : y / slope;
                Point p = new Point(((int)Math.Round(x) + p1.X), ((int)Math.Round(y) + p1.Y));
                points.Insert((int)i, p);
            }
            int index = quantity;
            points.Insert(index, p2);
            return points;
        }
    }

    #endregion
}
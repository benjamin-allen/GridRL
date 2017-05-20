using System.Windows.Forms;

namespace GridRL {
    public class Corridor : Tile {
        /* Constructors */
        public Corridor(int y, int x, int region) : base(Properties.Resources.Corridor, y, x) {
            Name = "corridor";
            Description = "A darkened hallway that connects the many rooms of the dungeon.";
            IsWalkable = true;
            Region = region;
        }
    }

    public class RoomFloor : Tile {
        /* Constructors */
        public RoomFloor(int y, int x, int region) : base(Properties.Resources.Floor, y, x) {
            Name = "floor";
            Description = "A tiled floor, cracked and worn from years of neglect.";
            IsWalkable = true;
            Region = region;
        }
    }

    public enum DoorState { Closed, Broken, Open }
    public class Door : Tile {
        /* Constructors */
        public Door(int y, int x, int region) : base(Properties.Resources.Door, y, x) {
            Name = "door";
            Description = "An old wooden door placed here long ago. You might be able to open it.";
            Region = region;
            IsCollidable = true;

        }

        /* Properties */
        public DoorState DoorState { get; set; } = DoorState.Closed;

        /* Overrides */
        public override void OnCollide(Actor a) {
            if(DoorState == DoorState.Closed) {
                DoorState = DoorState.Open;
                IsCollidable = false;
                IsWalkable = true;
            }
        }
    }

    public enum StairType { Up, Down }
    public class Stair : Tile {
        /* Constructors */
        public Stair(int y, int x, StairType s) : base(Properties.Resources.Stair, y, x) {
            Name = "stairway";
            StairType = s;
            if(s == StairType.Up) {
                Description = "A set of stairs leading up.";
            }
            else {
                Description = "A set of stairs leading down.";
            }
            IsWalkable = true;
        }

        /* Properties */
        public StairType StairType { get; set; }

        /* Overrides */
        public override void OnStepOn(Sprite s) {
            if(s.GetType() == typeof(Player)) {
                if(StairType == StairType.Up && Program.world.Level > 1) {
                    Program.world.Level--;
                    Program.world.GenerateLevel();
                }
                else if(StairType == StairType.Down) {
                    Program.world.Level++;
                    Program.world.GenerateLevel();
                }
                else {
                    Application.Exit();
                }
            }
        }
    }
}
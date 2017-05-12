using System.Windows.Forms;
using System.Drawing;

namespace GridRL {

    /// <summary> The basic definition of tiles, which make up a world. </summary>
    public class Tile : Actor {
        /* Constructors */
        /// <summary> Constructs a tile with the given image and position. </summary>
        /// <param name="image">The image used for this tile's sprite. </param>
        /// <param name="y"> The vertical position of the tile in the world data. </param>
        /// <param name="x"> The horizontal position of the tile in the world data. </param>
        public Tile(Image image, int y, int x) : base(image, y, x) {
            Name = "Dummy Tile";
            Description = "If you can see this, file a bug report for an improperly initialized creature.";
        }


        /* Properties */
        /// <summary> Property representing whether creatures and items can be on the tile. </summary>
        public bool IsWalkable { get; set; } = false;

        /// <summary> Identifies tiles connected to each other. Used during mapgen. </summary>
        public int Region { get; set; } = -1;


        /* Methods */
        /// <summary> Called when a tile is stepped on. </summary>
        /// <param name="s">The sprite that stepped on this tile. </param>
        public virtual void OnStepOn(Sprite s) { }
    }

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

    public enum DoorState { Closed, Broken, Open}
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

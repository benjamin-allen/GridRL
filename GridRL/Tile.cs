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

    public class Door : Tile {
        /* Constructors */
        public Door(int y, int x, int region) : base(Properties.Resources.Door, y, x) {
            Name = "door";
            Description = "An old wooden door placed here long ago. You might be able to open it.";
            IsWalkable = false;
            Region = region;
        }
    }
}

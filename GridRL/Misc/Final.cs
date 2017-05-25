using System.Collections.Generic;

namespace GridRL {
    public class Macguffin : Item {
        public Macguffin() : base(Properties.Resources.Macguffin) {
            Name = "Giant Doubloon";
            Description = "It's a big coin. Kinda neato. Also those monsters are after you for it.";
            MaxStack = 1;
        }

        public override void OnPickUp(Creature grabber) {
            if(grabber == Program.player) {
                for(int i = 0; i < 20; ++i) {
                    int y = Engine.rand.Next(Program.world.RoomPoints[0][0], Program.world.RoomPoints[0][2]);
                    int x = Engine.rand.Next(Program.world.RoomPoints[0][1], Program.world.RoomPoints[0][3]);
                    Creature c = new GridRL.Creature(Engine.MasterCreatures[1]);
                    c.CoordY = y;
                    c.CoordX = x;
                    Program.world.Creatures.Add(c);
                }
            }
            Program.console.SetText("Welcome to Mr. Bones' Wild Ride!");
            Engine.HasMacguffin = true;
        }
    }

    public partial class World {
        private void GenerateFinal() {
            // big room
            int roomY = Data.GetLength(0) / 4;
            int roomY2 = 3 * Data.GetLength(0) / 4;
            int roomX = Data.GetLength(1) / 4;
            int roomX2 = 3 * Data.GetLength(1) / 4;
            RoomPoints.Add(new List<int>(new int[] { roomY, roomX, roomY2, roomX2 }));
            for(int y = roomY; y < roomY2; ++y) {
                for(int x = roomX; x < roomX2; ++x) {
                    Data[y, x] = new RoomFloor(y, x, 0);
                }
            }
            for(int y = roomY-1; y < roomY2 + 1; ++y) {
                for(int x = roomX - 1; x < roomX2 + 1; ++x) {
                    if(Data[y, x] == null) {
                        Data[y, x] = new Wall(y, x);
                    }
                }
            }
            int entryY = roomY2;
            int entryX = (roomX + roomX2) / 2;
            Data[entryY, entryX] = new Stair(entryY, entryX, StairType.Up);
            Data[roomY, (roomX + roomX2) / 2].Inventory.AddItem(new Macguffin());
            Program.player.CoordX = entryX;
            Program.player.CoordY = entryY;
        }
    }
}

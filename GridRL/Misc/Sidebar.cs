using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridRL {
    public class Sidebar : Sprite {

        public int[] stats { get; } = new int[3];
        public int InvCellSize { get; } = 16;
        public int CellsY { get; } = 25;
        public int CellsY2 { get; } = 29;
        public int CellsX { get; } = 67;
        public int GridX { get; } = 68;
        public int GridY { get; } = 33;
        public int GridCellSize { get; } = 48;
        Font ConsoleText = new Font("Courier New", 9);
        public Inventory copyTileInv;

        public Sidebar() : base() {
        }

        

        protected override void Paint(Graphics g) {
            Pen red = new Pen(Color.FromArgb(208, 70, 72), 3f);
            //Drawing stats box + desc box
            stats[0] = Program.player.Health;
            stats[1] = Program.player.Attack;
            stats[2] = Program.player.Defense;
            Rectangle statsBox = new Rectangle(66 * 16, 16 * 2, 13 * 16, 8 * 16);
            g.DrawRectangle(Pens.White, statsBox);
            string statOutput =
                Program.player.Name + "\n" +
                "Health: " + stats[0] + "\n" +
                "Attack: " + stats[1] + "\n" +
                "Defense: " + stats[2];
            g.DrawString(statOutput,ConsoleText ,Brushes.White, statsBox);
            Rectangle descBox = new Rectangle(66 * 16, 16 * 10, 13 * 16, 6 * 16);
            g.DrawRectangle(Pens.White, descBox);
            g.DrawString(Program.HoverString, ConsoleText, Brushes.White, descBox);

            Rectangle holdBox =  new Rectangle(74 * 16, 16 * 18, 16, 16);
            Rectangle wearBox =  new Rectangle(74 * 16, 16 * 20, 16, 16);
            Rectangle wieldBox = new Rectangle(74 * 16, 16 * 22, 16, 16);
            g.DrawString("Holding  : ", ConsoleText, Brushes.White, 67 * 16, 16 * 18);
            g.DrawString("Wearing  : ", ConsoleText, Brushes.White, 67 * 16, 16 * 20);
            g.DrawString("Wielding : ", ConsoleText, Brushes.White, 67 * 16, 16 * 22);
            g.DrawRectangle(Pens.White, holdBox);
            g.DrawRectangle(Pens.White, wearBox);
            g.DrawRectangle(Pens.White, wieldBox);
            if(Program.MA == MouseArea.HoldBox) {
                g.DrawRectangle(red, holdBox);
            }
            else if(Program.MA == MouseArea.WearBox) {
                g.DrawRectangle(red, wearBox);
            }
            else if(Program.MA == MouseArea.WieldBox) {
                g.DrawRectangle(red, wieldBox);
            }

            //Drawing player inventory slots
            g.DrawString("Player Inventory", ConsoleText, Brushes.White, 16 * 67, 16 * (CellsY - 1));
            g.DrawString("Tile Inventory", ConsoleText, Brushes.White, 16 * 67, 16 * (CellsY2 - 1));
            int[] selectPoints = new int[2] { -1, -1 };
            for(int i = 0 ; i < 2 ; i++) {
                for(int j = 0 ; j < 11 ; j++) {
                    Rectangle rect = new Rectangle((CellsX * InvCellSize) + j * InvCellSize, (CellsY * InvCellSize) + i * InvCellSize, InvCellSize, InvCellSize);
                    Rectangle rect2 = new Rectangle((CellsX * InvCellSize) + j * InvCellSize, (CellsY2 * InvCellSize) + i * InvCellSize, InvCellSize, InvCellSize);
                    if(j == Program.PlrInvMouseCoords[1] && i == Program.PlrInvMouseCoords[0] && Program.MA == MouseArea.PlayerInv) {
                        selectPoints[0] = i;
                        selectPoints[1] = j;
                    }
                    else if(j == Program.TileInvMouseCoords[1] && i == Program.TileInvMouseCoords[0] && Program.MA == MouseArea.TileInv) {
                        selectPoints[0] = i;
                        selectPoints[1] = j;
                    }
                    g.DrawRectangle(Pens.White, rect);
                    g.DrawRectangle(Pens.White, rect2);
                }
            }
            if(selectPoints[0] != -1 && (Program.MA == MouseArea.PlayerInv || Program.MA == MouseArea.TileInv)) {
                int temp = CellsY;
                if(Program.MA == MouseArea.TileInv) {
                    temp = CellsY2;
                }
                Rectangle rect = new Rectangle((CellsX * InvCellSize) + selectPoints[1] * InvCellSize, (temp * InvCellSize) + selectPoints[0] * InvCellSize, InvCellSize, InvCellSize);
                g.DrawRectangle(red, rect);
            }

            //Drawing ability grid
            for(int i = 0; i < 3; i++) {
                for(int j = 0; j < 3; j++) {
                    if(Program.MA == MouseArea.Grid && j == Program.GridMouseCoords[1] && i == Program.GridMouseCoords[0]) {
                        selectPoints[0] = i;
                        selectPoints[1] = j;
                    }
                    Rectangle rect = new Rectangle(16 * GridX + i * GridCellSize, GridY * 16 + j * GridCellSize, GridCellSize, GridCellSize);
                    g.DrawRectangle(Pens.White, rect);
                }
            }
            if(selectPoints[0] != -1 && Program.MA == MouseArea.Grid) {
                Rectangle rect = new Rectangle((16 * GridX) + selectPoints[1] * GridCellSize, (16 * GridY) + selectPoints[0] * GridCellSize, GridCellSize, GridCellSize);
                g.DrawRectangle(red, rect);
            }

            // Render player inventory
            for(int i = 0; i < 22; ++i) {
                if(Program.player.Inventory.Items[i] != null) {
                    Program.player.Inventory.Items[i].CoordY = (i / 11) + CellsY;
                    Program.player.Inventory.Items[i].CoordX = (i % 11) + CellsX;
                    Program.player.Inventory.Items[i].Visibility = Vis.Visible;
                    g.DrawImage(Program.player.Inventory.Items[i].Image, Program.player.Inventory.Items[i].CoordX * 16, Program.player.Inventory.Items[i].CoordY * 16);
                }
            }
            copyTileInv = Program.world[Program.player.CoordY, Program.player.CoordX].Inventory;
            for(int i = 0; i < 22; ++i) {
                if(copyTileInv.Items[i] != null) {
                    g.DrawImage(copyTileInv.Items[i].Image, (i % 11) + CellsX * 16, (i / 11) + CellsY2 * 16);
                }
            }
            // render tile inventory
            // render abilities
        }
    }
}

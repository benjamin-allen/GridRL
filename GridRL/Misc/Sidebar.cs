using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridRL {
    public class Sidebar : Sprite {

        private static int[] stats = new int[3];
        private static int InvCellSize = 16;
        private static int CellsY = 25;
        private static int CellsY2 = CellsY + 4;
        private static int CellsX = 67;
        Font ConsoleText = new Font("Courier New", 9);

        public Sidebar() : base() {
        }

        

        protected override void Paint(Graphics g) {
            //Drawing stats box
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
            Rectangle descBox = new Rectangle(66 * 16, 16 * 10, 13 * 16, 8 * 16);
            g.DrawRectangle(Pens.White, descBox);

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
            if(selectPoints[0] != -1) {
                int temp = CellsY;
                if(Program.MA == MouseArea.TileInv) {
                    CellsY = CellsY2;
                }
                Rectangle rect = new Rectangle((CellsX * InvCellSize) + selectPoints[1] * InvCellSize, (CellsY * InvCellSize) + selectPoints[0] * InvCellSize, InvCellSize, InvCellSize);
                g.DrawRectangle(new Pen(Color.FromArgb(208, 70, 72)), rect);
                CellsY = temp;
            }

            //Drawing ability grid
            int cellSize = 16 * 3;
            for(int i = 0; i < 3; i++) {
                for(int j = 0; j < 3; j++) {
                    Rectangle rect = new Rectangle(16 * 68 + i * cellSize, 33 * 16 + j * cellSize, cellSize, cellSize);
                    g.DrawRectangle(Pens.White, rect);
                }
            }
        }
    }
}

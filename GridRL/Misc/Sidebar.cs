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
        private static int CellsY = 11;
        private static int CellsY2 = CellsY + 6;
        private static int CellsX = 66;
        Font ConsoleText = new Font("Courier New", 9);

        public Sidebar() : base() {
        }

        

        protected override void Paint(Graphics g) {

            //Drawing stats box
            stats[0] = Program.player.Health;
            stats[1] = Program.player.Attack;
            stats[2] = Program.player.Defense;
            Rectangle statsBox = new Rectangle(1080, 10, 300, 100);
            g.DrawRectangle(Pens.White, statsBox);
            string statOutput =
                "Name: " + Program.player.Name + "\n" +
                "Health: " + stats[0] + "\n" +
                "Attack: " + stats[1] + "\n" +
                "Defense: " + stats[2];
            g.DrawString(statOutput,ConsoleText ,Brushes.White, statsBox);

            //Drawing player inventory slots
            g.DrawString("Player Inventory", ConsoleText, Brushes.White, 1080, 150);
            for(int i = 0 ; i < 2 ; i++) {
                for(int j = 0 ; j < 10 ; j++) {
                    Rectangle rect = new Rectangle((CellsX * InvCellSize) + j * InvCellSize, (CellsY * InvCellSize) + i * InvCellSize, InvCellSize, InvCellSize);
                    g.DrawRectangle(Pens.White, rect);
                }
            }

            //Drawing tile inventory slots
            g.DrawString("Tile Inventory", ConsoleText, Brushes.White, 1080, 250);
            for(int i = 0; i < 2; i++) {
                for(int j = 0; j < 10; j++) {
                    Rectangle rect = new Rectangle((CellsX * InvCellSize) + j * InvCellSize, (CellsY2 * InvCellSize) + i * InvCellSize, InvCellSize, InvCellSize);
                    g.DrawRectangle(Pens.White, rect);
                }
            }

            //Drawing ability grid
            int cellSize = 80;
            base.Paint(g);
            for(int i = 0; i < 3; i++) {
                for(int j = 0; j < 3; j++) {
                    Rectangle rect = new Rectangle(1080 + i * cellSize, 450 + j * cellSize, cellSize, cellSize);
                    g.DrawRectangle(Pens.DarkGreen, rect);
                }
            }
        }
    }
}

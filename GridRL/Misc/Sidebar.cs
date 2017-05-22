using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridRL {
    public class Sidebar : Sprite {

        private static int[] stats = new int[3];
        Font ConsoleText = new Font("Courier New", 9);

        public Sidebar() : base() {
        }

        

        protected override void Paint(Graphics g) {
            stats[0] = Program.player.Health;
            stats[1] = Program.player.Attack;
            stats[2] = Program.player.Defense;
            Rectangle statsBox = new Rectangle(1050, 10, 300, 100);
            g.DrawRectangle(Pens.White, statsBox);
            string statOutput =
                "Name: " + Program.player.Name + "\n" +
                "Health: " + stats[0] + "\n" +
                "Attack: " + stats[1] + "\n" +
                "Defense: " + stats[2];
            g.DrawString(statOutput,ConsoleText ,Brushes.White, statsBox);

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

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
        static ColorConverter cc = new ColorConverter();
        static Color[] Colors = new Color[9] {(Color)cc.ConvertFromString("#FF30346D"),
                                       (Color)cc.ConvertFromString("#FF854C30"),
                                       (Color)cc.ConvertFromString("#FF346524"),
                                       (Color)cc.ConvertFromString("#FF757161"),
                                       (Color)cc.ConvertFromString("#FF597DCE"),
                                       (Color)cc.ConvertFromString("#FFD27D2C"),
                                       (Color)cc.ConvertFromString("#FF8595A1"),
                                       (Color)cc.ConvertFromString("#FF6DAA2C"),
                                       (Color)cc.ConvertFromString("#FF6DC2CA"), };
        Color[] InvColors = new Color[9] {Color.FromArgb(Colors[0].ToArgb() ^ 0x00ffffff),
                                          Color.FromArgb(Colors[1].ToArgb() ^ 0x00ffffff),
                                          Color.FromArgb(Colors[2].ToArgb() ^ 0x00ffffff),
                                          Color.FromArgb(Colors[3].ToArgb() ^ 0x00ffffff),
                                          Color.FromArgb(Colors[4].ToArgb() ^ 0x00ffffff),
                                          Color.FromArgb(Colors[5].ToArgb() ^ 0x00ffffff),
                                          Color.FromArgb(Colors[6].ToArgb() ^ 0x00ffffff),
                                          Color.FromArgb(Colors[7].ToArgb() ^ 0x00ffffff),
                                          Color.FromArgb(Colors[8].ToArgb() ^ 0x00ffffff), };

        public Sidebar() : base() {
        }

        

        protected override void Paint(Graphics g) {
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

            //Drawing player inventory slots
            g.DrawString("Player Inventory", ConsoleText, Brushes.White, 16 * 67, 16 * (CellsY - 1));
            g.DrawString("Tile Inventory", ConsoleText, Brushes.White, 16 * 67, 16 * (CellsY2 - 1));
            for(int i = 0 ; i < 2 ; i++) {
                for(int j = 0 ; j < 11 ; j++) {
                    Rectangle rect = new Rectangle((CellsX * InvCellSize) + j * InvCellSize, (CellsY * InvCellSize) + i * InvCellSize, InvCellSize, InvCellSize);
                    Rectangle rect2 = new Rectangle((CellsX * InvCellSize) + j * InvCellSize, (CellsY2 * InvCellSize) + i * InvCellSize, InvCellSize, InvCellSize);
                    g.DrawRectangle(Pens.White, rect);
                    g.DrawRectangle(Pens.White, rect2);
                }
            }

            //Drawing ability grid
            for(int i = 0; i < 3; i++) {
                for(int j = 0; j < 3; j++) {
                    Rectangle rect = new Rectangle((16 * GridX + i * GridCellSize), GridY * 16 + j * GridCellSize, GridCellSize, GridCellSize);
                    g.DrawRectangle(Pens.White, rect);
                }
            }
            if(Program.waitState == 3) {
                foreach(List<int> points in Program.AbilityPlacePoints) {
                    g.FillRectangle(Brushes.Green, 16 * GridX + points[1] * GridCellSize + 1, 16 * GridY + points[0] * GridCellSize + 1, GridCellSize - 1, GridCellSize - 1);
                }
            }
            for(int i = 0; i < 22; ++i) {
                if(Program.player.Inventory.Items[i] != null) {
                    g.DrawImage(Program.player.Inventory.Items[i].Image, ((i % 11) + CellsX) * 16, ((i / 11) + CellsY) * 16);
                }
            }
            for(int i = 0; i < 22; ++i) {
                if(Program.world[Program.player.CoordY, Program.player.CoordX].Inventory.Items[i] != null) {
                    g.DrawImage(Program.world[Program.player.CoordY, Program.player.CoordX].Inventory.Items[i].Image, ((i % 11) + CellsX) * 16, ((i / 11) + CellsY2) * 16);
                }
            }
            if(Program.player.HeldItem != null) {
                g.DrawImage(Program.player.HeldItem.Image, 74 * 16, 16 * 18);
            }
            if(Program.player.WornArmor != null) {
                g.DrawImage(Program.player.WornArmor.Image, 74 * 16, 16 * 20);
            }
            if(Program.player.HeldWeapon != null) {
                g.DrawImage(Program.player.HeldWeapon.Image, 74 * 16, 16 * 22);
            }
            foreach(Ability a in Program.player.Abilities) {
                int y = a.GridY;
                int x = a.GridX;
                int w = a.GridWidth;
                int h = a.GridHeight;
                String text = a.Name;
                bool isPrinted = false;
                for(int j = y; j < y + h; ++j) {
                    for(int i = x; i < x + w; ++i) {
                        int index = Program.player.Abilities.IndexOf(a);
                        Rectangle rect = new Rectangle((GridX * 16) + (i * GridCellSize), (GridY * 16) + (j * GridCellSize), GridCellSize + 1, GridCellSize + 1);
                        g.FillRectangle(new SolidBrush(Colors[index]), rect);
                        if(!isPrinted) {
                            g.DrawString(text, new Font("Courier New", 8), new SolidBrush(InvColors[index]), rect);
                            isPrinted = true;
                        }
                    }
                }
            }
        }
    }
}

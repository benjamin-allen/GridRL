using System;
using System.Collections.Generic;
using System.Drawing;

namespace GridRL {
    public class ScreenOutput : Sprite {

        public static string text1 = "";
        public static string text2 = "";
        public static string text3 = "";
        public static string text4 = "";
        Rectangle box = new Rectangle(16,16 * 41, 16 * 64, 16 * 4);
        Font ConsoleText = new Font("Courier New", 9);

        public ScreenOutput() : base() {
        }

        public void SetText(string str) {
            text4 = text3;
            text3 = text2;
            text2 = text1;
            text1 = str;
        }

        protected override void Paint(Graphics g) {
            base.Paint(g);
            string output = text4 + "\n" + text3 + "\n" + text2 + "\n" + text1;
            g.FillRectangle(Brushes.Black, box);
            g.DrawString(output, ConsoleText ,Brushes.White, box);
        }
    }
}

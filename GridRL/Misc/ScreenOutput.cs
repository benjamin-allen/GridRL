using System;
using System.Collections.Generic;
using System.Drawing;

namespace GridRL {
    public class ScreenOutput : Sprite {

        public static string text1 = "";
        public static string text2 = "";
        public static string text3 = "";
        Rectangle box = new Rectangle(5,645, 1000, 50);
        Font ConsoleText = new Font("Courier New", 9);

        public ScreenOutput() : base() {
            SetText("Dummy Test message to see if the scrolling text will work??? Maybe if this text gets long enough I can test the line breaks too....");
        }

        public void SetText(string str) {
            text3 = text2;
            text2 = text1;
            text1 = str;
        }

        protected override void Paint(Graphics g) {
            base.Paint(g);
            string output = text3 + "\n" + text2 + "\n" + text1;
            g.FillRectangle(Brushes.Black, box);
            g.DrawString(output, ConsoleText ,Brushes.White, box);
        }
    }
}

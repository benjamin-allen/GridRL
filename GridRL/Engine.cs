using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GridRL
{
    public partial class Engine : Form
    {
        public static Form form;
        public static Sprite canvas = new Sprite();
        public static Sprite innerCanvas = new Sprite(canvas);
        public static Sprite innerInnerCanvas = new Sprite(innerCanvas);

        public Engine()
        {
            innerCanvas.X = 100;
            innerInnerCanvas.Y = 50;
            innerInnerCanvas.X = 50;
            InitializeComponent();
            DoubleBuffered = true;
            form = this;

            // main menu function goes here
            // if main menu returns something, start the game loop
            // otherwise spawn some help
            // otherwise exit the game
        }

        public static void gameLoop()
        {
            // call on player input
            // update player
            // update enemies
            // render
        }

        protected override void OnPaint(PaintEventArgs e) {
            canvas.Render(e.Graphics);
        }
    }
}

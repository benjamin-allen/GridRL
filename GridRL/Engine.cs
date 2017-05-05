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
        public static Thread gameThread;

        public Engine()
        {
            InitializeComponent();
            DoubleBuffered = true;
            form = this;

            gameThread = new Thread(new ThreadStart(gameLoop));
        }

        public static void gameLoop()
        {
            // wait for player input
            // update player
            // update enemies
            // render
        }
    }
}

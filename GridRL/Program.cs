using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GridRL
{
    public class Program : Engine
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.Run(new Engine());
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            //if(e.KeyCode == Keys.A) Console.WriteLine(innerInnerCanvas.X);
        }
    }
}

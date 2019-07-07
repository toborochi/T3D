using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T3D.Core.Game;

namespace T3D
{
    class Program
    {
        static void Main(string[] args)
        {
            new Game().Run(30, 30);

            using (var bmp = new Bitmap(100, 100))
            using (var gr = Graphics.FromImage(bmp))
            {
                gr.FillRectangle(Brushes.Orange, new Rectangle(0, 0, bmp.Width, bmp.Height));
                var path = System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "tea.png");
                bmp.Save(path);
            }
        }
    }
}

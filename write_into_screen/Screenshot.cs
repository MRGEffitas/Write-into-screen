using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace write_into_screen
{
    class Screenshot
    {
        // create the screenshot 
        public static void CaptureImage(Point SourcePoint, Point DestinationPoint, Rectangle SelectionRectangle)
        {

            using (Bitmap bitmap = new Bitmap(SelectionRectangle.Width, SelectionRectangle.Height))
            {

                using (Graphics g = Graphics.FromImage(bitmap))
                {

                    g.CopyFromScreen(SourcePoint, DestinationPoint, SelectionRectangle.Size);

                }
                    Image img = (Image)bitmap;
                    Clipboard.SetImage(img);
            }
        }
    }
}
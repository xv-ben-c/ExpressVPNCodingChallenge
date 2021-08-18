using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace ExpressVPNClientModel
{
    public class ServerIcon
    {

        private string testbase64icon = "iVBORw0KGgoAAAANSUhEUgAAABAAAAALCAIAAAD5gJpuAAAABGdBTUEAAK/I NwWK6QAAABl0RVh0U29mdHdhcmUAQWRvYmUgSW1hZ2VSZWFkeXHJZTwAAAHz SURBVHjaYkxOP8IAB//+Mfz7w8Dwi4HhP5CcJb/n/7evb16/APL/gRFQDiAA w3JuAgAIBEDQ/iswEERjGzBQLEru97ll0g0+3HvqMn1SpqlqGsZMsZsIe0SI CA5gt5a/AGIEarCPtFh+6N/ffwxA9OvP/7//QYwff/6fZahmePeB4dNHhi+f Gb59Y4zyvHHmCEAAAW3YDzQYaJJ93a+vX79aVf58//69fvEPlpIfnz59+vDh w7t37968efP3b/SXL59OnjwIEEAsDP+YgY53b2b89++/awvLn98MDi2cVxl+ /vl6mituCtBghi9f/v/48e/XL86krj9XzwEEEENy8g6gu22rfn78+NGs5Ofr 16+ZC58+fvyYwX8rxOxXr169fPny+fPn1//93bJlBUAAsQADZMEBxj9/GBxb 2P/9+S/R8u3vzxuyaX8ZHv3j8/YGms3w8ycQARmi2eE37t4ACCDGR4/uSkrK AS35B3TT////wADOgLOBIaXIyjBlwxKAAGKRXjCB0SOEaeu+/y9fMnz4AHQx CP348R/o+l+//sMZQBNLEvif3AcIIMZbty7Ly6t9ZmXl+fXj/38GoHH/UcGf P79//BBiYHjy9+8/oUkNAAHEwt1V/vI/KBY/QSISFqM/GBg+MzB8A6PfYC5E FiDAABqgW776MP0rAAAAAElFTkSuQmCC";

        public int Id { get; private set; }

        private Bitmap IconBitmap;

        internal ServerIcon(int id, string base64encoding)
        {
            Id = id;
            IconBitmap = base64encoding.Base64StringToBitmap();
        }

        private ImageSource ImgSrc;


        // Include, in your class or elsewhere:
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        //FIXME DISPOSE
        public ImageSource IconImageSource
        {
            get
            {
                if (ImgSrc == null)
                {
                    Bitmap image = IconBitmap;

                    IntPtr hbitmap = image.GetHbitmap();
                    try
                    {
                        ImgSrc = Imaging.CreateBitmapSourceFromHBitmap(
                           hbitmap, IntPtr.Zero, Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                    }
                    finally
                    {
                        // Clean up the bitmap data
                        DeleteObject(hbitmap);
                    }

                }
                return ImgSrc;


            }
        }



    }
}

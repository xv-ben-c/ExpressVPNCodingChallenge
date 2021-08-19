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

        public  int Id { get; private set; }

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
                        DeleteObject(hbitmap);
                    }

                }
                return ImgSrc;
            }
        }



    }
}

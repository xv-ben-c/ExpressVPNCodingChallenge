using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressVPNClientModel
{

    public static class IconHelper
    {
        public static Bitmap Base64StringToBitmap(this string base64String)

        {
            byte[] byteBuffer = Convert.FromBase64String(base64String);
            MemoryStream memoryStream = new MemoryStream(byteBuffer);


            memoryStream.Position = 0;


            Bitmap bmpReturn = (Bitmap)Image.FromStream(memoryStream);


            memoryStream.Close();


            return bmpReturn;
        }
    }

}

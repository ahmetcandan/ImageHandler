using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using ImageHandler.Helper;
using ImageHandler.Models;

namespace ImageHandler
{
    /// <summary>
    /// Summary description for Image
    /// </summary>
    public class ImageHandle : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var requestPaths = context.Request.Path.Split('/');
            Guid guid = Guid.Parse(requestPaths[requestPaths.Length - 1]);
            int width = 0, height = 0;

            if (context.Request.QueryString.ContainsKey("width"))
                width = int.Parse(context.Request.QueryString.Get("width"));
            if (context.Request.QueryString.ContainsKey("height"))
                height = int.Parse(context.Request.QueryString.Get("height"));

            string imageName = guid.ToString() + context.Request.QueryString.ToString() + ".jpeg";

            byte[] imageData = null;
            if (File.Exists("C://ImageHandler/Images/" + imageName + ".jpeg"))
                imageData = File.ReadAllBytes("C://ImageHandler/Images/" + imageName + ".jpeg");
            else
            {
                var tblImage = new ImageHandlerDBEntities().tbl_Image.FirstOrDefault(c => c.ImageId == guid);
                if (tblImage != null)
                    imageData = resizeImage(tblImage.ImageData, width, height, imageName);
            }

            context.Response.Clear();
            context.Response.ContentType = "jpeg";
            context.Response.OutputStream.Write(imageData, 0, imageData.Length);
            context.Response.End();
        }

        byte[] resizeImage(byte[] byteArrayIn, int width, int height, string url)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn, 0, byteArrayIn.Length);
            ms.Write(byteArrayIn, 0, byteArrayIn.Length);
            Image image = Image.FromStream(ms, true);
            int _width = width, _height = height;

            double factor = 1;

            if (width > 0 && height == 0)
                factor = (double)width / (double)image.Width;
            else if (width == 0 && height > 0)
                factor = (double)height / (double)image.Height;

            if (width == 0)
                _width = (int)(image.Width * factor);

            if (height == 0)
                _height = (int)(image.Height * factor);

            Image resizeImage = (new Bitmap(image, _width, _height));
            resizeImage.Save("C://ImageHandler/Images/" + url, ImageFormat.Jpeg);
            return ImageToByteArray(resizeImage);
        }

        public byte[] ImageToByteArray(Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
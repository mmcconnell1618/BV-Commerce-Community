using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace BVSoftware.Web
{
    public class Images
    {

        public static Size GetNewDimensions(int maxwidth, int maxheight, ref Bitmap originalImage)        
        {
            int height;
            int width;            
            height = originalImage.Height;
            width = originalImage.Width;
            return GetNewDimensions(maxwidth, maxheight, width, height);
        }

        public static Size GetNewDimensions(int maxwidth, int maxheight, int width, int height)
        {

            float multiplier;

            Size result = new Size(width, height);

            // both dimensions are within size
            if (height <= maxheight && width <= maxwidth)
                return result;

            multiplier = (float)((float)maxwidth / (float)width);

            if ((height * multiplier) <= maxheight)
            {
                height = (int)(height * multiplier);
                return new Size(maxwidth, height);
            }

            multiplier = (float)maxheight / (float)height;
            width = (int)(width * multiplier);
            return new Size(width, maxheight);
        }

        public static void ShrinkImageFileOnDisk(string originalFile, string newFileName, int maxWidth, int maxHeight)
        {
            //Bitmap origImg = (System.Drawing.Bitmap)Image.FromFile(originalFile).Clone();
            // Replaced 5.5 "FromFile" call with a manual loading of bytes
            FileStream fs = new FileStream(originalFile, FileMode.Open, FileAccess.Read);            
            Bitmap origImg = (System.Drawing.Bitmap)Image.FromStream(fs);
            fs.Close();
            fs.Dispose();
            
            Bitmap output = ShrinkImage(origImg, maxWidth, maxHeight);

            ImageFormat format = origImg.RawFormat;
            FileInfo tempFile = TempFiles.GetTemporaryFileInfo();
            output.Save(tempFile.FullName, format);
            if (File.Exists(newFileName))
            {
                File.SetAttributes(newFileName, FileAttributes.Normal);
                File.Delete(newFileName);
            }
            File.Copy(tempFile.FullName, newFileName);
            //output.Save(newFileName);
            origImg.Dispose();
            output.Dispose();
        }

        public static Bitmap ShrinkImage(Bitmap original, int maxWidth, int maxHeight)
        {
            Size ResizedDimensions = GetNewDimensions(maxWidth, maxHeight, ref original);
            Bitmap output = new Bitmap(ResizedDimensions.Width, ResizedDimensions.Height);
            Graphics g = Graphics.FromImage(output);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            Rectangle rect = new Rectangle(0, 0, ResizedDimensions.Width, ResizedDimensions.Height);
            g.DrawImage(original, rect, 0, 0, original.Width, original.Height, GraphicsUnit.Pixel);            
            
            return output;           
        }
    }
}

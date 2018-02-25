using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Image2Ico
{
    internal sealed class ImageHelper
    {
        /// <summary>
        /// Image Format
        /// </summary>
        public enum FileExt : Int32
        {
            PNG = 13780,
            JPG = 255216,
            BMP = 6677,
            NULL = 0
        }

        /// <summary>
        /// Get The File Type
        /// </summary>
        /// <param name="filePath">File Path</param>
        /// <returns>File Type</returns>
        public static FileExt GetFileType(String filePath)
        {
            FileExt extension = FileExt.NULL;
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                Byte[] buff = new Byte[2];
                try
                {
                    fileStream.Read(buff, 0, 2);
                    var result = buff[0].ToString() + buff[1].ToString();
                    var fileclass = Int32.Parse(result);
                    if (fileclass == (Int32)FileExt.PNG)
                    {
                        extension = FileExt.PNG;
                    }
                    else if (fileclass == (Int32)FileExt.JPG)
                    {
                        extension = FileExt.JPG;
                    }
                    else if (fileclass == (Int32)FileExt.BMP)
                    {
                        extension = FileExt.BMP;
                    }
                }
                catch (Exception ex)
                {
                    fileStream.Close();
                    Console.WriteLine(ex.Message);
                    return FileExt.NULL;
                }
            }
            return extension;
        }

        /// <summary>
        /// Convert Image To Icon
        /// </summary>
        /// <param name="image">Image</param>
        /// <returns>Icon</returns>
        // See Also http://blog.csdn.net/wangzibigan/article/details/79121924
        public static Icon ConvertToIcon(ImageInf imageinf)
        {
            using (var msImg = new MemoryStream())
            {
                using (var msIco = new MemoryStream())
                {
                    // Save Image As PNG And Put Into Stream
                    using (Bitmap bmp = new Bitmap(imageinf.Image, new Size(imageinf.TargetWidth, imageinf.TargetHeight)))
                    {
                        bmp.Save(msImg, ImageFormat.Png);
                    }
                    using (var bin = new BinaryWriter(msIco))
                    {
                        IconInf iconInf = new IconInf()
                        {
                            Width = (Byte)imageinf.TargetWidth,
                            Height = (Byte)imageinf.TargetHeight,
                            ImageSize = (Int32)msImg.Length,
                            ImageData = msImg.ToArray()
                        };

                        // Write Icon
                        bin.Write(iconInf.Header);
                        bin.Write(iconInf.Width);
                        bin.Write(iconInf.Height);
                        bin.Write(iconInf.ColorNum);
                        bin.Write(iconInf.Reserved);
                        bin.Write(iconInf.Planes);
                        bin.Write(iconInf.PixelBit);
                        bin.Write(iconInf.ImageSize);
                        bin.Write(iconInf.ImageOffSet);
                        bin.Write(iconInf.ImageData);
                        bin.Flush();
                        bin.Seek(0, SeekOrigin.Begin);

                        iconInf.Dispose();
                        return new Icon(msIco);
                    }
                }
            }
        }
    }
}

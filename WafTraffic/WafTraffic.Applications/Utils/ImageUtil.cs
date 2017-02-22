using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing.Imaging;
using DotNet.Utilities;
using DotNet.Business;

namespace WafTraffic.Applications.Utils
{
    [Export]
    public class ImageUtil
    {
        #region Data

        private static ImageUtil instance = null;
        private static object locker = new object();

        #endregion

        #region Constructor

        public static ImageUtil Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new ImageUtil();
                            //instance.Initialize();
                        }
                    }
                }
                return instance;
            }
        }

        #endregion

        #region Member

        /// 生成缩略图 
        /// </summary> 
        /// <param name="originalImagePath">源图路径</param> 
        /// <param name="thumbnailPath">缩略图路径</param> 
        /// <param name="width">缩略图宽度</param> 
        /// <param name="height">缩略图高度</param> 
        /// <param name="mode">生成缩略图的方式:HW指定高宽缩放(可能变形);W指定宽，高按比例 H指定高，宽按比例 Cut指定高宽裁减(不变形)</param>　　 
        /// <param name="mode">要缩略图保存的格式(gif,jpg,bmp,png) 为空或未知类型都视为jpg</param>　　 
        public void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode, string imageType)
        {
            Image originalImage = Image.FromFile(originalImagePath);
            int towidth = width;
            int toheight = height;
            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;
            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）　　　　　　　　 
                    break;
                case "W"://指定宽，高按比例　　　　　　　　　　 
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例 
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）　　　　　　　　 
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }
            //新建一个bmp图片 
            Image bitmap = new System.Drawing.Bitmap(towidth, toheight);
            //新建一个画板 
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            //设置高质量插值法 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //清空画布并以透明背景色填充 
            g.Clear(Color.Transparent);
            //在指定位置并且按指定大小绘制原图片的指定部分 
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
               new Rectangle(x, y, ow, oh),
               GraphicsUnit.Pixel);
            try
            {
                //以jpg格式保存缩略图 
                switch (imageType.ToLower())
                {
                    case "gif":
                        bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                    case "jpg":
                        bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case "bmp":
                        bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case "png":
                        bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    default:
                        bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                }
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }

        public Stream MakeThumbnail(string originalImagePath, int width, int height, string mode, string imageType)
        {
            Image originalImage = Image.FromFile(originalImagePath);
            int towidth = width;
            int toheight = height;
            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;
            Stream stream = null;
            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）　　　　　　　　 
                    break;
                case "W"://指定宽，高按比例　　　　　　　　　　 
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例 
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）　　　　　　　　 
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }
            //新建一个bmp图片 
            Image bitmap = new System.Drawing.Bitmap(towidth, toheight);
            //新建一个画板 
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            //设置高质量插值法 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //清空画布并以透明背景色填充 
            g.Clear(Color.Transparent);
            //在指定位置并且按指定大小绘制原图片的指定部分 
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
               new Rectangle(x, y, ow, oh),
               GraphicsUnit.Pixel);
            try
            {
                stream = new MemoryStream();
                //以jpg格式保存缩略图 
                switch (imageType.ToLower())
                {
                    case "gif":
                        bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                    case "jpg":
                        bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case "bmp":
                        bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case "png":
                        bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    default:
                        bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                }
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
            return stream;
        }

        public byte[] MakeThumbnail(string originalImagePath, int width, int height, string mode)
        {
            byte[] thumbnail = null;

            if (ValidateUtil.IsBlank(originalImagePath)) return null;

            Image originalImage = Image.FromFile(originalImagePath);
            int towidth = width;
            int toheight = height;
            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;
            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）　　　　　　　　 
                    break;
                case "W"://指定宽，高按比例　　　　　　　　　　 
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例 
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）　　　　　　　　 
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }
            //新建一个bmp图片 
            Image bitmap = new System.Drawing.Bitmap(towidth, toheight);
            //新建一个画板 
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            //设置高质量插值法 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //清空画布并以透明背景色填充 
            g.Clear(Color.Transparent);
            //在指定位置并且按指定大小绘制原图片的指定部分 
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
               new Rectangle(x, y, ow, oh),
               GraphicsUnit.Pixel);
            try
            {
                thumbnail = ImageToBytes(bitmap);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }

            return thumbnail;
        }


        public static byte[] BitmapimageToBytes(BitmapImage bmp)
        {
            byte[] bytearray = null;

            try
            {
                Stream smarket = bmp.StreamSource;

                if (smarket != null && smarket.Length > 0)
                {
                    //很重要，因为position经常位于stream的末尾，导致下面读取到的长度为0。 
                    smarket.Position = 0;

                    using (BinaryReader br = new BinaryReader(smarket))
                    {
                        bytearray = br.ReadBytes((int)smarket.Length);
                    }
                }
            }
            catch
            {
                //other exception handling 
            }

            return bytearray;
        }




        public byte[] ImageToBytes(Image img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, ImageFormat.Jpeg);
            byte[] bytes = new byte[ms.Length];
            ms.Seek(0, SeekOrigin.Begin);
            ms.Read(bytes, 0, Convert.ToInt32(ms.Length));
            return bytes;
        }

        public byte[] GetImageContent(string fileName)
        {
            BitmapImage bitmapImage;

            if (ValidateUtil.IsBlank(fileName)) return null;

            bitmapImage = new BitmapImage();

            bitmapImage.BeginInit();

            bitmapImage.StreamSource = System.IO.File.OpenRead(fileName);

            bitmapImage.EndInit();

            byte[] imageData = new byte[bitmapImage.StreamSource.Length];

            bitmapImage.StreamSource.Seek(0, System.IO.SeekOrigin.Begin);

            bitmapImage.StreamSource.Read(imageData, 0, imageData.Length);

            bitmapImage.StreamSource.Dispose();
            bitmapImage = null;

            return imageData;

        }

        public BitmapImage GetImageFromBytes(byte[] content)
        {
            BitmapImage newBitmapImage = null;
            try
            {
                if (content == null)
                {
                    return null;
                }
                System.IO.MemoryStream ms = new System.IO.MemoryStream(content);
                ms.Seek(0, System.IO.SeekOrigin.Begin);

                newBitmapImage = new BitmapImage();

                newBitmapImage.BeginInit();

                newBitmapImage.StreamSource = ms;

                newBitmapImage.EndInit();
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
            return newBitmapImage;
        }

        public BitmapImage GetImageFromStream(MemoryStream ms)
        {
            BitmapImage newBitmapImage = null;
            try
            {
                if (ms == null)
                {
                    return null;
                }
                ms.Seek(0, System.IO.SeekOrigin.Begin);

                newBitmapImage = new BitmapImage();

                newBitmapImage.BeginInit();

                newBitmapImage.StreamSource = ms;

                newBitmapImage.EndInit();
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
                newBitmapImage = null;
            }
            finally
            {
                if (ms != null)
                {
                    ms.Close();
                }
            }
            return newBitmapImage;
        }

        public bool ValidateImage(string file, int maxFileSize = 0, int maxWidth = 0, int maxHeight = 0)
        {
            try
            {
                if (maxFileSize > 0)
                {
                    byte[] bs = File.ReadAllBytes(file);
                    double size = (bs.Length / 1024);
                    if (size > maxFileSize) return false;
                }

                if (maxWidth > 0 && maxHeight > 0)
                {
                    Image img = Image.FromFile(file);
                    if (img.Width > maxWidth || img.Height > maxHeight) return false;
                }
                return true;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
                return false;
            }

        }

        #endregion
    }
}

using System;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Nhea.Helper
{
    [Obsolete("Image helper is not recommended since .Net System.Drawing GDI+ packages are only supported on Windows.")]
    public static class ImageHelper
    {
        public static byte[] ResizeImage(byte[] data, int width, int height, bool stretchImage)
        {
            int dimension = width;

            if (height != 0 && (width == 0 || height < width))
            {
                dimension = height;
            }

            return ResizeImage(data, width, height, stretchImage, DetermineQuality(dimension, ResizeType.MinDimension));
        }

        public static byte[] ResizeImage(byte[] data, int dimension, ResizeType resizeType)
        {
            return ResizeImage(data, dimension, resizeType, DetermineQuality(dimension, resizeType));
        }

        public static byte[] ResizeImage(byte[] data, int dimension, ResizeType resizeType, long quality)
        {
            try
            {
                if (dimension == 0)
                {
                    return data;
                }

                using (MemoryStream ms = new MemoryStream(data))
                {
                    Bitmap bitmap = new Bitmap(ms);

                    var adjustSizeResult = AdjustSizes(bitmap, dimension, resizeType);

                    //if (!adjustSizeResult.Resize)
                    //{
                    //    return data;
                    //}

                    int width = adjustSizeResult.Width;
                    int height = adjustSizeResult.Height;

                    return ResizeImageCore(bitmap, width, height, quality);
                }
            }
            catch (Exception ex)
            {
                Nhea.Logging.Logger.Log(ex);
                throw;
            }
        }

        public static byte[] ResizeImage(byte[] data, int width, int height, bool stretchImage, long quality)
        {
            if (width == 0 && height == 0)
            {
                return data;
            }

            using (MemoryStream ms = new MemoryStream(data))
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms);

                if (!stretchImage)
                {
                    if (image.Width > width || image.Height > height)
                    {
                        int sourceWidth = image.Width;
                        int sourceHeight = image.Height;

                        float nPercent = 0;
                        float nPercentW = 0;
                        float nPercentH = 0;

                        nPercentW = ((float)width / (float)sourceWidth);
                        nPercentH = ((float)height / (float)sourceHeight);

                        if (nPercentW == 0 || (nPercentH > 0 && nPercentH < nPercentW))
                            nPercent = nPercentH;
                        else
                            nPercent = nPercentW;

                        width = (int)(sourceWidth * nPercent);
                        height = (int)(sourceHeight * nPercent);
                    }
                }

                return ResizeImageCore(image, width, height, quality);
            }
        }

        private static byte[] ResizeImageCore(Image image, int width, int height, long quality)
        {
            System.Drawing.Image newImage = image.GetThumbnailImage(width, height, null, IntPtr.Zero);

            using (var thumbStream = new System.IO.MemoryStream())
            {
                Graphics graphics = Graphics.FromImage(newImage);
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.DrawImage(image, new Rectangle(0, 0, width, height));

                if (image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
                {
                    EncoderParameters encoderParams = new EncoderParameters(1);
                    EncoderParameter parameter = new EncoderParameter(Encoder.Quality, quality);
                    encoderParams.Param[0] = parameter;

                    newImage.Save(thumbStream, GetEncoderInfo("image/jpeg"), encoderParams);
                }
                else
                {
                    newImage.Save(thumbStream, image.RawFormat);
                }

                newImage.Dispose();
                return thumbStream.ToArray();
            }
        }

        public static void ChangeImageQuality(string filePath, long quality)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open);
            Image image = Image.FromStream(fs);
            fs.Close();

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);

            //if (System.IO.File.Exists(filePath))
            //    System.IO.File.Delete(filePath);

            image.Save(filePath, GetEncoderInfo("image/jpeg"), encoderParams);
            image.Dispose();
        }

        public static byte[] CropImage(byte[] data, int width, int height, VerticalCrop verticalCrop, HorizontalCrop horizontalCrop)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                Bitmap bitmap = null;
                System.Drawing.Image image = null;
                try
                {

                    image = System.Drawing.Image.FromStream(stream);

                    Rectangle r = new Rectangle(0, 0, width, height);

                    if (r.Width > image.Width)
                    {
                        r.Width = image.Width;
                    }

                    if (r.Height > image.Height)
                    {
                        r.Height = image.Height;
                    }

                    if (verticalCrop == VerticalCrop.Center)
                    {
                        r.Y = (image.Height / 2) - r.Height / 2;
                    }
                    else if (verticalCrop == VerticalCrop.Bottom)
                    {
                        r.Y = image.Height - r.Height;
                    }

                    if (horizontalCrop == HorizontalCrop.Center)
                    {
                        r.X = (image.Width / 2) - r.Width / 2;
                    }
                    else if (horizontalCrop == HorizontalCrop.Right)
                    {
                        r.X = image.Width - r.Width;
                    }

                    bitmap = new Bitmap(r.Width, r.Height, PixelFormat.Format32bppArgb);
                    bitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);
                    Graphics graphics = Graphics.FromImage(bitmap);
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    graphics.DrawImage(image, new Rectangle(0, 0, r.Width, r.Height), r, GraphicsUnit.Pixel);
                    graphics.Dispose();

                    var imageInfo = CheckImageFormat(image);

                    image.Dispose();

                    EncoderParameters encoderParams = new EncoderParameters(1);
                    EncoderParameter parameter = new EncoderParameter(Encoder.Quality, (long)100);
                    encoderParams.Param[0] = parameter;

                    using (var thumbStream = new System.IO.MemoryStream())
                    {
                        bitmap.Save(thumbStream, GetEncoderInfo("image/jpeg"), encoderParams);
                        bitmap.Dispose();
                        return thumbStream.GetBuffer();
                    }
                }
                catch
                {
                    if (bitmap != null)
                    {
                        bitmap.Dispose();
                    }
                    if (image != null)
                    {
                        image.Dispose();
                    }
                }

                return data;
            }
        }

        public static void SaveImage(byte[] data, string destinationFilePath)
        {
            if (File.Exists(destinationFilePath))
            {
                File.Delete(destinationFilePath);
            }

            using (MemoryStream stream = new MemoryStream(data))
            {
                Bitmap bitmap = new Bitmap(stream);

                string extension = destinationFilePath.Substring(destinationFilePath.LastIndexOf('.'));

                switch (extension)
                {
                    case ".png":
                        bitmap.Save(destinationFilePath, ImageFormat.Png);
                        break;

                    case ".gif":
                        bitmap.Save(destinationFilePath, ImageFormat.Gif);
                        break;

                    case ".jpg":
                    case ".jpeg":
                        {
                            EncoderParameters encoderParams = new EncoderParameters(1);
                            encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, (long)100);
                            bitmap.Save(destinationFilePath, GetEncoderInfo("image/jpeg"), encoderParams);
                            break;
                        }
                    case ".tif":
                    case ".tiff":
                        bitmap.Save(destinationFilePath, ImageFormat.Tiff);
                        break;

                    case ".bmp":
                        bitmap.Save(destinationFilePath, ImageFormat.Bmp);
                        break;
                }

                bitmap.Dispose();
            }
        }

        public static long DetermineQuality(int dimension, ResizeType resizeType)
        {
            if (resizeType == ResizeType.MinDimension)
            {
                if (dimension >= 300)
                {
                    return 90;
                }
                else if (dimension >= 150)
                {
                    return 95;
                }
            }
            else
            {
                if (dimension >= 1200)
                {
                    return 85;
                }
                else if (dimension >= 800)
                {
                    return 90;
                }
                else if (dimension >= 400)
                {
                    return 95;
                }
            }

            return 100;
        }

        //public static byte[] SaveAsJpeg(byte[] data, long quality)
        //{
        //    using (MemoryStream stream = new MemoryStream(data))
        //    {
        //        Bitmap bitmap = null;
        //        System.Drawing.Image image = null;
        //        try
        //        {
        //            image = System.Drawing.Image.FromStream(stream);

        //            bitmap = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
        //            bitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        //            Graphics graphics = Graphics.FromImage(bitmap);
        //            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //            graphics.SmoothingMode = SmoothingMode.HighQuality;
        //            graphics.CompositingQuality = CompositingQuality.HighQuality;
        //            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

        //            Rectangle destRect = new Rectangle(0, 0, image.Width, image.Height);
        //            Rectangle srcRect = new Rectangle(0, 0, image.Width, image.Height);
        //            graphics.Clear(Color.White);
        //            graphics.DrawImageUnscaled(image, 0, 0);
        //            graphics.Dispose();

        //            image.Dispose();

        //            EncoderParameters encoderParams = new EncoderParameters(1);
        //            EncoderParameter parameter = new EncoderParameter(Encoder.Quality, quality);
        //            encoderParams.Param[0] = parameter;

        //            using (var thumbStream = new System.IO.MemoryStream())
        //            {
        //                image.Save(thumbStream, GetEncoderInfo("image/jpeg"), encoderParams);
        //                return thumbStream.GetBuffer();
        //            }
        //        }
        //        catch
        //        {
        //            if (bitmap != null)
        //            {
        //                bitmap.Dispose();
        //            }
        //            if (image != null)
        //            {
        //                image.Dispose();
        //            }
        //        }

        //        return data;
        //    }
        //}

        public static ImageInfo CheckImageFormat(Image image)
        {
            ImageFormat format = image.RawFormat;

            if (image.RawFormat.Guid == new Guid("b96b3caa-0728-11d3-9d7b-0000f81ef32e"))
            {
                return new ImageInfo(ImageFormat.MemoryBmp, ".bmp", "image/bmp");
            }

            ImageCodecInfo codec = ImageCodecInfo.GetImageDecoders().First(c => c.FormatID == format.Guid);
            string mimeType = codec.MimeType;
            System.Drawing.Imaging.ImageFormat imageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;

            switch (mimeType)
            {
                case "image/gif":
                    return new ImageInfo(ImageFormat.Gif, ".gif", "image/gif");
                case "image/png":
                    return new ImageInfo(ImageFormat.Png, ".png", "image/png");
                case "image/jpeg":
                case "image/jpg":
                default:
                    return new ImageInfo(ImageFormat.Jpeg, ".jpg", "image/jpeg");
                case ".tif":
                case ".tiff":
                    return new ImageInfo(ImageFormat.Tiff, ".tif", "image/tif");
                case ".bmp":
                    return new ImageInfo(ImageFormat.Bmp, ".bmp", "image/bmp");
            }
        }

        public class ImageInfo
        {
            public ImageInfo(ImageFormat format, string extension, string mimeType)
            {
                this.Format = format;
                this.Extension = extension;
                this.MimeType = mimeType;
                this.CodecInfo = GetEncoderInfo(this.MimeType);
            }

            public ImageFormat Format { get; set; }

            public string Extension { get; set; }

            public string MimeType { get; set; }

            public ImageCodecInfo CodecInfo { get; set; }
        }

        public static ImageCodecInfo GetEncoderInfo(string mime_type)
        {
            ImageCodecInfo[] infoArray = ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i < infoArray.Length; i++)
            {
                if (infoArray[i].MimeType == mime_type)
                {
                    return infoArray[i];
                }
            }
            return null;
        }

        public enum ResizeType
        {
            MinDimension,
            MaxDimension
        }

        public enum VerticalCrop
        {
            Top,
            Center,
            Bottom
        }

        public enum HorizontalCrop
        {
            Left,
            Center,
            Right
        }

        public static AdjustedSizesResult AdjustSizes(Bitmap bitmap, int dimension, ResizeType resizeType)
        {
            AdjustedSizesResult adjustedSizes = new AdjustedSizesResult();

            if (resizeType == ResizeType.MinDimension)
            {
                int relatedDimension = 0;
                adjustedSizes.Resize = true;

                if (bitmap.Height > bitmap.Width)
                {
                    adjustedSizes.Width = dimension;
                    adjustedSizes.Height = Convert.ToInt32((double)bitmap.Height / ((double)bitmap.Width / (double)dimension));
                    relatedDimension = bitmap.Width;
                }
                else
                {
                    adjustedSizes.Height = dimension;
                    adjustedSizes.Width = Convert.ToInt32((double)bitmap.Width / ((double)bitmap.Height / (double)dimension));
                    relatedDimension = bitmap.Height;
                }

                if (relatedDimension <= dimension)
                {
                    adjustedSizes.Resize = false;
                }
            }
            else
            {
                int relatedDimension = 0;
                adjustedSizes.Resize = true;

                if (bitmap.Height < bitmap.Width)
                {
                    adjustedSizes.Width = dimension;
                    adjustedSizes.Height = Convert.ToInt32((double)bitmap.Height / ((double)bitmap.Width / (double)dimension));
                    relatedDimension = bitmap.Width;
                }
                else
                {
                    adjustedSizes.Height = dimension;
                    adjustedSizes.Width = Convert.ToInt32((double)bitmap.Width / ((double)bitmap.Height / (double)dimension));
                    relatedDimension = bitmap.Height;
                }

                if (relatedDimension <= dimension)
                {
                    adjustedSizes.Resize = false;
                }
            }

            return adjustedSizes;
        }

        public static AdjustedSizesResult AdjustSizes(Bitmap bitmap, int width, int height)
        {
            AdjustedSizesResult adjustedSizes = new AdjustedSizesResult();
            adjustedSizes.Resize = true;

            if (width == 0 || height == 0)
            {
                if (height == 0)
                {
                    adjustedSizes.Width = width;
                    adjustedSizes.Height = Convert.ToInt32((double)bitmap.Height / ((double)bitmap.Width / (double)width));
                }
                else
                {
                    adjustedSizes.Height = height;
                    adjustedSizes.Width = Convert.ToInt32((double)bitmap.Width / ((double)bitmap.Height / (double)height));
                }
            }

            return adjustedSizes;
        }

        public class AdjustedSizesResult
        {
            public int Width { get; set; }

            public int Height { get; set; }

            public bool Resize { get; set; }
        }

        private static bool ThumbnailCallback()
        {
            return false;
        }
    }
}

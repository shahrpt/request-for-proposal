namespace UrbanRFP.Infrastructure.Helpers
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Net;

    public static class ImageExtension
    {
        public static void SaveImage(string fileFullName, byte[] photo)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileFullName));
                WriteByteArrayToFile(fileFullName, photo);
            }
            catch { throw; }
        }

        public static void SaveImageBase64(string fileFullName, string base64String)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileFullName));

                Image originalImage = Base64ToImage(base64String);
                Image grayImage = MakeGrayscale(originalImage);
                File.WriteAllBytes(fileFullName, System.Convert.FromBase64String(base64String));
            }
            catch { throw; }
        }


        public static Image MakeGrayscale(this Image original)
        {
            try
            {
                //create a blank bitmap the same size as original
                Bitmap newBitmap = new Bitmap(original.Width, original.Height);

                //get a graphics object from the new image
                Graphics g = Graphics.FromImage(newBitmap);

                //create the grayscale ColorMatrix
                ColorMatrix colorMatrix = new ColorMatrix(
                   new float[][]
                  {
                     new float[] {.3f, .3f, .3f, 0, 0},
                     new float[] {.59f, .59f, .59f, 0, 0},
                     new float[] {.11f, .11f, .11f, 0, 0},
                     new float[] {0, 0, 0, 1, 0},
                     new float[] {0, 0, 0, 0, 1}
                  });

                //create some image attributes
                ImageAttributes attributes = new ImageAttributes();

                //set the color matrix attribute
                attributes.SetColorMatrix(colorMatrix);

                //draw the original image on the new image
                //using the grayscale color matrix
                g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                   0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

                //dispose the Graphics object
                g.Dispose();
                return newBitmap;
            }
            catch { throw; }
        }

        public static Image Base64ToImage(string base64String)
        {
            try
            {
                // Convert Base64 String to byte[]
                byte[] imageBytes = Convert.FromBase64String(base64String);
                MemoryStream ms = new MemoryStream(imageBytes, 0,
                  imageBytes.Length);

                // Convert byte[] to Image
                ms.Write(imageBytes, 0, imageBytes.Length);
                Image image = Image.FromStream(ms, true);
                return image;
            }
            catch { throw; }
        }

        public static Bitmap ImageURLToBitmap(string _imageWebURL)
        {
            try
            {
                Bitmap bitmap;
                WebClient client = new WebClient();
                Stream stream = client.OpenRead(_imageWebURL);
                bitmap = new Bitmap(stream);
                stream.Flush();
                stream.Close();
                return bitmap;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static string ToBase64(this Image image, ImageFormat format)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    // Convert Image to byte[]
                    image.Save(ms, format);
                    byte[] imageBytes = ms.ToArray();

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
            catch { throw; }
        }

        public static Bitmap Base64StringToBitmap(this string base64String)
        {
            try
            {
                Bitmap bmpReturn = null;
                byte[] byteBuffer = Convert.FromBase64String(base64String);
                MemoryStream memoryStream = new MemoryStream(byteBuffer);
                memoryStream.Position = 0;
                bmpReturn = (Bitmap)Bitmap.FromStream(memoryStream);
                memoryStream.Close();
                memoryStream = null;
                byteBuffer = null;
                return bmpReturn;
            }
            catch { throw; }
        }

        public static string BitmapToBase64String(this Bitmap bmp, ImageFormat imageFormat)
        {
            try
            {
                string base64String = string.Empty;
                MemoryStream memoryStream = new MemoryStream();
                bmp.Save(memoryStream, imageFormat);
                memoryStream.Position = 0;
                byte[] byteBuffer = memoryStream.ToArray();
                memoryStream.Close();
                base64String = Convert.ToBase64String(byteBuffer);
                byteBuffer = null;
                return base64String;
            }
            catch { throw; }
        }

        public static void WriteByteArrayToFile(string fileFullName, byte[] buffer)
        {
            try
            {
                using (FileStream fs = new FileStream(fileFullName, FileMode.Create, FileAccess.ReadWrite))
                {
                    fs.Write(buffer, 0, (int)buffer.Length);
                }
            }
            catch { throw; }
        }

        public static Image ByteArrayToImage(byte[] byteArrayIn)
        {
            try
            {
                MemoryStream ms = new MemoryStream(byteArrayIn);
                Image returnImage = Image.FromStream(ms);
                return returnImage;
            }
            catch { throw; }
        }

        public static byte[] CropImage(this Image img, Rectangle cropArea)
        {
            try
            {
                Bitmap bmpImage = new Bitmap(img);
                Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
                return ImageToByte((Image)(bmpCrop));
            }
            catch { throw; }
        }

        public static byte[] ImageToByte(this Image img)
        {
            try
            {
                ImageConverter converter = new ImageConverter();
                return (byte[])converter.ConvertTo(img, typeof(byte[]));
            }
            catch { throw; }
        }

        public static Image ScaleImage(this Image image, double imageSize)
        {
            try
            {
                var ratioX = imageSize / image.Width;
                var ratioY = imageSize / image.Height;
                var ratio = Math.Min(ratioX, ratioY);

                var newWidth = (int)(image.Width * ratio);
                var newHeight = (int)(image.Height * ratio);

                var newImage = new Bitmap(newWidth, newHeight);
                Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
                return newImage;
            }
            catch { throw; }
        }

        public static Image CompressImage(this Image _image)
        {
            try
            {
                ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);

                // Create an Encoder object based on the GUID
                // for the Quality parameter category.
                Encoder myEncoder = Encoder.Quality;

                // Create an EncoderParameters object.
                // An EncoderParameters object has an array of EncoderParameter
                // objects. In this case, there is only one
                // EncoderParameter object in the array.
                EncoderParameters myEncoderParameters = new EncoderParameters(1);

                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                myEncoderParameters.Param[0] = myEncoderParameter;
                var stream = new MemoryStream();
                _image.Save(stream, jgpEncoder, myEncoderParameters);
                stream.Position = 0;
                return Image.FromStream(stream);
            }
            catch { throw; }
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            try
            {
                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

                foreach (ImageCodecInfo codec in codecs)
                {
                    if (codec.FormatID == format.Guid)
                    {
                        return codec;
                    }
                }
                return null;
            }
            catch { throw; }
        }

        public static void SaveImage(this Image image, string folderPath, string fileName)
        {
            double thumbnailSize = 0.0, imageSize = 0.0;
            double.TryParse(Utility.GetSetting("ThumbnailSize"), out thumbnailSize);
            double.TryParse(Utility.GetSetting("ImageSize"), out imageSize);
            string thumbnailPath = string.Format("{0}x{0}", thumbnailSize);

            if (thumbnailSize == 0.0) thumbnailSize = 100;
            if (imageSize == 0.0) imageSize = 500;

            //Image compressedImage = image.CompressImage();
            Image standardImage = image.ScaleImage(imageSize);
            Image thumbnailImage = image.ScaleImage(thumbnailSize);
            Directory.CreateDirectory(folderPath);
            standardImage.Save(Path.Combine(folderPath, fileName));
            Directory.CreateDirectory(Path.Combine(folderPath, thumbnailPath));
            thumbnailImage.Save(Path.Combine(folderPath, thumbnailPath, fileName));

            image.Dispose();
            //compressedImage.Dispose();
            standardImage.Dispose();
            thumbnailImage.Dispose();

            GC.Collect();
        }
    }
}

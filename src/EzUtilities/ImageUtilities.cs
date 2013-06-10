using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace EzUtilities
{
    public static class ImageUtilities
    {
        /// <summary>
        /// The exception that is thrown when an attempt to load an image fails.
        /// </summary>
        public class InvalidImageException : Exception
        {
            private readonly string _path;
            private readonly bool _isImage;

            /// <summary>
            /// Gets whether the file is an image; false means that the image 
            /// is corrupted or is too large.
            /// </summary>
            public bool IsImage
            {
                get { return _isImage; }
            }
            /// <summary>
            /// Gets the path to the file.
            /// </summary>
            public string Path
            {
                get { return _path; }
            }

            public InvalidImageException(string path, bool isImage)
                : base("The file is not an image, is corrupted, or is too large")
            {
                _path = path;
                _isImage = isImage;
            }
        }

        /// <summary>
        /// Loads an image and splits it into individual frames.
        /// </summary>
        /// <param name="path">The path to the image.</param>
        public static Image[] LoadFrames(string path)
        {
            Image img;
            try
            {
                img = Image.FromFile(path);
            }
            catch (ArgumentException)
            {
                //Probably not an image
                throw new InvalidImageException(path, false);
            }
            catch (OutOfMemoryException)
            {
                //Probably a corrupted image
                throw new InvalidImageException(path, true);
            }

            //Check for multi-frame images
            FrameDimension dimension = new FrameDimension(img.FrameDimensionsList[0]);
            int frameCount = img.GetFrameCount(dimension);

            Image[] frames = new Image[frameCount];
            if (frameCount == 1)
            {
                //Single-frame image
                frames[0] = img;
            }
            else
            {
                //Multi-frame image
                for (int index = 0; index < frameCount; ++index)
                {
                    frames[index] = img.GetFrame(dimension, index);
                }

                img.Dispose();
            }

            return frames;
        }

        /// <summary>
        /// Gets a specified frame of an image.
        /// </summary>
        /// <param name="img">The image to extract the frame from.</param>
        /// <param name="dimension">The frame dimensions of the image.</param>
        /// <param name="frameIndex">The zero-based index of the frame.</param>
        private static Image GetFrame(this Image img, FrameDimension dimension, int frameIndex)
        {
            Image imgFrame;

            img.SelectActiveFrame(dimension, frameIndex);
            using (MemoryStream byteStream = new MemoryStream())
            {
                img.Save(byteStream, img.RawFormat);
                imgFrame = Image.FromStream(byteStream);
            }

            return imgFrame;
        }

        /// <summary>
        /// Saves the image as a JPEG file with the specified quality, creating the containing directory.
        /// </summary>
        /// <param name="img">The image to save.</param>
        /// <param name="path">The path to the file.</param>
        /// <param name="quality">The quality of the image (0 to 100).</param>
        public static void SaveAsJpeg(this Image img, string path, long quality)
        {
            var encoder = GetEncoder(ImageFormat.Jpeg);
            var encoderParams = GetQualityEncoderParams(quality);

            img.EzSave(path, encoder, encoderParams);
        }

        /// <summary>
        /// Saves the image as a PNG file, creating the containing directory.
        /// </summary>
        /// <param name="img">The image to save.</param>
        /// <param name="path">The path to the file.</param>
        public static void SaveAsPng(this Image img, string path)
        {
            img.EzSave(path, ImageFormat.Png);
        }

        /// <summary>
        /// Saves the image as a BMP file, creating the containing directory.
        /// </summary>
        /// <param name="img">The image to save.</param>
        /// <param name="path">The path to the file.</param>
        public static void SaveAsBmp(this Image img, string path)
        {
            img.EzSave(path, ImageFormat.Bmp);
        }

        /// <summary>
        /// Saves the image, creating the containing directory.
        /// </summary>
        /// <param name="img">The image to save.</param>
        /// <param name="path">The path to the file.</param>
        public static void EzSave(this Image img, string path)
        {
            IOUtilities.CreateParentDirectory(path);
            img.Save(path);
        }

        /// <summary>
        /// Saves the image with the specified format, creating the containing directory.
        /// </summary>
        /// <param name="img">The image to save.</param>
        /// <param name="path">The path to the file.</param>
        /// <param name="format">The format of the image.</param>
        public static void EzSave(this Image img, string path, ImageFormat format)
        {
            IOUtilities.CreateParentDirectory(path);
            img.Save(path, format);
        }

        /// <summary>
        /// Saves the image with the specified encoder and parameters, creating the containing directory.
        /// </summary>
        /// <param name="img">The image to save.</param>
        /// <param name="path">The path to the file.</param>
        /// <param name="encoder">The encoder information for this image.</param>
        /// <param name="encoderParams">The encoder parameters for this image.</param>
        public static void EzSave(this Image img, string path, ImageCodecInfo encoder, EncoderParameters encoderParams)
        {
            IOUtilities.CreateParentDirectory(path);
            img.Save(path, encoder, encoderParams);
        }

        /// <summary>
        /// Gets an encoder parameter that specifies the quality of the image.
        /// </summary>
        /// <param name="quality"></param>
        public static EncoderParameters GetQualityEncoderParams(long quality)
        {
            if (quality < 0 || quality > 100) throw new ArgumentOutOfRangeException("quality");
            EncoderParameters ep = new EncoderParameters();
            ep.Param[0] = new EncoderParameter(Encoder.Quality, quality);
            return ep;
        }

        /// <summary>
        /// Gets the image encoder information that corresponds to an image format.
        /// </summary>
        /// <param name="format">The image format.</param>
        /// <returns></returns>
        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
        }
    }
}
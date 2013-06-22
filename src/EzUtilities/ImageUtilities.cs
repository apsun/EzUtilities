using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace EzUtilities
{
    /// <summary>
    /// Provides tools to load and save images.
    /// </summary>
    public static class ImageUtilities
    {
        /// <summary>
        /// The exception that is thrown when an attempt to load an image fails.
        /// </summary>
        public class InvalidImageException : Exception
        {
            private readonly string _path;

            /// <summary>
            /// Gets the path to the file.
            /// </summary>
            public string Path
            {
                get { return _path; }
            }

            /// <summary>
            /// Instantiates a new instance of the <see cref="InvalidImageException"/> class.
            /// </summary>
            public InvalidImageException()
                : this(null)
            {

            }

            /// <summary>
            /// Instantiates a new instance of the <see cref="InvalidImageException"/> class.
            /// </summary>
            /// 
            /// <param name="path">The path to the image.</param>
            public InvalidImageException(string path)
                : base("The file is not an image, is corrupted, or is too large")
            {
                _path = path;
            }
        }

        /// <summary>
        /// Loads an image, converting ArgumentException and 
        /// OutOfMemoryException to InvalidImageException
        /// </summary>
        /// 
        /// <param name="path">The path to the image file.</param>
        /// 
        /// <exception cref="InvalidImageException">
        /// Thrown if the file is not an image, corrupted, 
        /// or a PNG image with a dimension greater than 65,535 px.
        /// </exception>
        public static Image FromFileConvertException(string path)
        {
            Image img;
            try
            {
                img = Image.FromFile(path);
            }
            catch (ArgumentException)
            {
                //Probably not an image or a PNG with a dimension greater than 65,535 px
                throw new InvalidImageException(path);
            }
            catch (OutOfMemoryException)
            {
                //Probably a corrupted image or we really ran out of memory
                throw new InvalidImageException(path);
            }

            return img;
        }

        /// <summary>
        /// Loads an image from a file without locking the file.
        /// </summary>
        /// 
        /// <param name="path">The path to the image file.</param>
        /// <param name="stream">The MemoryStream that contains data for the image.</param>
        public static Image FromFileNoLock(string path, out MemoryStream stream)
        {
            stream = new MemoryStream();
            using (Image img = Image.FromFile(path))
            {
                img.Save(stream, img.RawFormat);
            }
            return Image.FromStream(stream);
        }

        /// <summary>
        /// Loads an image from a file without locking the file. 
        /// This does not create any <see cref="System.IO.Stream"/> 
        /// dependencies, but will lose some data, notably the 
        /// <see cref="System.Drawing.Image.RawFormat"/> property.
        /// </summary>
        /// 
        /// <param name="path">The path to the image file.</param>
        /// 
        /// <exception cref="InvalidImageException">
        /// Thrown if the file is not an image, corrupted, 
        /// or a PNG image with a dimension greater than 65,535 px.
        /// </exception>
        /// 
        /// <exception cref="System.IO.FileNotFoundException">The specified file does not exist.</exception>
        public static Image CloneFromFile(string path)
        {
            //tmp is locked, so we need to copy its pixels into a new image
            using (Image tmp = FromFileConvertException(path))
            {
                Image img = new Bitmap(tmp);
                return img;
            }
        }

        /// <summary>
        /// Loads an image from a stream without needing to keep the 
        /// stream open over the lifetime of the image. Note that this will 
        /// lose some data, notably the <see cref="System.Drawing.Image.RawFormat"/> property.
        /// </summary>
        /// 
        /// <param name="stream">The stream that contains the data for this image.</param>
        /// 
        /// <exception cref="System.ArgumentException">The stream does not have a valid image format-or-stream is null.</exception>
        public static Image CloneFromStream(Stream stream)
        {
            //tmp has an internal dependency on the stream
            using (Image tmp = Image.FromStream(stream))
            {
                Image img = new Bitmap(tmp);
                return img;
            }
        }

        /// <summary>
        /// Loads an image and splits it into individual frames. 
        /// Will throw an exception if the image has more than one frame dimension.
        /// </summary>
        /// 
        /// <param name="path">The path to the image.</param>
        /// <param name="streams">The MemoryStreams that correspond to the images.</param>
        /// 
        /// <exception cref="InvalidImageException">
        /// Thrown if the file is not an image, corrupted, 
        /// or a PNG image with a dimension greater than 65,535 px.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Thrown if the image does not have exactly one frame dimension.
        /// </exception>
        public static Image[] LoadFrames(string path, out MemoryStream[] streams)
        {
            using (Image img = FromFileConvertException(path))
            {
                Image[] frames = img.ToFrames(out streams);
                return frames;
            }
        }

        /// <summary>
        /// Splits an image into individual frames. 
        /// Will throw an exception if the image has more than one frame dimension.
        /// </summary>
        /// 
        /// <param name="img">The image to split.</param>
        /// <param name="streams">The MemoryStreams that correspond to the images.</param>
        /// 
        /// <exception cref="System.ArgumentException">
        /// Thrown if the image does not have exactly one frame dimension.
        /// </exception>
        public static Image[] ToFrames(this Image img, out MemoryStream[] streams)
        {
            Guid[] dimensions = img.FrameDimensionsList;
            if (dimensions.Length < 1)
            {
                throw new ArgumentException("Image has no frame dimensions");
            }
            if (dimensions.Length > 1)
            {
                throw new ArgumentException("Image has more than one frame dimension");
            }

            FrameDimension dimension = new FrameDimension(dimensions[0]);
            return img.ToFrames(dimension, out streams);
        }

        /// <summary>
        /// Splits an image into individual frames using the specified dimension.
        /// </summary>
        /// 
        /// <param name="img">The image to split.</param>
        /// <param name="dimension">The dimension to use when determining frames.</param>
        /// <param name="streams">The MemoryStreams that correspond to the images.</param>
        public static Image[] ToFrames(this Image img, FrameDimension dimension, out MemoryStream[] streams)
        {
            //We need to copy the image, or else SelectActiveFrame will
            //cause permanant changes to the original image.
            using (MemoryStream origStream = img.SaveToStream(img.RawFormat))
            using (Image copy = Image.FromStream(origStream))
            {
                int frameCount = copy.GetFrameCount(dimension);
                Image[] frames = new Image[frameCount];
                streams = new MemoryStream[frameCount];
                for (int index = 0; index < frameCount; ++index)
                {
                    copy.SelectActiveFrame(dimension, index);
                    MemoryStream frameStream = copy.SaveToStream(ImageFormat.Bmp);
                    Image imgFrame = Image.FromStream(frameStream);
                    frames[index] = imgFrame;
                    streams[index] = frameStream;
                }

                return frames;
            }
        }

        /// <summary>
        /// Saves this image to a new stream.
        /// </summary>
        /// 
        /// <param name="img">The image to save.</param>
        /// <param name="format">The format of the image.</param>
        /// 
        /// <exception cref="System.ArgumentNullException"><see cref="format"/> is null.</exception>
        /// <exception cref="System.Runtime.InteropServices.ExternalException">The image was saved with the wrong image format</exception>
        public static MemoryStream SaveToStream(this Image img, ImageFormat format)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, format);
            return ms;
        }

        /// <summary>
        /// Saves this image to a new stream.
        /// </summary>
        /// 
        /// <param name="img">The image to save.</param>
        /// <param name="encoder">The encoder information for this image.</param>
        /// <param name="encoderParams">The encoder parameters for this image.</param>
        /// 
        /// <exception cref="System.Runtime.InteropServices.ExternalException">The image was saved with the wrong image format.</exception>
        public static MemoryStream SaveToStream(this Image img, ImageCodecInfo encoder, EncoderParameters encoderParams)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, encoder, encoderParams);
            return ms;
        }

        /// <summary>
        /// Saves the image as a JPEG file with the specified quality, creating the containing directory.
        /// </summary>
        /// 
        /// <param name="img">The image to save.</param>
        /// <param name="path">The path to the file.</param>
        /// <param name="quality">The quality of the image (0 to 100).</param>
        /// 
        /// <exception cref="System.ArgumentNullException">filename or encoder is null.</exception>
        /// <exception cref="System.Runtime.InteropServices.ExternalException">The image was saved with the wrong image format.-or- The image was saved to the same file it was created from.</exception>
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
        /// 
        /// <exception cref="System.ArgumentNullException">filename or format is null.</exception>
        /// <exception cref="System.Runtime.InteropServices.ExternalException">The image was saved with the wrong image format.-or- The image was saved to the same file it was created from.</exception>
        public static void SaveAsPng(this Image img, string path)
        {
            img.EzSave(path, ImageFormat.Png);
        }

        /// <summary>
        /// Saves the image as a BMP file, creating the containing directory.
        /// </summary>
        /// 
        /// <param name="img">The image to save.</param>
        /// <param name="path">The path to the file.</param>
        /// 
        /// <exception cref="System.ArgumentNullException">filename or format is null.</exception>
        /// <exception cref="System.Runtime.InteropServices.ExternalException">The image was saved with the wrong image format.-or- The image was saved to the same file it was created from.</exception>
        public static void SaveAsBmp(this Image img, string path)
        {
            img.EzSave(path, ImageFormat.Bmp);
        }

        /// <summary>
        /// Saves the image, creating the containing directory.
        /// </summary>
        /// 
        /// <param name="img">The image to save.</param>
        /// <param name="path">The path to the file.</param>
        /// 
        /// <exception cref="System.ArgumentNullException">filename is null.</exception>
        /// <exception cref="System.Runtime.InteropServices.ExternalException">The image was saved with the wrong image format.-or- The image was saved to the same file it was created from.</exception>
        public static void EzSave(this Image img, string path)
        {
            IOUtilities.CreateParentDirectory(path);
            img.Save(path);
        }

        /// <summary>
        /// Saves the image with the specified format, creating the containing directory.
        /// </summary>
        /// 
        /// <param name="img">The image to save.</param>
        /// <param name="path">The path to the file.</param>
        /// <param name="format">The format of the image.</param>
        /// 
        /// <exception cref="System.ArgumentNullException">filename or format is null.</exception>
        /// <exception cref="System.Runtime.InteropServices.ExternalException">The image was saved with the wrong image format.-or- The image was saved to the same file it was created from.</exception>
        public static void EzSave(this Image img, string path, ImageFormat format)
        {
            IOUtilities.CreateParentDirectory(path);
            img.Save(path, format);
        }

        /// <summary>
        /// Saves the image with the specified encoder and parameters, creating the containing directory.
        /// </summary>
        /// 
        /// <param name="img">The image to save.</param>
        /// <param name="path">The path to the file.</param>
        /// <param name="encoder">The encoder information for this image.</param>
        /// <param name="encoderParams">The encoder parameters for this image.</param>
        /// 
        /// <exception cref="System.ArgumentNullException">filename or encoder is null.</exception>
        /// <exception cref="System.Runtime.InteropServices.ExternalException">The image was saved with the wrong image format.-or- The image was saved to the same file it was created from.</exception>
        public static void EzSave(this Image img, string path, ImageCodecInfo encoder, EncoderParameters encoderParams)
        {
            IOUtilities.CreateParentDirectory(path);
            img.Save(path, encoder, encoderParams);
        }

        /// <summary>
        /// Gets an encoder parameter that specifies the quality of an image.
        /// </summary>
        /// 
        /// <param name="quality">The quality property of the codec, between 0 and 100.</param>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <see cref="quality"/> is not between 0 and 100.
        /// </exception>
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
        /// 
        /// <param name="format">The image format.</param>
        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
        }

        /// <summary>
        /// Gets a rectangle that surrounds this image.
        /// </summary>
        public static Rectangle GetBounds(this Image img)
        {
            return new Rectangle(0, 0, img.Width, img.Height);
        }
    }
}
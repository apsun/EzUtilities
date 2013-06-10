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

            /// <summary>
            /// Gets the path to the file.
            /// </summary>
            public string Path
            {
                get { return _path; }
            }

            public InvalidImageException()
                : this(null)
            {

            }

            public InvalidImageException(string path)
                : base("The file is not an image, is corrupted, or is too large")
            {
                _path = path;
            }
        }

        /// <summary>
        /// Creates an image from a file without locking the file.
        /// </summary>
        /// <param name="path">The path to the image file.</param>
        /// <returns></returns>
        public static Image FromFileNoLock(string path)
        {
            MemoryStream ms;
            return FromFileNoLock(path, out ms);
        }

        /// <summary>
        /// Creates an image from a file without locking the file.
        /// </summary>
        /// <param name="path">The path to the image file.</param>
        /// <param name="stream">The MemoryStream that contains data for the image.</param>
        /// <returns></returns>
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
        /// Loads an image from a file without locking the file. Note that this will 
        /// lose some data, notably the RawFormat property.
        /// </summary>
        /// <param name="path">The path to the image file.</param>
        /// <returns></returns>
        public static Image CloneFromFile(string path)
        {
            //tmp is locked, so we need to copy its pixels into a new image
            Image tmp;
            try
            {
                tmp = Image.FromFile(path);
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
            Image img = new Bitmap(tmp);
            tmp.Dispose();
            return img;
        }

        /// <summary>
        /// Loads an image from a stream without needing to keep the 
        /// stream open over the lifetime of the image. Note that this will 
        /// lose some data, notably the RawFormat property.
        /// </summary>
        /// <param name="stream">The stream that contains the data for this image.</param>
        /// <returns></returns>
        public static Image CloneFromStream(Stream stream)
        {
            //tmp has an internal dependency on the stream
            Image tmp = Image.FromStream(stream);
            Image img = new Bitmap(tmp);
            tmp.Dispose();
            return img;
        }

        /// <summary>
        /// Loads an image and splits it into individual frames.
        /// </summary>
        /// <param name="path">The path to the image.</param>
        public static Image[] LoadFrames(string path)
        {
            Image img;
            MemoryStream imgStream;
            try
            {
                img = FromFileNoLock(path, out imgStream);
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

            Image[] frames = img.ToFrames();

            //Not sure if we have to manually dispose the stream or not
            //But better safe than sorry, right?
            imgStream.Dispose();
            img.Dispose();

            return frames;
        }

        /// <summary>
        /// Loads an image and splits it into individual frames.
        /// </summary>
        /// <param name="path">The path to the image.</param>
        /// <param name="streams">The MemoryStreams that correspond to the images.</param>
        public static Image[] LoadFrames(string path, out MemoryStream[] streams)
        {
            Image img;
            MemoryStream imgStream;
            try
            {
                img = FromFileNoLock(path, out imgStream);
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

            Image[] frames = img.ToFrames(out streams);

            //Not sure if we have to manually dispose the stream or not
            //But better safe than sorry, right?
            imgStream.Dispose();
            img.Dispose();

            return frames;
        }

        /// <summary>
        /// Splits an image into individual frames.
        /// </summary>
        /// <param name="img">The image to split.</param>
        /// <returns></returns>
        public static Image[] ToFrames(this Image img)
        {
            MemoryStream[] streams;
            return img.ToFrames(out streams);
        }

        /// <summary>
        /// Splits an image into individual frames.
        /// </summary>
        /// <param name="img">The image to split.</param>
        /// <param name="streams">The MemoryStreams that correspond to the images.</param>
        /// <returns></returns>
        public static Image[] ToFrames(this Image img, out MemoryStream[] streams)
        {
            FrameDimension dimension = new FrameDimension(img.FrameDimensionsList[0]);
            int frameCount = img.GetFrameCount(dimension);

            Image[] frames = new Image[frameCount];
            streams = new MemoryStream[frameCount];
            for (int index = 0; index < frameCount; ++index)
            {
                img.SelectActiveFrame(dimension, index);
                MemoryStream ms = img.SaveToStream(ImageFormat.Bmp);
                Image imgFrame = Image.FromStream(ms);
                frames[index] = imgFrame;
                streams[index] = ms;
            }

            return frames;
        }

        /// <summary>
        /// Saves this image to a stream.
        /// </summary>
        /// <param name="img">The image to save.</param>
        /// <param name="format">The format of the image.</param>
        /// <returns></returns>
        public static MemoryStream SaveToStream(this Image img, ImageFormat format)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, format);
            return ms;
        }

        /// <summary>
        /// Saves this image to a stream.
        /// </summary>
        /// <param name="img">The image to save.</param>
        /// <param name="encoder">The encoder information for this image.</param>
        /// <param name="encoderParams">The encoder parameters for this image.</param>
        /// <returns></returns>
        public static MemoryStream SaveToStream(this Image img, ImageCodecInfo encoder, EncoderParameters encoderParams)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, encoder, encoderParams);
            return ms;
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
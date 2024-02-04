using System.Drawing.Imaging;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Pezeshkafzar_v2.Utilities
{
    public static class ImageExtentions
    {
        public async static Task UploadAsync(this IFormFile file, string path)
        {
            if (file != null && path != null) {
                using (Stream fileStream = new FileStream(path, FileMode.Create))
                {   
                    await file.CopyToAsync(fileStream);
                }
            }
        }

        public async static Task UploadsAsync(this IList<IFormFile> files, string path)
        {
            foreach (IFormFile file in files)
            {
                if (file != null && path != null)
                {
                    using (Stream fileStream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
            }
        }


        public const int ImageMinimumBytes = 512;
        public static bool IsImage(this IFormFile postedFile)
        {
            //-------------------------------------------
            //  Check the image mime types
            //-------------------------------------------
            if (postedFile.ContentType.ToLower() != "image/jpg" &&
                        postedFile.ContentType.ToLower() != "image/jpeg" &&
                        postedFile.ContentType.ToLower() != "image/pjpeg" &&
                        postedFile.ContentType.ToLower() != "image/gif" &&
                        postedFile.ContentType.ToLower() != "image/x-png" &&
                        postedFile.ContentType.ToLower() != "image/webp" &&
                        postedFile.ContentType.ToLower() != "video/x-matroska" &&
                        postedFile.ContentType.ToLower() != "image/png")
            {
                return false;
            }

            //-------------------------------------------
            //  Check the image extension
            //-------------------------------------------
            if (Path.GetExtension(postedFile.FileName).ToLower() != ".jpg"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".png"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".gif"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".webp"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".mkv"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".jpeg")
            {
                return false;
            }

            //-------------------------------------------
            //  Attempt to read the file and check the first bytes
            //-------------------------------------------
            try
            {
                using var fileStream = postedFile.OpenReadStream();
                if (!fileStream.CanRead)
                {
                    return false;
                }

                if (fileStream.Length < ImageMinimumBytes)
                {
                    return false;
                }

                byte[] buffer = new byte[512];
                fileStream.Read(buffer, 0, 512);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            //-------------------------------------------
            //  Try to instantiate new Bitmap, if .NET will throw exception
            //  we can assume that it's not a valid image
            //-------------------------------------------

            //try
            //{
            //    using var fileStream = postedFile.OpenReadStream();
            //    using (var bitmap = new System.Drawing.Bitmap(fileStream))
            //    {
            //    }
            //}
            //catch (Exception)
            //{
            //    return false;
            //}

            return true;
        }

        public class ImageResizer
        {
            /// <summary>
            /// http://www.blackbeltcoder.com/Articles/graph/programmatically-resizing-an-image
            /// Maximum width of resized image.
            /// </summary>
            public int MaxX { get; set; }

            /// <summary>
            /// Maximum height of resized image.
            /// </summary>
            public int MaxY { get; set; }

            /// <summary>
            /// If true, resized image is trimmed to exactly fit
            /// maximum width and height dimensions.
            /// </summary>
            public bool TrimImage { get; set; }

            /// <summary>
            /// Format used to save resized image.
            /// </summary>
            //public ImageFormat SaveFormat { get; set; }

            /// <summary>
            /// Constructor.
            /// </summary>
            public ImageResizer()
            {
                MaxX = MaxY = 330;
                TrimImage = false;
                //SaveFormat = ImageFormat.Jpeg;
            }

            public ImageResizer(int size)
            {
                MaxX = MaxY = size;
                TrimImage = false;
                //SaveFormat = ImageFormat.Jpeg;
            }



            /// <summary>
            /// Resizes the image from the source file according to the
            /// current settings and saves the result to the targe file.
            /// </summary>
            /// <param name="source">Path containing image to resize</param>
            /// <param name="target">Path to save resized image</param>
            /// <returns>True if successful, false otherwise.</returns>
            public bool Resize(string source, string target)
            {
                using (Image src = Image.FromFile(source, true))
                {
                    // Check that we have an image
                    if (src != null)
                    {
                        int origX, origY, newX, newY;
                        int trimX = 0, trimY = 0;

                        // Default to size of source image
                        newX = origX = src.Width;
                        newY = origY = src.Height;

                        // Does image exceed maximum dimensions?
                        if (origX > MaxX || origY > MaxY)
                        {
                            // Need to resize image
                            if (TrimImage)
                            {
                                // Trim to exactly fit maximum dimensions
                                double factor = Math.Max((double)MaxX / (double)origX,
                                    (double)MaxY / (double)origY);
                                newX = (int)Math.Ceiling((double)origX * factor);
                                newY = (int)Math.Ceiling((double)origY * factor);
                                trimX = newX - MaxX;
                                trimY = newY - MaxY;
                            }
                            else
                            {
                                // Resize (no trim) to keep within maximum dimensions
                                double factor = Math.Min((double)MaxX / (double)origX,
                                    (double)MaxY / (double)origY);
                                newX = (int)Math.Ceiling((double)origX * factor);
                                newY = (int)Math.Ceiling((double)origY * factor);
                            }
                        }

                        // Create destination image
                        using (Image dest = new Bitmap(newX - trimX, newY - trimY))
                        {
                            Graphics graph = Graphics.FromImage(dest);
                            graph.InterpolationMode =
                                System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            graph.DrawImage(src, -(trimX / 2), -(trimY / 2), newX, newY);
                            dest.Save(target, ImageFormat.Webp);
                            // Indicate success
                            return true;
                        }
                    }
                }
                // Indicate failure
                return false;
            }
        }
    }
}

using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp.Formats.Webp;
using System.IO;
using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace App.UI.Infrastructure
{
    // Photo size specification for flexible upload
    public struct PhotoSize
    {
        public int Width { get; }
        public int Height { get; }
        public string Suffix { get; }

        public PhotoSize(int width, int height, string suffix)
        {
            Width = width;
            Height = height;
            Suffix = suffix;
        }
    }

    public static class FileManager
    {
        private static string GenerateRandomSuffix()
        {
            return "_" + Guid.NewGuid().ToString("N").Substring(0, 8);
        }

        public static string UploadPhoto(IFormFile PhotoFile, string PhysicalPath, int Width, int Height)
        {
            if (PhotoFile == null || PhotoFile.Length == 0)
                return null;

            string baseName = Path.GetFileNameWithoutExtension(PhotoFile.FileName);
            string fileName = baseName + GenerateRandomSuffix() + ".webp";

            string targetFolder = Path.Combine(Directory.GetCurrentDirectory(), PhysicalPath);
            Directory.CreateDirectory(targetFolder);

            string fullPath = Path.Combine(targetFolder, fileName);

            using (var stream = PhotoFile.OpenReadStream())
            using (var image = Image.Load(stream))
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(Width, Height),
                    Mode = ResizeMode.Crop
                }));

                var encoder = new WebpEncoder
                {
                    Quality = 90,
                    FileFormat = WebpFileFormatType.Lossy
                };

                image.Save(fullPath, encoder);
            }

            return fileName;
        }

        // Flexible method - accepts any number of photo sizes
        public static string UploadMultiplePhotoSizes(IFormFile PhotoFile, string FolderName, params PhotoSize[] sizes)
        {
            if (PhotoFile == null || PhotoFile.Length == 0)
                return null;

            if (sizes == null || sizes.Length == 0)
                throw new ArgumentException("At least one photo size must be specified", nameof(sizes));

            string baseName = Path.GetFileNameWithoutExtension(PhotoFile.FileName);
            string randomSuffix = GenerateRandomSuffix();
            string baseFileName = baseName + randomSuffix; // Same base name for all sizes

            // Upload all requested sizes with the same base name
            foreach (var size in sizes)
            {
                UploadPhotoWithBaseName(PhotoFile, $"wwwroot/photos/{FolderName}", size.Width, size.Height, baseFileName, size.Suffix);
            }

            // Return the filename with the first suffix (typically _thumb)
            return baseFileName + sizes[0].Suffix + ".webp";
        }



        private static void UploadPhotoWithBaseName(IFormFile PhotoFile, string PhysicalPath, int Width, int Height, string BaseFileName, string Suffix)
        {
            if (PhotoFile == null || PhotoFile.Length == 0)
                return;

            string fileName = BaseFileName + Suffix + ".webp";

            string targetFolder = Path.Combine(Directory.GetCurrentDirectory(), PhysicalPath);
            Directory.CreateDirectory(targetFolder);

            string fullPath = Path.Combine(targetFolder, fileName);

            using (var stream = PhotoFile.OpenReadStream())
            using (var image = Image.Load(stream))
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(Width, Height),
                    Mode = ResizeMode.Crop
                }));

                var encoder = new WebpEncoder
                {
                    Quality = 90,
                    FileFormat = WebpFileFormatType.Lossy
                };

                image.Save(fullPath, encoder);
            }
        }

        public static string UploadPhoto(IFormFile PhotoFile, string PhysicalPath, int Width, int Height, string Suffix)
        {
            if (PhotoFile == null || PhotoFile.Length == 0)
                return null;

            string baseName = Path.GetFileNameWithoutExtension(PhotoFile.FileName);
            string fileName = baseName + GenerateRandomSuffix() + Suffix + ".webp";

            string targetFolder = Path.Combine(Directory.GetCurrentDirectory(), PhysicalPath);
            Directory.CreateDirectory(targetFolder);

            string fullPath = Path.Combine(targetFolder, fileName);

            using (var stream = PhotoFile.OpenReadStream())
            using (var image = Image.Load(stream))
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(Width, Height),
                    Mode = ResizeMode.Crop
                }));

                var encoder = new WebpEncoder
                {
                    Quality = 90,
                    FileFormat = WebpFileFormatType.Lossy
                };

                image.Save(fullPath, encoder);
            }

            return fileName;
        }


        public static string UploadFile(IFormFile File, string PhysicalPath)
        {
            if (File == null || File.Length == 0)
                return null;

            string baseName = Path.GetFileNameWithoutExtension(File.FileName);
            string extension = Path.GetExtension(File.FileName);
            string fileName = baseName + GenerateRandomSuffix() + extension;

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), PhysicalPath);
            Directory.CreateDirectory(filePath);

            string fullPath = Path.Combine(filePath, fileName);

            using (var stream = System.IO.File.Create(fullPath))
            {
                File.CopyTo(stream);
            }

            return fileName;
        }
    }
}

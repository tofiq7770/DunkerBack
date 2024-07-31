using Microsoft.AspNetCore.Http;

namespace Service.Helpers.Extentions
{
    public static class FileExtentions
    {
        public static bool ValidateSize(this IFormFile file, int mb)
        {

            return file.Length < mb * 1028 * 1028;
        }

        public static bool ValidateType(this IFormFile file, string type = "image")
        {
            return file.ContentType.Contains(type);

        }

        public static bool CheckImage(this IFormFile file)
        {
            return file.ValidateType() && file.ValidateSize(2);

        }
        public static async Task<string> CreateImageAsync(this IFormFile file, string path)
        {
            string filename = Guid.NewGuid().ToString() + file.FileName.Substring(file.FileName.LastIndexOf('.'));

            path = Path.Combine(path, filename);

            using (FileStream stream = new(path, FileMode.CreateNew))
            {
                await file.CopyToAsync(stream);
            }

            return filename;
        }

        public static void DeleteImage(this string imagePath, string path)
        {
            path = Path.Combine(path, imagePath);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        public static void DeleteFileFromLocalAsync(string path, string image)
        {
            if (File.Exists(Path.Combine(path, image)))
            {
                File.Delete(Path.Combine(path, image));
            }
        }
    }
}

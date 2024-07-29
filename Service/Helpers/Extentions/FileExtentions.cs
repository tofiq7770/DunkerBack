using Microsoft.AspNetCore.Http;

namespace Service.Helpers.Extentions
{
    public static class FileExtentions
    {
        public static bool CheckFileSize(this IFormFile file, int size)
        {
            return file.Length / 1024 < size;
        }
        public static bool CheckFileType(this IFormFile file, string pattern)
        {
            return file.ContentType.Contains(pattern);
        }
        public static async Task<string> CreateFileAsync(this IFormFile file, string root, params string[] folders)
        {
            string fileName = Guid.NewGuid().ToString() + file.FileName;
            string path = root;
            for (int i = 0; i < folders.Length; i++)
            {
                path = Path.Combine(path, folders[i]);
            }

            path = Path.Combine(path, fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;

        }
        public static async void DeleteFile(this string fileName, string root, params string[] folders)
        {
            string path = root;
            for (int i = 0; i < folders.Length; i++)
            {
                path = Path.Combine(path, folders[i]);
            }
            path = Path.Combine(path, fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        public async static Task SaveFileToLocalAsync(this IFormFile file, string path)
        {
            using FileStream stream = new(path, FileMode.Create);
            await file.CopyToAsync(stream);

        }
        public static void DeleteFileFromLocal(this string path)
        {
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

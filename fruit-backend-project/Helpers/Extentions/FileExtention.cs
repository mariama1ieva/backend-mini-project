namespace fruit_backend_project.Helpers.Extentions
{
    public static class FileExtention
    {
        public static bool CheckFileSize(this IFormFile file, int size)
        {
            return file.Length / 1024 < size;
        }

        public static bool CheckFileFormat(this IFormFile file, string pattern)
        {
            return file.ContentType.Contains(pattern);
        }

        public async static Task SaveFileToLocalAsync(this IFormFile file, string path)
        {
            using FileStream stream = new(path, FileMode.Create);

            await file.CopyToAsync(stream);
        }

        public static void DeleteFileFromLocalAsync(string path, string image)
        {
            if (File.Exists(Path.Combine(path, image)))
                File.Delete(Path.Combine(path, image));
        }
    }
}

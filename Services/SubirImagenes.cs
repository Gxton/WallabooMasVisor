namespace Wallaboo.Services
{
    public class SubirImagenes
    {
        private readonly IWebHostEnvironment _hosting;
        public string RootDirectory { get; set; } = @"files";

        public SubirImagenes(IWebHostEnvironment hosting)
        {
            _hosting = hosting;
        }
        public Task<string> Upload(IFormFile file, string name)
        {
            if (file == null) throw new ArgumentNullException();
            return Upload(new List<IFormFile> { file }, name);
        }

        public async Task<string> Upload(IList<IFormFile> files, string name)
        {
            var filesPath = string.Empty;
            var namefile = name;
            if (files.Count != 0)
            {
                var directory = @$"{RootDirectory}/{namefile}";
                var fullPath = Path.Combine(_hosting.WebRootPath, directory);
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
                var count = Directory.GetFiles(fullPath)?.Length;
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var extension = Path.GetExtension(file.FileName);
                        namefile = $"{name}-{count}";
                        var filePath = @$"{namefile}{extension}";
                        await using var stream =
                        File.Create(Path.Combine(fullPath, filePath));
                        filesPath += $"{directory}/{filePath};";
                        await file.CopyToAsync(stream);
                        count++;
                    }
                }

                filesPath = filesPath.Remove((filesPath.Length - 1));
            }

            return filesPath;
        }

    }
}

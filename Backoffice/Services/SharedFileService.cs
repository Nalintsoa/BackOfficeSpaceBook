namespace Backoffice.Services
{
    public class SharedFileService
    {
        private readonly string _sharedFilesPath;
        public SharedFileService(IConfiguration configuration) {
            _sharedFilesPath = Path.GetFullPath(configuration["SharedFilesPath"]);
            Directory.CreateDirectory(_sharedFilesPath);
        }

        public string GetSharedFilePath (string FileName) {
            return Path.Combine("SharedFiles", FileName);
        }

        public string GetSharedFilesDirectory()
        {
            return _sharedFilesPath;
        }
    }
}

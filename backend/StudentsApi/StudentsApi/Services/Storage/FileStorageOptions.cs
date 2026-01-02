namespace StudentsApi.Services.Storage
{
    public class FileStorageOptions
    {
        public string RootPath { get; set; } = null!;
        public long MaxAvatarBytes { get; set; } = 2 * 1024 * 1024; // 2MB
        public long MaxEnclosureBytes { get; set; } = 10 * 1024 * 1024; // 10MB
    }
}

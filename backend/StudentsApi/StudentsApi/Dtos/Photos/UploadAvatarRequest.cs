namespace StudentsApi.Dtos.Photos
{
    public class UploadAvatarRequest
    {
        public IFormFile File { get; set; } = null!;
    }
}

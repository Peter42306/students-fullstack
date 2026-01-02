namespace StudentsApi.Dtos.Photos
{
    public class UploadEnclosuresRequest
    {
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
    }
}

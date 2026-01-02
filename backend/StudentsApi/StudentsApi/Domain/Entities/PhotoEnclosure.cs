namespace StudentsApi.Domain.Entities
{
    public class PhotoEnclosure
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public string FileName { get; set; } = null!;
        public string StoredFileName { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public long Size { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}

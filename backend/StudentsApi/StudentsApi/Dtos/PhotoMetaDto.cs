namespace StudentsApi.Dtos
{
    public sealed record PhotoMetaDto(
        int Id,
        string FileName,
        string StoredFileName,
        string ContentType,
        long Size,
        DateTime UploadedAt);
}

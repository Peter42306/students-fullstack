namespace StudentsApi.Services.Storage
{
    public sealed record StoredFileResult(
        string StoredFileName,
        string RelativePath,
        long Size,
        string ContentType,
        string OriginalFileName);    
}

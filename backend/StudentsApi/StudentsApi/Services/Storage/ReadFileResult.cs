namespace StudentsApi.Services.Storage
{
    public sealed record ReadFileResult(
        Stream Stream,
        string ContentType,
        long? Size = null);    
}

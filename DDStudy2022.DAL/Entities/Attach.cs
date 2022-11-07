namespace DDStudy2022.DAL.Entities;

public class Attach
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? MimeType { get; set; }
    public string? FilePath { get; set; }
    public long Size { get; set; }
    public User? Author { get; set; }
}
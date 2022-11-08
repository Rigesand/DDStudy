namespace DDStudy2022.DAL.Entities;

public class Attach
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public long Size { get; set; }
    public UserAccount UserAccount { get; set; } = null!;
}
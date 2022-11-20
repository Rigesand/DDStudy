namespace DDStudy2022.Api.Models.Attaches;

public class AttachModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string MimeType { get; set; } = null!;
    public string FilePath { get; set; } = null!;
}
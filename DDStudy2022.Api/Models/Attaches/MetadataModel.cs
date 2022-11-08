namespace DDStudy2022.Api.Models.Attaches;

public class MetadataModel
{
    public Guid TempId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;
    public long Size { get; set; }
}
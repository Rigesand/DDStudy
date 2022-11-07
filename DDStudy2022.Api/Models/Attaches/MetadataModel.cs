namespace DDStudy2022.Api.Models.Attaches;

public class MetadataModel
{
    public Guid TempId { get; set; }
    public string? Name { get; set; }
    public string? MimeType { get; set; }
    public long Size { get; set; }
}
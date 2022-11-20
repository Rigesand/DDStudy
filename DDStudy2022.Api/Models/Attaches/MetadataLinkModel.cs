namespace DDStudy2022.Api.Models.Attaches;

public class MetadataLinkModel : MetadataModel
{
    public string FilePath { get; set; } = null!;
    public Guid AuthorId { get; set; }
}
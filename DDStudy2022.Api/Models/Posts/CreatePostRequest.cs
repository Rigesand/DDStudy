using DDStudy2022.Api.Models.Attaches;

namespace DDStudy2022.Api.Models.Posts;

public class CreatePostRequest
{
    public Guid? AuthorId { get; set; }
    public string? Description { get; set; }
    public List<MetadataModel> Contents { get; set; } = new List<MetadataModel>();

}
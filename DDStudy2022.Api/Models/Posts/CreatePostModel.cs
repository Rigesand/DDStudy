using DDStudy2022.Api.Models.Attaches;

namespace DDStudy2022.Api.Models.Posts;

public class CreatePostModel
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public Guid AuthorId { get; set; }
    public List<MetadataLinkModel> Contents { get; set; } = new List<MetadataLinkModel>();

}
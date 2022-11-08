using DDStudy2022.Api.Models.Attaches;

namespace DDStudy2022.Api.Models.Posts;

public class CreatePostModel
{
    public string Content { get; set; } = string.Empty;
    public ICollection<MetadataModel>? Photos { get; set; }
}
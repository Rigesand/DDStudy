using DDStudy2022.Api.Models.Photos;

namespace DDStudy2022.Api.Models.Posts;

public class GetPostModel
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public string Content { get; set; } = string.Empty;
    public ICollection<GetPhoto> Photos { get; set; } = null!;
}
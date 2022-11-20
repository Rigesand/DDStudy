using DDStudy2022.Api.Models.Attaches;
using DDStudy2022.Api.Models.Users;

namespace DDStudy2022.Api.Models.Posts;

public class PostModel
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public UserAvatarModel  Author { get; set; } = null!;
    public List<AttachExternalModel>? Contents { get; set; } = new List<AttachExternalModel>();
}
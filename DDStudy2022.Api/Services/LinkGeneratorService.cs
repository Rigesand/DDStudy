using DDStudy2022.Api.Models.Attaches;
using DDStudy2022.Api.Models.Users;
using DDStudy2022.DAL.Entities;

namespace DDStudy2022.Api.Services;

public class LinkGeneratorService
{
    public Func<PostContent, string?>? LinkContentGenerator;
    public Func<User, string?>? LinkAvatarGenerator;

    public void FixAvatar(User s, UserAvatarModel d)
    {
        d.AvatarLink = s.Avatar == null ? null : LinkAvatarGenerator?.Invoke(s);
    }

    public void FixContent(PostContent s, AttachExternalModel d)
    {
        d.ContentLink = LinkContentGenerator?.Invoke(s);
    }
}
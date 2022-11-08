using AutoMapper;
using AutoMapper.QueryableExtensions;
using DDStudy2022.Api.Interfaces;
using DDStudy2022.Api.Models.Attaches;
using DDStudy2022.Api.Models.Comments;
using DDStudy2022.Api.Models.Photos;
using DDStudy2022.Api.Models.Posts;
using DDStudy2022.Common.Exceptions;
using DDStudy2022.DAL;
using DDStudy2022.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DDStudy2022.Api.Services;

public class UserAccountService : IUserAccountService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly IAttachService _attachService;

    public UserAccountService(DataContext context, IMapper mapper, IAttachService attachService)
    {
        _context = context;
        _mapper = mapper;
        _attachService = attachService;
    }

    public async Task AddAvatarToUser(Guid userAccountId, MetadataModel meta, string filePath)
    {
        var userAccount = await _context.UserAccounts.Include(x => x.Avatar)
            .FirstOrDefaultAsync(x => x.Id == userAccountId);
        if (userAccount == null)
        {
            throw new UserException("User not found");
        }

        var avatar = new Avatar
        {
            UserAccount = userAccount,
            MimeType = meta.MimeType,
            FilePath = filePath,
            Name = meta.Name,
            Size = meta.Size
        };
        userAccount.Avatar = avatar;

        await _context.SaveChangesAsync();
    }

    public async Task CreatePost(CreatePostModel createPostModel, Guid userAccountId)
    {
        var userAccount = await _context.UserAccounts.Include(x => x.Posts)
            .FirstOrDefaultAsync(x => x.Id == userAccountId);
        if (userAccount == null)
        {
            throw new UserException("User not found");
        }

        var post = _mapper.Map<Post>(createPostModel);
        post.UserAccount = userAccount;
        if (createPostModel.Photos != null)
        {
            foreach (var photo in createPostModel.Photos)
            {
                var path = _attachService.GetPath(photo);
                var dbPhoto = new Photo
                {
                    UserAccount = userAccount,
                    MimeType = photo.MimeType,
                    FilePath = path,
                    Name = photo.Name,
                    Size = photo.Size,
                    Post = post
                };
                post.Photos!.Add(dbPhoto);
            }
        }

        userAccount.Posts.Add(post);

        await _context.SaveChangesAsync();
    }

    public async Task<AttachModel> GetUserAvatar(Guid userAccountId)
    {
        var avatar = await _context.Avatars.FirstOrDefaultAsync(it => it.UserAccountId == userAccountId);

        if (avatar == null)
        {
            throw new PostException("User not found");
        }

        var attachModel = _mapper.Map<AttachModel>(avatar);
        return attachModel;
    }

    public async Task<GetPostModel> GetPost(Guid postId)
    {
        var post = await _context.Posts.Include(it => it.Photos)
            .FirstOrDefaultAsync(it => it.Id == postId);

        if (post == null)
        {
            throw new PostException("Post not found");
        }

        var getPost = _mapper.Map<GetPostModel>(post);
        getPost.Photos = new List<GetPhoto>();
        foreach (var photo in post.Photos!)
        {
            getPost.Photos.Add(new GetPhoto
            {
                Id = photo.Id,
                MimeType = photo.MimeType,
                Url = $"/api/UserAccount/GetImage?photoId={photo.Id}"
            });
        }

        return getPost;
    }

    public async Task CreateComment(CreateCommentModel newComment, Guid userAccountId)
    {
        var post = await _context.Posts.Include(it => it.Comments)
            .FirstOrDefaultAsync(it => it.Id == newComment.PostId);

        if (post == null)
        {
            throw new PostException("Post not found");
        }

        var comment = _mapper.Map<Comment>(newComment);
        comment.UserAccountId = userAccountId;
        post.Comments!.Add(comment);

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<CommentModel>> GetAllComments(Guid postId)
    {
        return await _context.Comments.AsNoTracking().Where(it => it.PostId == postId)
            .ProjectTo<CommentModel>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }
}
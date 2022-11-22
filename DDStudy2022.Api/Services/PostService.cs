using AutoMapper;
using DDStudy2022.Api.Interfaces;
using DDStudy2022.Api.Models.Attaches;
using DDStudy2022.Api.Models.Posts;
using DDStudy2022.Common.Exceptions;
using DDStudy2022.DAL;
using DDStudy2022.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DDStudy2022.Api.Services;

public class PostService : IPostService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;

    public PostService(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task CreatePost(CreatePostRequest request)
    {
        var model = _mapper.Map<CreatePostModel>(request);

        model.Contents.ForEach(x =>
        {
            x.AuthorId = model.AuthorId;
            x.FilePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "attaches",
                x.TempId.ToString());

            var tempFi = new FileInfo(Path.Combine(Path.GetTempPath(), x.TempId.ToString()));
            if (tempFi.Exists)
            {
                var destFi = new FileInfo(x.FilePath);
                if (destFi.Directory != null && !destFi.Directory.Exists)
                    destFi.Directory.Create();

                File.Move(tempFi.FullName, x.FilePath, true);
            }
        });

        var dbModel = _mapper.Map<Post>(model);
        await _context.Posts.AddAsync(dbModel);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<PostModel>> GetPosts(int skip, int take)
    {
        var posts = await _context.Posts
            .Include(x => x.Author).ThenInclude(x => x.Avatar)
            .Include(x => x.PostContents).AsNoTracking().OrderByDescending(x => x.Created).Skip(skip).Take(take)
            .Include(x => x.Comments)
            .Include(x => x.Likes)
            .Select(x => _mapper.Map<PostModel>(x))
            .ToListAsync();

        return posts;
    }

    public async Task<PostModel> GetPostById(Guid id)
    {
        var post = await _context.Posts
            .Include(x => x.Author).ThenInclude(x => x.Avatar)
            .Include(x => x.PostContents).AsNoTracking()
            .Include(x => x.Comments)
            .Include(x => x.Likes)
            .Where(x => x.Id == id)
            .Select(x => _mapper.Map<PostModel>(x))
            .FirstOrDefaultAsync();
        if (post == null)
            throw new PostException("Post not Found");

        return post;
    }

    public async Task<AttachModel> GetPostContent(Guid postContentId)
    {
        var res = await _context.PostContents.FirstOrDefaultAsync(x => x.Id == postContentId);

        return _mapper.Map<AttachModel>(res);
    }

    public async Task DeletePost(Guid id, Guid userId)
    {
        var post = await _context.Posts.FirstOrDefaultAsync(it => it.Id == id && it.AuthorId == userId);
        if (post == null)
        {
            throw new PostException("Not found");
        }

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
    }
}
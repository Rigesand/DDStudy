using AutoMapper;
using DDStudy2022.Api.Interfaces;
using DDStudy2022.Api.Models.Likes;
using DDStudy2022.Common.Exceptions;
using DDStudy2022.DAL;
using DDStudy2022.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DDStudy2022.Api.Services;

public class LikeService : ILikeService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;

    public LikeService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task ChangeLikeFromPost(PostLikeRequest newLike)
    {
        var post = await _context.Posts
            .Include(it => it.Likes).AsNoTracking()
            .FirstOrDefaultAsync(it => it.Id == newLike.PostId);
        if (post == null)
        {
            throw new PostException("Post not found");
        }

        var like = _mapper.Map<PostLike>(newLike);
        var isExist = post.Likes!.FirstOrDefault(it => it.UserId == like.UserId);
        if (isExist != null)
        {
            _context.PostLikes.Remove(isExist);
            await _context.SaveChangesAsync();
            return;
        }

        await _context.PostLikes.AddAsync(like);
        await _context.SaveChangesAsync();
    }

    public async Task ChangeLikeFromComment(CommentLikeRequest newLike)
    {
        var comment = await _context.Comments
            .Include(it => it.Likes).AsNoTracking()
            .FirstOrDefaultAsync(it => it.Id == newLike.CommentId);
        if (comment == null)
        {
            throw new CommentException("Comment not found");
        }

        var like = _mapper.Map<CommentLike>(newLike);
        var isExist = comment.Likes!.FirstOrDefault(it => it.UserId == like.UserId);
        if (isExist != null)
        {
            _context.CommentLikes.Remove(isExist);
            await _context.SaveChangesAsync();
            return;
        }

        await _context.CommentLikes.AddAsync(like);
        await _context.SaveChangesAsync();
    }
}
using AutoMapper;
using DDStudy2022.Api.Interfaces;
using DDStudy2022.Api.Models.Comments;
using DDStudy2022.Common.Exceptions;
using DDStudy2022.DAL;
using DDStudy2022.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DDStudy2022.Api.Services;

public class CommentService : ICommentService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;

    public CommentService(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task CreateComment(CreateCommentModel newComment, Guid userId)
    {
        var post = await _context.Posts.FirstOrDefaultAsync(it => it.Id == newComment.PostId);

        if (post == null)
        {
            throw new PostException("Post not found");
        }

        var comment = _mapper.Map<Comment>(newComment);
        comment.UserId = userId;
        comment.Post = post;

        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteComment(Guid id, Guid userId)
    {
        var comment = await _context.Comments.FirstOrDefaultAsync(it => it.Id == id && it.UserId==userId);
        if (comment == null)
        {
            throw new CommentException("Not found");
        }

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
    }
}
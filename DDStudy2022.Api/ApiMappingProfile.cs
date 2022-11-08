using AutoMapper;
using DDStudy2022.Api.Models.Attaches;
using DDStudy2022.Api.Models.Comments;
using DDStudy2022.Api.Models.Posts;
using DDStudy2022.Api.Models.Users;
using DDStudy2022.Common.Services;
using DDStudy2022.DAL.Entities;

namespace DDStudy2022.Api;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        CreateMap<CreateUserModel, User>()
            .ForMember(d => d.Id, m => m.MapFrom(s => Guid.NewGuid()))
            .ForMember(d => d.PasswordHash, m => m.MapFrom(s => HashService.GetHash(s.Password)))
            .ForMember(d => d.BirthDate, m => m.MapFrom(s => s.BirthDate.UtcDateTime))
            .ForMember(d => d.UserAccountId, m => m.MapFrom(s => Guid.NewGuid()));
        CreateMap<User, UserModel>()
            .ForMember(d => d.AccountId, m => m.MapFrom(it => it.UserAccountId));
        CreateMap<Avatar, AttachModel>();
        CreateMap<Photo, AttachModel>().ReverseMap();
        CreateMap<CreatePostModel, Post>()
            .ForMember(d => d.Photos, m => m.Ignore())
            .ForMember(d => d.CreatedDate, m => m.MapFrom(s => DateTimeOffset.UtcNow));
        CreateMap<Post, GetPostModel>()
            .ForMember(d => d.Photos, m => m.Ignore())
            .ReverseMap();
        CreateMap<CommentModel, Comment>().ReverseMap();
        CreateMap<CreateCommentModel, Comment>()
            .ForMember(d => d.CreatedDate, m => m.MapFrom(s => DateTimeOffset.UtcNow));
    }
}
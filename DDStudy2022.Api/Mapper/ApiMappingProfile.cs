using AutoMapper;
using DDStudy2022.Api.Mapper.MapperActions;
using DDStudy2022.Api.Models.Attaches;
using DDStudy2022.Api.Models.Comments;
using DDStudy2022.Api.Models.Posts;
using DDStudy2022.Api.Models.Users;
using DDStudy2022.Common.Services;
using DDStudy2022.DAL.Entities;

namespace DDStudy2022.Api.Mapper;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        CreateMap<CreateUserModel, User>()
            .ForMember(d => d.Id, m => m.MapFrom(s => Guid.NewGuid()))
            .ForMember(d => d.PasswordHash, m => m.MapFrom(s => HashService.GetHash(s.Password)))
            .ForMember(d => d.BirthDay, m => m.MapFrom(s => s.BirthDate.UtcDateTime));
        CreateMap<User, UserModel>();
        CreateMap<User, UserAvatarModel>()
            .ForMember(d=>d.BirthDate, m=>m.MapFrom(s=>s.BirthDay))
            .ForMember(d=>d.PostsCount, m=>m.MapFrom(s=>s.Posts!.Count))
            .AfterMap<UserAvatarMapperAction>();

        CreateMap<Avatar, AttachModel>();
        CreateMap<Post, PostModel>()
            .ForMember(d=>d.Contents, m=>m.MapFrom(d=>d.PostContents));

        CreateMap<PostContent, AttachModel>();
        CreateMap<PostContent, AttachExternalModel>().AfterMap<PostContentMapperAction>();

        CreateMap<CreatePostRequest, CreatePostModel>();
        CreateMap<MetadataModel, MetadataLinkModel>();
        CreateMap<MetadataLinkModel, PostContent>();
        CreateMap<CreatePostModel, Post>()
            .ForMember(d => d.PostContents, m => m.MapFrom(s => s.Contents))
            .ForMember(d => d.Created, m => m.MapFrom(s => DateTime.UtcNow));
        
        CreateMap<CommentModel, Comment>().ReverseMap();
        CreateMap<CreateCommentModel, Comment>()
            .ForMember(d => d.CreatedDate, m => m.MapFrom(s => DateTimeOffset.UtcNow));
    }
}
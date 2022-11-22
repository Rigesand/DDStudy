using AutoMapper;
using DDStudy2022.Api.Mapper.MapperActions;
using DDStudy2022.Api.Models.Attaches;
using DDStudy2022.Api.Models.Comments;
using DDStudy2022.Api.Models.Likes;
using DDStudy2022.Api.Models.Posts;
using DDStudy2022.Api.Models.Subscriptions;
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
            .ForMember(d => d.BirthDate, m => m.MapFrom(s => s.BirthDay))
            .ForMember(d => d.PostsCount, m => m.MapFrom(s => s.Posts!.Count))
            .AfterMap<UserAvatarMapperAction>();

        CreateMap<Avatar, AttachModel>();
        CreateMap<Post, PostModel>()
            .ForMember(d => d.Contents, m => m.MapFrom(d => d.PostContents))
            .ForMember(d => d.CommentsCount, m => m.MapFrom(s => s.Comments!.Count))
            .ForMember(d => d.LikesCount, m => m.MapFrom(s => s.Likes!.Count));

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

        CreateMap<LikeRequest, Like>().ReverseMap();
        CreateMap<CommentLikeRequest, CommentLike>()
            .ForMember(d => d.Id, m => m.MapFrom(s => Guid.NewGuid()))
            .ReverseMap();
        CreateMap<PostLikeRequest, PostLike>()
            .ForMember(d => d.Id, m => m.MapFrom(s => Guid.NewGuid()))
            .ReverseMap();

        CreateMap<SubscriptionRequest, Subscription>()
            .ForMember(d => d.Id, m => m.MapFrom(s => Guid.NewGuid()));
    }
}
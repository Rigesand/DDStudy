using AutoMapper;
using DDStudy2022.Api.Models;
using DDStudy2022.Common.Services;
using DDStudy2022.DAL.Entities;

namespace DDStudy2022.Api;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        CreateMap<CreateUserModel, User>()
            .ForMember(d => d.Id, m => m.MapFrom(s => Guid.NewGuid()))
            .ForMember(d => d.PasswordHash, m => m.MapFrom(s => HashService.GetHash(s.Password!)))
            .ForMember(d => d.BirthDate, m => m.MapFrom(s => s.BirthDate.UtcDateTime));
        CreateMap<User, UserModel>();
    }
}
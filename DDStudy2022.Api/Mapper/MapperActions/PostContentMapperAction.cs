using AutoMapper;
using DDStudy2022.Api.Models.Attaches;
using DDStudy2022.Api.Services;
using DDStudy2022.DAL.Entities;

namespace DDStudy2022.Api.Mapper.MapperActions;

public class PostContentMapperAction : IMappingAction<PostContent, AttachExternalModel>
{
    private LinkGeneratorService _links;

    public PostContentMapperAction(LinkGeneratorService linkGeneratorService)
    {
        _links = linkGeneratorService;
    }

    public void Process(PostContent source, AttachExternalModel destination, ResolutionContext context)
        => _links.FixContent(source, destination);
}
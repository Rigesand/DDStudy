using DDStudy2022.Api.Interfaces;
using DDStudy2022.Api.Models.Attaches;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DDStudy2022.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class AttachController : ControllerBase
{
    private readonly IAttachService _attachService;

    public AttachController(IAttachService attachService)
    {
        _attachService = attachService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MetadataModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<List<MetadataModel>> UploadFiles([FromForm] List<IFormFile> files)
    {
        var res = new List<MetadataModel>();
        foreach (var file in files)
        {
            res.Add(await _attachService.UploadFile(file));
        }

        return res;
    }
}
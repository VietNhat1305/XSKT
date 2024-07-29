using Microsoft.AspNetCore.Mvc;
using XoSoKienThiet.Constants;
using XoSoKienThiet.Controllers.Core;
using XoSoKienThiet.Exceptions;
using XoSoKienThiet.Helpers;
using XoSoKienThiet.Installers;
using XoSoKienThiet.Interfaces.Common;
using XoSoKienThiet.Models.Core;

namespace XoSoKienThiet.Controllers.Common;
[ApiController]
//[Authorize]
[Route("api/v1/[controller]")]
public class CommonController : CommonRepositoryController<CommonModel, string>
{
    private DataContext _dataContext;
    private readonly ICommonService _service;
    public CommonController(ICommonService service) : base(service)
    {
        this._service = service;
    }
    [HttpGet]
    [Route("get-list")]
    public async Task<IActionResult> GetList()
    {
        try
        {
            var response = ListCommon.listCommon;
            return Ok(
                new ResultMessageResponse()
                    .WithData(response)
                    .WithCode(DefaultCode.SUCCESS)
                    .WithMessage(DefaultMessage.GET_DATA_SUCCESS)
            );
        }
        catch (ResponseMessageException ex)
        {
            return Ok(
                new ResultMessageResponse().WithCode(ex.ResultCode)
                    .WithMessage(ex.ResultString)
            );
        }
    }



}
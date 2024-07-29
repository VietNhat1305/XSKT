using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XoSoKienThiet.Constants;
using XoSoKienThiet.Controllers.Core;
using XoSoKienThiet.Exceptions;
using XoSoKienThiet.Helpers;
using XoSoKienThiet.Installers;
using XoSoKienThiet.Interfaces.NghiepVu;
using XoSoKienThiet.Models.NghiepVu;
using XoSoKienThiet.Models.PagingParam;

namespace XoSoKienThiet.Controllers.NghiepVu
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class LoaiAnhController : DefaultReposityController<LoaiAnh>
    {
        private readonly ILoaiAnhService _service;
        private readonly IThuVienAnhService _thuVienService;
        private DataContext _dataContext;
        private static string NameCollection = DefaultNameCollection.LOAIANH;
        public LoaiAnhController(DataContext context, ILoaiAnhService service, IThuVienAnhService thuVienService) : base(context, NameCollection)
        {
            _service = service;
            _dataContext = context;
            _thuVienService = thuVienService;
        }
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] LoaiAnh model)
        {
            try
            {
                var data = await _service.Create(model);
                return Ok(
                    new ResultMessageResponse()
                        .WithData(data)
                        .WithCode(DefaultCode.SUCCESS)
                        .WithMessage(DefaultMessage.CREATE_SUCCESS)

                );
               
            }
            catch (ResponseMessageException ex)
            {
                return Ok(
                    new ResultMessageResponse().WithCode(ex.ResultCode)
                        .WithMessage(ex.ResultString).WithDetail(ex.Error)
                );
            }
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] LoaiAnh model)
        {
            try
            {

                var data = await _service.Update(model);
                return Ok(
                    new ResultMessageResponse()
                        .WithData(data)
                        .WithCode(DefaultCode.SUCCESS)
                        .WithMessage(DefaultMessage.UPDATE_SUCCESS)
                );
            }
            catch (ResponseMessageException ex)
            {
                return Ok(
                    new ResultMessageResponse().WithCode(ex.ResultCode)
                        .WithMessage(ex.ResultString).WithDetail(ex.Error)
                );
            }
        }

        [HttpPost]
        [Route("get-paging-params")]
        public override async Task<IActionResult> GetPagingCore([FromBody] PagingParam param)
        {
            try
            {
                var response = await _service.GetPagingCore(param);
              
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
                       .WithMessage(ex.ResultString).WithDetail(ex.Error)
               );
            }
        }

    }
}
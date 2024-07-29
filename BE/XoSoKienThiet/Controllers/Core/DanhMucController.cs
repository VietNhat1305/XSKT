using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XoSoKienThiet.Constants;
using XoSoKienThiet.Exceptions;
using XoSoKienThiet.Helpers;
using XoSoKienThiet.Installers;
using XoSoKienThiet.Interfaces.Core;
using XoSoKienThiet.Models.Core;

namespace XoSoKienThiet.Controllers.Core
{
    [Route("api/v1/[controller]")]
     [Authorize]
    public class DanhMucController : DefaultReposityController<Menu>
    {
        private readonly IDanhMucService _service;
        private DataContext _dataContext;
        private static string NameCollection = DefaultNameCollection.DANH_MUC;
        public DanhMucController(DataContext context, IDanhMucService service) : base(context, NameCollection)
        {
            _service = service;
            _dataContext = context;
        }

        [HttpPost]
        [Route("create")]
      
        public async Task<IActionResult> Create([FromBody] Menu model)
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
  
        [Authorize]
        public async Task<IActionResult> Update([FromBody] Menu model)
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





        [HttpGet]
        [Route("getTreeList")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTreeList()
        {
            try
            {
                var response = await _service.GetTreeList();
                return Ok(new ResultMessageResponse()
                    .WithData(response)
                    .WithCode(DefaultCode.SUCCESS)
                    .WithMessage(DefaultMessage.GET_DATA_SUCCESS));
            }
            catch (ResponseMessageException ex)
            {
                return Ok(
                    new ResultMessageResponse().WithCode(ex.ResultCode)
                        .WithMessage(ex.ResultString).WithDetail(ex.Error)
                );
            }

        }



        [HttpGet]
        [Route("getTreeFlatten")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var response = await _service.GetTreeFlatten();
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
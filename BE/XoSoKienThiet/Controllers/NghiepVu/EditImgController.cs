using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpCompress.Compressors.Xz;
using System.IO;
using XoSoKienThiet.Exceptions;
using XoSoKienThiet.Helpers;
using XoSoKienThiet.Interfaces.Core;
using XoSoKienThiet.Interfaces.NghiepVu;
using XoSoKienThiet.Models.NghiepVu;
namespace XoSoKienThiet.Controllers.Core
{
    [Route("api/v1/[controller]")]
    //[Authorize]
    public class EditImgController : Controller
    {
        private readonly IEditImgService _service;
        public EditImgController(
             IEditImgService service
        )
        {
            _service = service;
        }
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] KetQuaXS model)
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
        public async Task<IActionResult> Update([FromBody] KetQuaXS model)
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
       
        //api du dinh tim ket qua xo so
        [HttpPost]
        [Route("get-model")]
        [AllowAnonymous]
        public async Task<IActionResult> GetModel([FromBody] string ketqua, DateTime date)
        {
            try
            {
                var data = await _service.GetModel(ketqua, date);

                return Ok(
                new ResultMessageResponse()
                    .WithData(data)
                    .WithCode(DefaultCode.SUCCESS)
                    .WithMessage(DefaultMessage.GET_DATA_SUCCESS));
                //return File(data, "application/octet-stream", "KetQuaXS.png");
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
        [Route("get-by-date")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByDate([FromBody] DateTime date)
        {
            try
            {
                var data = await _service.GetByDate(date);

                return Ok(
                new ResultMessageResponse()
                    .WithData(data)
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
    }
}
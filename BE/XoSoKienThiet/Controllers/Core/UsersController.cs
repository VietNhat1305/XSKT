using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XoSoKienThiet.Constants;
using XoSoKienThiet.Exceptions;
using XoSoKienThiet.Helpers;
using XoSoKienThiet.Installers;
using XoSoKienThiet.Interfaces.Core;
using XoSoKienThiet.Models.Core;
using XoSoKienThiet.Models.PagingParam;
using XoSoKienThiet.ViewModels;

namespace XoSoKienThiet.Controllers.Core
{
    [Route("api/v1/[controller]")]
   //  [Authorize]
    public class UsersController : DefaultReposityController<User>
    {
    private readonly IUserService _service;
    private DataContext _dataContext;
    private static string NameCollection = DefaultNameCollection.USERS;

        public UsersController(DataContext context, IUserService service) : base(context, NameCollection)
        {
            _service = service;
        }
        
        
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] User model)
        {
            try
            {
                var response = await _service.Create(model);
                return Ok(
                    new ResultMessageResponse()
                        .WithData(response)
                        .WithCode(DefaultCode.SUCCESS)
                        .WithMessage(DefaultMessage.CREATE_SUCCESS)
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

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] User model)
        {
            try
            {
                var response = await _service.Update(model);

                return Ok(
                    new ResultMessageResponse()
                        .WithData(response)
                        .WithCode(DefaultCode.SUCCESS)
                        .WithMessage(DefaultMessage.UPDATE_SUCCESS)
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
        
        
         
        [HttpPost]
        [Route("change-profile")]
        public async Task<IActionResult> ChangeProfile([FromBody] User model)
        {
            try
            {
                var data = await _service.ChangeProfile(model);

                return Ok(
                    new ResultMessageResponse()
                        .WithData(data)
                        .WithCode(DefaultCode.SUCCESS)
                        .WithMessage("Cập nhật thông tin thành công.")
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


        [HttpPost]
        [Route("get-paging-params-core")]
        public override  async Task<IActionResult> GetPagingCore([FromBody] PagingParam pagingParam)
        {
            try
            {
                var response = await _service.GetPaging(pagingParam);
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

        [HttpPost]
        [Route("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] UserVM model)
        {
            try
            {
                var data = await _service.ChangePassword(model);

                return Ok(
                    new ResultMessageResponse()
                        .WithData(data)
                        .WithCode(DefaultCode.SUCCESS)
                        .WithMessage("Đổi mật khẩu thành công.")
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
}
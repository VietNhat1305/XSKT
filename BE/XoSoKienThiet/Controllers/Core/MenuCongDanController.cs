using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XoSoKienThiet.Constants;
using XoSoKienThiet.Exceptions;
using XoSoKienThiet.FromBodyModels;
using XoSoKienThiet.Helpers;
using XoSoKienThiet.Installers;
using XoSoKienThiet.Interfaces.Core;
using XoSoKienThiet.Models.Core;

namespace XoSoKienThiet.Controllers.Core
{
    [Route("api/v1/[controller]")]
       [Authorize]
    public class MenuCongDanController : DefaultReposityController<MenuCongDan>
    {
        private readonly IMenuCongDanService _service;
        private DataContext _dataContext;
        private static string NameCollection = DefaultNameCollection.MENU_CONG_DAN;
        public MenuCongDanController(DataContext context, IMenuCongDanService service) : base(context, NameCollection)
        {
            _service = service;
            _dataContext = context;
        }
        
        [HttpPost]
        [Route("create")]
 
        public async Task<IActionResult> Create([FromBody] MenuCongDan model)
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
  
        public async Task<IActionResult> Update([FromBody] MenuCongDan model)
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
        [Route("getTreeListAD")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTreeListAD()
        {
            try
            {
                var response = await _service.GetTreeListAD();
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
        [Route("getTreeListTBV")]
        public async Task<IActionResult> GetTreeListTBV()
        {
            try
            {
                var response = await _service.GetTreeListTBV();
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
        [Route("getDanhSach")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDanhSach()
        {
            try
            {
                var response = await _service.GetDanhMuc();
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

        [HttpPost]
        [Route("deleted-with-user")]
        [AllowAnonymous]
        public  async Task<IActionResult> DeletedWithUser([FromBody] IdFromBodyModel model)
        {
            try
            {
                var data = await _service.DeletedWithUser(model);
                return Ok(
                    new ResultMessageResponse()
                        .WithData(data)
                        .WithCode(DefaultCode.SUCCESS)
                        .WithMessage(DefaultMessage.DELETE_SUCCESS)
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
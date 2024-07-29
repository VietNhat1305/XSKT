using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minio;
using XoSoKienThiet.Constants;
using XoSoKienThiet.Controllers.Core;
using XoSoKienThiet.Exceptions;
using XoSoKienThiet.Helpers;
using XoSoKienThiet.Installers;
using XoSoKienThiet.Interfaces.Core;
using XoSoKienThiet.Interfaces.NghiepVu;
using XoSoKienThiet.Models.NghiepVu;

namespace XoSoKienThiet.Controllers.NghiepVu
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class SliderController : DefaultReposityController<Slider>
    {
        private readonly ISliderService _service;
        private readonly IMinioClient _minio;
        private readonly IFileMinioService _fileMinioService;
        private DataContext _dataContext;
        private static string NameCollection = DefaultNameCollection.SLIDER;
        public SliderController
         (
            DataContext context,
             IMinioClient minio,
            IFileMinioService fileMinioService,
            ISliderService service) : base(context, NameCollection)
        {
            _fileMinioService = fileMinioService;
            _service = service;
            _dataContext = context;
            _minio = minio;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody]  Slider files)
        {
            try
            {

                var data = await _service.Create(files);
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
        
        
        
        
        [HttpGet]
        [Route("get-all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var response = await _service.GetAll();
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
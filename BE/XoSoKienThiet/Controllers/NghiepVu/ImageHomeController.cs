﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XoSoKienThiet.Constants;
using XoSoKienThiet.Controllers.Core;
using XoSoKienThiet.Exceptions;
using XoSoKienThiet.Helpers;
using XoSoKienThiet.Installers;
using XoSoKienThiet.Interfaces.NghiepVu;
using XoSoKienThiet.Models.NghiepVu;

namespace XoSoKienThiet.Controllers.NghiepVu
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class ImageHomeController : DefaultReposityController<ImageHome>
    {
        private readonly IImageHomeService _service;
        private DataContext _dataContext;
        private static string NameCollection = DefaultNameCollection.IMAGEHOME;
        public ImageHomeController(DataContext context, IImageHomeService service) : base(context, NameCollection)
        {
            _service = service;
            _dataContext = context;
        }
        [HttpPost]
        [Route("create")]
      
        public async Task<IActionResult> Create([FromBody] ImageHome file)
        {
            try
            {
                var data = await _service.Create(file);
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
        public async Task<IActionResult> Update([FromBody] ImageHome model)
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

        //[HttpGet]
        //[Route("get-all")]
        //public async Task<IActionResult> GetAll()
        //{
        //    try
        //    {
        //        var response = await _service.GetAll();
        //        return Ok(
        //            new ResultMessageResponse()
        //                .WithData(response)
        //                .WithCode(DefaultCode.SUCCESS)
        //                .WithMessage(DefaultMessage.GET_DATA_SUCCESS)
        //        );
        //    }
        //    catch (ResponseMessageException ex)
        //    {
        //        return Ok(
        //            new ResultMessageResponse().WithCode(ex.ResultCode)
        //                .WithMessage(ex.ResultString).WithDetail(ex.Error)
        //        );
        //    }
        //}

    }
}
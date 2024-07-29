using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using XoSoKienThiet.Exceptions;
using XoSoKienThiet.FromBodyModels;
using XoSoKienThiet.Helpers;
using XoSoKienThiet.Installers;
using XoSoKienThiet.Interfaces.Core;
using XoSoKienThiet.Models.PagingParam;
using XoSoKienThiet.Services.Core;

namespace XoSoKienThiet.Controllers.Core
{
    public abstract class DefaultReposityController<T> : ControllerBase where T : class
    {
        protected readonly IDefaultReposityService<T> Repository;
        public IMongoCollection<T> _collection;
        
        public DefaultReposityController(DataContext context ,  string collectionName)
        {
            _collection = context.Database.GetCollection<T>(collectionName);
            Repository = new DefaultReposityService<T>(_collection);
         //   Repository.getCollection(_collection);
        }
       
        
        [HttpGet]
        [Route("get-all-core")]
        [AllowAnonymous]
        public virtual async Task<IActionResult> GetAllData()
        {
            try
            {
                var response = await  Repository.GetAll();
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
        [AllowAnonymous]
        [Route("get-by-id-core")]
        public async Task<IActionResult> GetById([FromBody] IdFromBodyModel model)
        {
            try
            {
                var response = await  Repository.GetById(model);
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
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] IdFromBodyModel model)
        {
            try
            {
                await  Repository.Delete(model);
                return Ok(
                    new ResultMessageResponse()
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
        
                
        [HttpPost]
        [Route("deleted")]
        public async Task<IActionResult> Deleted([FromBody] IdFromBodyModel model)
        {
            try
            {
                await  Repository.Deleted(model);
                return Ok(
                    new ResultMessageResponse()
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
        
        
        
        [HttpPost]
        [Route("get-paging-params-core")]
        public virtual async Task<IActionResult> GetPagingCore([FromBody] PagingParam pagingParam)
        {
            try
            {
                var response = await  Repository.GetPagingCore(pagingParam);
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
}
using System.Net.Mime;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel.Args;
using XoSoKienThiet.Exceptions;
using XoSoKienThiet.Helpers;
using XoSoKienThiet.Interfaces.Core;
using XoSoKienThiet.Models.Appsettings;
using XoSoKienThiet.Models.Core;
namespace XoSoKienThiet.Controllers.Core
{
    [Route("api/v1/[controller]")]
    public class FilesMinioController : Controller
    {
        private readonly IMinioClient _minio;
        private readonly IFileMinioService _fileMinioService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        private MinioSetting _minioSetting;
        private MinioClient _minioClient;
        public FilesMinioController(
            IMinioClient minio,
             IWebHostEnvironment hostingEnvironment,
            IFileMinioService fileMinioService
        )
        {
            _minio = minio;
            _fileMinioService = fileMinioService;
            _hostingEnvironment = hostingEnvironment;

            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfiguration _configuration = builder.Build();
            _minioSetting = _configuration.GetSection("MinioSettings").Get<MinioSetting>();
            _minioClient = (MinioClient)new MinioClient()
                .WithEndpoint(_minioSetting.End_Point)
                .WithCredentials(_minioSetting.Access_Key, _minioSetting.Secret_Key)
                .WithSSL()
                .Build();
        }


        [HttpPost]
        [Route("~/api/v1/filesminio/uploadCK")]
        public async Task<IActionResult> UpdateCK(IFormFile upload)
        {
            try
            {
                var file = await _fileMinioService.UploadFileCK(upload);

                return Ok(
                    new {url=file}
                    
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

        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [DisableRequestSizeLimit]
        [HttpPost]
        [Route("~/api/v1/filesminio/upload")]
        public async Task<IActionResult> UploadFile()
        {
            try
            {
                var name = String.Empty;
                var files = HttpContext.Request.Form.Files.Count > 0 ? HttpContext.Request.Form.Files : null;
                IFormFile file = null;
                if (files != null && files.Count > 0)
                {
                    file = files[0];
                }

                if (file != null && file.Length > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    FileInfo fileInfo = new FileInfo(fileName);
                    var extFile = fileInfo.Extension;
                    if (fileName.Length > 100)
                    {
                        throw new Exception("Tên tệp tin quá dài.");
                    }
                    var currentDate = DateTime.UtcNow.ToLocalTime();
                    var subfolderName = $"{currentDate.Day}-{currentDate.Month}-{currentDate.Year}";
                    var path = $"{subfolderName}/";
                    name = Guid.NewGuid().ToString();
                    string fileExtension = Path.GetExtension(file.FileName);

                    var stream = file.OpenReadStream();
                    var request = new PutObjectArgs()
                        .WithBucket(_minioSetting.Bucket)
                        .WithObject(path + name + fileExtension)
                    .WithStreamData(stream)
                        .WithObjectSize(stream.Length)
                        .WithContentType(_minioSetting.Content_Type);
                    await _minioClient.PutObjectAsync(request);

                    var result =
                        await _fileMinioService.SaveFileAsyncMinio(file.FileName, name + fileExtension, path + name + fileExtension, path, file.Length, fileExtension, path);
                    return Ok( new ResultMessageResponse()
                                .WithData(result)
                                .WithCode(DefaultCode.SUCCESS)
                                .WithMessage(DefaultMessage.CREATE_SUCCESS));
                }
                return Ok(
                    new ResultMessageResponse().WithCode(DefaultCode.CREATE_FAILURE)
                        .WithMessage(DefaultMessage.CREATE_FAILURE)
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
        [Route("~/api/v1/filesminio/uploadIFormFile")]
        public async Task<IActionResult> Update(IFormFile files)
        {
            try
            {
                var file = await _fileMinioService.UploadIFormFile(files);

                return Ok(
                    new ResultMessageResponse()
                        .WithData(file)
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


        //[HttpPost]
        //[Route("~/api/v1/filesminio/uploadmulti")]
        //public async Task<IActionResult> UpdateMulti(List<IFormFile> files)
        //{
        //    try
        //    {
        //        var data = new List<FileShort>();
        //        foreach(var file in files)
        //        {
        //            var result = await _fileMinioService.UploadFile(file);
        //            data.Add(result);
        //        }         
        //        return Ok(
        //            new ResultMessageResponse()
        //                .WithData(data)
        //                .WithCode(DefaultCode.SUCCESS)
        //                .WithMessage(DefaultMessage.CREATE_SUCCESS)
        //        );
        //    }
        //    catch (ResponseMessageException ex)
        //    {
        //        return Ok(
        //            new ResultMessageResponse().WithCode(ex.ResultCode)
        //                .WithMessage(ex.ResultString)
        //        );
        //    }
        //}

        [HttpPost]
        [Route("~/api/v1/filesminio/delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {

                var key = await _fileMinioService.DeleteFile(id);

                return Ok(
                    new ResultMessageResponse()
                        .WithData(key)
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


        [HttpGet]
        [Route("~/api/v1/filesminio/view/{id}")]
        public async Task<IActionResult> ViewFile(string id)
        {

            try
            {
                var data = await  _fileMinioService.GetById(id);
                if (data == null)
                {
                    return Ok(new ResultMessageResponse().WithCode(DefaultCode.DATA_NOT_FOUND)
                        .WithMessage(DefaultMessage.DATA_NOT_FOUND));
                }
                MemoryStream memory = new MemoryStream();
                var memorystream = await _fileMinioService.Dowload(id);
                if (memorystream != null)
                {
                    return File(memorystream, "application/octet-stream", Path.GetFileName(data.FileName));
                }
                return Ok(new ResultMessageResponse().WithCode(DefaultCode.ERROR_STRUCTURE)
                        .WithMessage(DefaultMessage.ERROR_STRUCTURE + data.FileName + " | " + memorystream));
            }
            catch (ResponseMessageException ex)
            {
                return Ok(
                    new ResultMessageResponse().WithCode(ex.ResultCode)
                        .WithMessage(ex.ResultString)
                );
            }
        }

      


      
        [HttpGet]
        [Route("~/api/v1/filesminio/dowload/{id}")]
        public async Task<IActionResult> Dowload(string id)
        {
            try
            {

                var key = await _fileMinioService.Dowload(id);
               
                
                return Ok(
                    new ResultMessageResponse()
                        .WithData(key)
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
        [Route("~/api/v1/filesminio/UploadFileImage")]
        public async Task<IActionResult> UploadFileImage(IFormFile files)
        {
            try
            {

                var file = await _fileMinioService.UploadFileImage(files);

                return Ok(
                    new ResultMessageResponse()
                        .WithData(file)
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



    }
}
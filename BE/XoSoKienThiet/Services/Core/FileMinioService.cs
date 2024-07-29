using Microsoft.AspNetCore.Http;
using Minio;
using Minio.DataModel.Args;
using MongoDB.Driver;
using System.IO;
using MongoDB.Bson;
using XoSoKienThiet.Exceptions;
using XoSoKienThiet.Extensions;
using XoSoKienThiet.Helpers;
using XoSoKienThiet.Installers;
using XoSoKienThiet.Interfaces.Core;
using XoSoKienThiet.Models.Appsettings;
using XoSoKienThiet.Models.Core;
using XoSoKienThiet.Models.NghiepVu;
using File = XoSoKienThiet.Models.Core.FileModel;
using SharpCompress.Common;
using System.Xml.Linq;


namespace XoSoKienThiet.Services.Core
{
    public class FileMinioService : BaseService, IFileMinioService
    {

        private readonly IMinioClient _minio;
        private readonly IWebHostEnvironment _hostingEnvironment;
   
        
        private readonly ILoggingService _logger;

        private DataContext _context;
        private MinioClient _minioClient;
        private MinioSetting _minioSetting;
        private GetDomain _settings;
        private BaseMongoDb<File, string> BaseMongoDb;
        IMongoCollection<File> _collection;
        public FileMinioService(
            DataContext context,
            IWebHostEnvironment hostingEnvironment,
            ILoggingService logger,
            IMinioClient minio,
            IHttpContextAccessor contextAccessor) :
            base(context, contextAccessor)
        {
        
            _context = context;
            _minio = minio;
            _logger = logger;
            _collection = context.FILES;
            _hostingEnvironment = hostingEnvironment;
            BaseMongoDb = new BaseMongoDb<File, string>(_context.FILES);
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

            _settings = _configuration.GetSection("GetDomain").Get<GetDomain>();


        }

        public async Task<string> UploadFileCK(IFormFile file)
        {
            var name = String.Empty;
            try
            {
                if (file == null || (file != null && file.Length == 0))
                    throw new ResponseMessageException()
                         .WithCode(DefaultCode.DATA_EXISTED)
                        .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
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
                var result = await SaveFileAsyncMinio(file.FileName, name + fileExtension, path + name + fileExtension, path,  file.Length, fileExtension, path);

                 return $"{_settings.Domain}/api/v1/filesminio/view/{result.FileId}";
            }
            catch (Exception e)
            {
                throw new ResponseMessageException().WithException(DefaultCode.CREATE_FAILURE);
            }
        }

        public async Task<FileShort> UploadIFormFile(IFormFile file)
        {
            var name = String.Empty;
            try
            {
                if (file == null || (file != null && file.Length == 0))
                    throw new ResponseMessageException().WithCode(DefaultCode.DATA_EXISTED).WithMessage(DefaultMessage.DATA_NOT_EMPTY);
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

                var a = 0;
                var result = await SaveFileAsyncMinio(file.FileName, name + fileExtension, path + name + fileExtension, path, file.Length, fileExtension, path);

                return result;
            }
            catch (Exception e)
            {
                throw new ResponseMessageException().WithException(DefaultCode.CREATE_FAILURE);
            }
        }
        public async Task<FileShort> UploadImg(Stream stream, string id)
        {
            var name = String.Empty;
            try
            {
                var currentDate = DateTime.UtcNow.ToLocalTime();
                var subfolderName = $"{currentDate.Day}-{currentDate.Month}-{currentDate.Year}";
                var path = $"{subfolderName}/";
                name = Guid.NewGuid().ToString();
                string fileExtension = ".png";

                stream.Seek(0, SeekOrigin.Begin);
                var request = new PutObjectArgs()
                    .WithBucket(_minioSetting.Bucket)
                    .WithObject(path + name + fileExtension)
                    .WithStreamData(stream)
                    .WithObjectSize(stream.Length)
                    .WithContentType(_minioSetting.Content_Type);

                await _minioClient.PutObjectAsync(request);

                var result = await SaveFileAsyncMinio(id, name + fileExtension, path + name + fileExtension, path, stream.Length, fileExtension, path);

                return result;
            }
            catch (Exception e)
            {
                throw new ResponseMessageException().WithException(DefaultCode.CREATE_FAILURE);
            }
        }

        public async Task<string> DeleteFile(string id)
        {
            var key = String.Empty;
            try
            {
                var file = _context.FILES.Find(x => x.Id == id && !x.IsDeleted).FirstOrDefault();
                if (file == null)
                    throw new ResponseMessageException().WithCode(DefaultCode.DATA_NOT_FOUND)
                        .WithMessage(DefaultMessage.DATA_NOT_FOUND);

                var filePath = file.Path;
                key = file.SaveName;
                var request = new RemoveObjectArgs().WithBucket(_minioSetting.Bucket).WithObject(filePath);
                await _minioClient.RemoveObjectAsync(request);

                file.IsDeleted = true;
                var result = await BaseMongoDb.UpdateAsync(file);
            }
            catch (Exception e)
            {
                throw new ResponseMessageException().WithException(DefaultCode.DELETE_FAILURE);
            }
            return key;
        }

       
        //public async Task<string> DeleteFileAvatar(string id)
        //{
        //    var key = String.Empty;
        //    try
        //    {
        //        var file = _context.FILES.Find(x => x.Id == id && !x.IsDeleted).FirstOrDefault();
        //        if (file == null)
        //            throw new ResponseMessageException().WithCode(DefaultCode.DATA_NOT_FOUND)
        //                .WithMessage(DefaultMessage.DATA_NOT_FOUND);

        //        var filePath = file.Path;
        //        key = file.SaveName;
        //        var request = new RemoveObjectArgs().WithBucket(_minioSetting.Bucket).WithObject(filePath);
        //        await _minioClient.RemoveObjectAsync(request);

        //        file.IsDeleted = true;
        //        var result = await BaseMongoDb.UpdateAsync(file);

        //        var slide = _context.SLIDER.Find(x => x.Id == id && !x.IsDeleted).FirstOrDefault();

        //            slide.IsDeleted = true;
        //           //await BaseMongoDb.UpdateAsync(slide);
        //        var result1 = await BaseMongoDb.UpdateAsync(slide);

        //    }
        //    catch (Exception e)
        //    {
        //        throw new ResponseMessageException().WithException(DefaultCode.DELETE_FAILURE);
        //    }
        //    return key;
        //}
        public async Task<byte[]> Dowload(string id)
        {
            try
            {
                var file = _context.FILES.Find(x => x.Id == id && !x.IsDeleted).FirstOrDefault();
                if (file == null)
                    throw new ResponseMessageException().WithException(DefaultCode.EXCEPTION);

                var filePath = file.Path;
                MemoryStream memoryStream = new MemoryStream();
                var localFilePath = Path.Combine(file.Path);
                var pathFile = Path.Combine(_hostingEnvironment.ContentRootPath, localFilePath);

                var request = new GetObjectArgs()
                    .WithBucket(_minioSetting.Bucket)
                    .WithObject(filePath)
                    .WithCallbackStream((data) => data.CopyTo(memoryStream));
                var a =  await _minioClient.GetObjectAsync(request);
                // System.IO.File.WriteAllBytes($"{file.FileName}", memoryStream.ToArray());

                return memoryStream.ToArray();

            }
            catch (ResponseMessageException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }


        //public async Task<byte[]> Dowloadview(string id)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(id))
        //        {
        //            throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);
        //        }

        //        var file = _context.FILES.Find(x => x.Id == id && !x.IsDeleted).FirstOrDefault();
        //        if (file == null)
        //        {
        //            throw new ResponseMessageException().WithException(DefaultCode.DATA_NOT_FOUND);
        //        }

        //        var filePath = file.Path;
        //        MemoryStream memoryStream = new MemoryStream();

        //        var request = new GetObjectArgs()
        //            .WithBucket(_minioSetting.Bucket)
        //            .WithObject(filePath)
        //            .WithCallbackStream((stream) => {
        //                stream.CopyTo(memoryStream);
        //            });

        //        await _minioClient.GetObjectAsync(request);

        //        return memoryStream.ToArray();
        //    }
        //    catch (ResponseMessageException ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return null;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        return null;
        //    }
        //}

        public async Task<FileModel> GetById(string id)
        {
            return  _collection.Find(x => !x.IsDeleted && x.Id == id).FirstOrDefault();
        }
        public async Task<FileShort> SaveFileAsyncMinio(string fileName, string saveName, string path, string pathFolder, long fileSize, string fileExt, string foreignKey)
        {
            var entity = new File();
            entity.FileName = fileName;
            entity.SaveName = saveName;
            entity.Path = path;
            entity.PathFolder = pathFolder;
            entity.Size = fileSize;
            entity.Ext = fileExt;
            entity.CreatedAt = DateTime.Now;
            entity.IsDeleted = false;
            entity.Key= foreignKey;

            var result = await BaseMongoDb.CreateAsync(entity);

            if (result.Entity.Id == default || !result.Success)
                throw new ResponseMessageException().WithException(DefaultCode.CREATE_FAILURE);
            var fileshort = new FileShort();
            fileshort.FileId = entity.Id;
            fileshort.FileName = entity.FileName;
            fileshort.Ext = entity.Ext;
            fileshort.Path = entity.Path;
            return fileshort;
        }
        public async Task<File> SaveFileAsyncType(string filePath, string FileName, string newFileName,
         string fileExt, long fileSize, string pathFolder)
        {
            var entity = new File();
            entity.FileName = FileName;
            entity.SaveName = newFileName;
            entity.Path = filePath;
            entity.PathFolder = pathFolder;
            entity.Size = fileSize;
            entity.Ext = fileExt;
            entity.CreatedAt = DateTime.Now;
            entity.IsDeleted = false;
            
            //        await _collection.InsertOneAsync(entity);

            var result = await BaseMongoDb.CreateAsync(entity);

            if (result.Entity.Id == default || !result.Success)
                throw new ResponseMessageException().WithCode(DefaultCode.CREATE_FAILURE)
                    .WithMessage(DefaultMessage.CREATE_FAILURE);

            return entity;
        }
        public async Task<FileShort> UploadFileImage(IFormFile file)
        {
            var name = String.Empty;
            try
            {
                if (file == null || (file != null && file.Length == 0))
                    throw new ResponseMessageException()
                        .WithCode(DefaultCode.DATA_EXISTED)
                        .WithMessage(DefaultMessage.DATA_NOT_EMPTY);

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" }; 

               
                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    throw new ResponseMessageException()
                        .WithCode(DefaultCode.DATA_EXISTED)
                        .WithMessage("Only JPG, JPEG, PNG, and GIF files are allowed.");
                }

                var currentDate = DateTime.UtcNow.ToLocalTime();
                var subfolderName = $"{currentDate.Day}-{currentDate.Month}-{currentDate.Year}";
                var path = $"{subfolderName}/";
                name = Guid.NewGuid().ToString();

                var stream = file.OpenReadStream();

                var request = new PutObjectArgs()
                    .WithBucket(_minioSetting.Bucket)
                    .WithObject(path + name + fileExtension)
                    .WithStreamData(stream)
                    .WithObjectSize(stream.Length)
                    .WithContentType(GetContentType(fileExtension)); 

                await _minioClient.PutObjectAsync(request);

                var result = await SaveFileAsyncMinio(file.FileName, name + fileExtension, path + name + fileExtension, path, file.Length, fileExtension, path);
                return result;
            }
            catch (Exception e)
            {
                throw new ResponseMessageException().WithException(DefaultCode.CREATE_FAILURE);
            }
        }

        private string GetContentType(string fileExtension)
        {
           
            switch (fileExtension)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                default:
                    return "application/octet-stream";
            }
        }


    }
}
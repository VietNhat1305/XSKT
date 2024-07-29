using FluentValidation.Results;
using Minio;
using Minio.DataModel.Args;
using MongoDB.Bson;
using MongoDB.Driver;
using XoSoKienThiet.Exceptions;
using XoSoKienThiet.Extensions;
using XoSoKienThiet.Helpers;
using XoSoKienThiet.Installers;
using XoSoKienThiet.Interfaces.Core;
using XoSoKienThiet.Interfaces.NghiepVu;
using XoSoKienThiet.Models.Appsettings;
using XoSoKienThiet.Models.Core;
using XoSoKienThiet.Models.NghiepVu;
using XoSoKienThiet.Services.Core;

namespace XoSoKienThiet.Services.NghiepVu
{

    public class SliderService : BaseService, ISliderService
    {
        private DataContext _context;
        private BaseMongoDb<Slider, string> BaseMongoDb;
        private MinioClient _minioClient;
        private MinioSetting _minioSetting;
        private readonly IFileMinioService _fileMinioService;

        public SliderService(
            DataContext context,
             IFileMinioService fileMinioService,
            IHttpContextAccessor contextAccessor) :
            base(context, contextAccessor)
        {
            _context = context;
            _fileMinioService = fileMinioService;
            BaseMongoDb = new BaseMongoDb<Slider, string>(_context.SLIDER);

        }

        public async Task<dynamic> Create(Slider files)
        {
            try
            {
                if (files == default)
                    throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);
                ResultBaseMongo<Slider> result = null;
                foreach (var item in files.Files)
                {
                    var image = new Slider();
                    image.Id = BsonObjectId.GenerateNewId().ToString();
                    image.File = item;
                    image.Sort  = files.Sort;
                    result = await BaseMongoDb.CreateAsync(image);
                    if (result == null || result.Entity.Id == default || !result.Success)
                    {
                        throw new ResponseMessageException()
                            .WithException(DefaultCode.CREATE_FAILURE);
                    }
                }
               
             
            }
            catch (ResponseMessageException e)
            {
                throw new ResponseMessageException()
                    .WithCode(DefaultCode.EXCEPTION)
                    .WithMessage(e.ResultString).WithDetail(e.Error);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("is not a valid 24 digit hex string."))
                    throw new ResponseMessageException()
                        .WithException(DefaultCode.ID_NOT_CORRECT_FORMAT);

                throw new ResponseMessageException()
                    .WithCode(DefaultCode.EXCEPTION).
                    WithMessage(ex.Message);
            }

            return null;
        }

        public async Task<dynamic> GetAll()
        {
            var list = await _context.SLIDER.Find(x => !x.IsDeleted).ToListAsync();

            return list;
        }
    }
}
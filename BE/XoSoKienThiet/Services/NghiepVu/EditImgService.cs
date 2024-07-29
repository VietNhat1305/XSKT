using XoSoKienThiet.Exceptions;
using XoSoKienThiet.Helpers;
using XoSoKienThiet.Interfaces.NghiepVu;
using XoSoKienThiet.Services.Core;
using SixLabors.ImageSharp.Drawing.Processing;
using XoSoKienThiet.Models.NghiepVu;
using XoSoKienThiet.Extensions;
using XoSoKienThiet.Installers;
using XoSoKienThiet.Interfaces.Core;
using FluentValidation.Results;
using XoSoKienThiet.Models.Core;
using MongoDB.Driver;
using XoSoKienThiet.Constants;
using MongoDB.Bson;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats;
using System.Net.Mime;
using SixLabors.Fonts;
using Microsoft.AspNetCore.Components.RenderTree;
using Minio.DataModel;


namespace XoSoKienThiet.Services.NghiepVu
{

    public class EditImgService : BaseService, IEditImgService
    {
        private DataContext _context;
        private BaseMongoDb<KetQuaXS, string> BaseMongoDb;
        private readonly IFileMinioService _fileMinioService;
        public EditImgService(
           DataContext context,
           IHttpContextAccessor contextAccessor,
           IFileMinioService fileMinioService) :
           base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<KetQuaXS, string>(_context.KQXOSO);
            _fileMinioService = fileMinioService;
        }
        public async Task<dynamic> Create(KetQuaXS model)
        {
            try
            {
                if (model == default)
                    throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);

                //ValidationResult validationResult = new KQXoSoValidation().Validate(model);
                //if (!validationResult.IsValid)
                //    throw new ResponseMessageException().WithValidationResult(validationResult);
                var date = DateTime.Now;
                if (model.Date != null && model.Date > date)
                    throw new ResponseMessageException().WithMessage("Ngày xổ số không được lớn hơn hôm nay");

                var ketqua = new KetQuaXS()
                {
                    Id = BsonObjectId.GenerateNewId().ToString(),
                    KyVe = model.KyVe,
                    Date = model.Date,
                    Giai8 = model.Giai8,
                    Giai7 = model.Giai7,
                    Giai6 = model.Giai6,
                    Giai5 = model.Giai5,
                    Giai4 = model.Giai4,
                    Giai3 = model.Giai3,
                    Giai2 = model.Giai2,
                    Giai1 = model.Giai1,
                    GiaiDB = model.GiaiDB
                };

                //ketqua.CreatedBy = CurrentUser.UserName;

                var content = _context.KQXOSO.Find(x => !x.IsDeleted && x.DateShow == ketqua.DateShow).FirstOrDefault();
                if (content != null)
                    throw new ResponseMessageException().WithCode(DefaultCode.CREATE_FAILURE).WithMessage("Kết quả xổ số tại ngày " + ketqua.DateShow + " đã tồn tại, vui lòng tạo kết quả của ngày mới hoặc cập nhật lại ngày đã chọn");

                FileShort file = await Edit(ketqua);
                if (file != null)
                {
                    ketqua.File = file;
                }
                ResultBaseMongo<KetQuaXS> result;

                result = await BaseMongoDb.CreateAsync(ketqua);

                if (result.Entity.Id == default || !result.Success)
                    throw new ResponseMessageException().WithException(DefaultCode.CREATE_FAILURE);
                return ketqua;
            }
            catch (ResponseMessageException e)
            {
                throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage(e.ResultString).WithDetail(e.Error);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("is not a valid 24 digit hex string."))
                {
                    throw new ResponseMessageException().WithException(DefaultCode.ID_NOT_CORRECT_FORMAT);
                }

                throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage(ex.Message);
            }
        }
        public async Task<dynamic> Update(KetQuaXS model)
        {
            try
            {
                if (model == default)
                    throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);

                var content = _context.KQXOSO.Find(x => !x.IsDeleted && x.Id == model.Id).FirstOrDefault();
                if (content == default)
                    throw new ResponseMessageException().WithException(DefaultCode.DATA_NOT_FOUND);

                var date = DateTime.Now;
                if (model.Date != null && model.Date > date)
                    throw new ResponseMessageException().WithMessage("Ngày xổ số không được lớn hơn hôm nay");

                //ValidationResult validationResult = new KQXoSoValidation().Validate(model);
                //if (!validationResult.IsValid)
                //    throw new ResponseMessageException().WithValidationResult(validationResult);
                content.KyVe = model.KyVe;
                content.Date = model.Date;
                content.Giai8 = model.Giai8;
                content.Giai7 = model.Giai7;
                content.Giai6 = model.Giai6;
                content.Giai5 = model.Giai5;
                content.Giai4 = model.Giai4;
                content.Giai3 = model.Giai3;
                content.Giai2 = model.Giai2;
                content.Giai1 = model.Giai1;
                content.GiaiDB = model.GiaiDB;

                FileShort file = await Edit(content);
                if (file != null)
                {
                    content.File = file;
                }
                var result = await BaseMongoDb.UpdateAsync(content);
                if (!result.Success)
                    throw new ResponseMessageException().WithException(DefaultCode.UPDATE_FAILURE);
                return content;
            }
            catch (ResponseMessageException e)
            {
                throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage(e.ResultString).WithDetail(e.Error);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("is not a valid 24 digit hex string."))
                {
                    throw new ResponseMessageException().WithException(DefaultCode.ID_NOT_CORRECT_FORMAT);
                }

                throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage(ex.Message);
            }
        }

        public async Task<dynamic> Edit(KetQuaXS model)
        {
            try
            {
                // Đường dẫn của ảnh nguồn
                var sourceImagePath = "Files/KetQuaXS_9.jpg";

                using (var image = Image.Load(sourceImagePath))
                {
                    // Tạo một đối tượng Font
                    var fontBig = SystemFonts.CreateFont("Segoe UI Black", 90, FontStyle.Bold);
                    var font = SystemFonts.CreateFont("Segoe UI Black", 70, FontStyle.Bold);

                    // Màu sắc của văn bản
                    var textColorBlack = Color.Black;
                    var textColorRed = Color.Red;
                    var textColorYellow = Color.Yellow;
                    // Vị trí để vẽ văn bản               

                    var KyVe = new PointF(1060, 50);
                    var DateFormat = new PointF(1439, 50);
                    int x = 470;
                    // Thêm văn bản vào ảnh

                    DayOfWeek dayOfWeek = model.Date.Value.DayOfWeek;
                    string day = model.Date.Value.ToLocalTime().ToString(FormatTime.FORMAT_DATE_CORE);
                    image.Mutate(ctx => ctx.DrawText( model.KyVe, fontBig, textColorYellow, KyVe));
                    image.Mutate(ctx => ctx.DrawText(day, fontBig, textColorYellow, DateFormat));
                    if (model.Giai8 == null || model.Giai8.Equals(""))
                    {
                        var Giai8 = new PointF(Center("xx", fontBig, 1).Start, 233);
                        image.Mutate(ctx => ctx.DrawText("xx", fontBig, textColorRed, Giai8));
                    }
                    else
                    {
                        var Giai8 = new PointF(Center(model.Giai8, fontBig, 1).Start, 233);
                        image.Mutate(ctx => ctx.DrawText(model.Giai8, fontBig, textColorRed, Giai8));
                    }

                    if (model.Giai7 == null || model.Giai7.Equals(""))
                    {
                        var Giai7 = new PointF(Center("xxx", font, 1).Start, 455);
                        image.Mutate(ctx => ctx.DrawText("xxx", font, textColorBlack, Giai7));
                    }
                    else
                    {
                        var Giai7 = new PointF(Center(model.Giai7, font, 1).Start, 455);
                        image.Mutate(ctx => ctx.DrawText(model.Giai7, font, textColorBlack, Giai7));
                    }
                    int count = 0;
                    foreach (var item in model.Giai6)
                    {
                      
                        if (item.Equals(""))
                        {
                                KhoanCanh khoanCach = Center("xxxx", font, 3);
                                var Giai6 = new PointF(khoanCach.Start + (khoanCach.TextWidth * count) + (khoanCach.Step * count), 646);
                                image.Mutate(ctx => ctx.DrawText("xxxx", font, textColorBlack, Giai6));
                                count ++;
                        }
                        else
                        {
                                KhoanCanh khoanCach = Center(item, font, 3);
                                var Giai6 = new PointF(khoanCach.Start + (khoanCach.TextWidth * count) + (khoanCach.Step * count) , 646);
                                image.Mutate(ctx => ctx.DrawText(item, font, textColorBlack, Giai6));
                                count++;                            
                        }
                    }

                    if (model.Giai5 == null || model.Giai5.Equals(""))
                    {
                        var Giai5 = new PointF(Center("xxxx", font, 1).Start, 815);
                        image.Mutate(ctx => ctx.DrawText("xxxx", font, textColorBlack, Giai5));
                    }
                    else
                    {
                        var Giai5 = new PointF(Center(model.Giai5, font, 1).Start, 815);
                        image.Mutate(ctx => ctx.DrawText(model.Giai5, font, textColorBlack, Giai5));
                    }

                    count = 0;
                    foreach (var item in model.Giai4)
                    {
                        if(count < 4)
                        {
                            if (item.Equals(""))
                            {
                                KhoanCanh khoanCach = Center("xxxxx", font, 4);
                                var Giai4 = new PointF(khoanCach.Start + (khoanCach.TextWidth * count) + (khoanCach.Step * count), 1030);
                                image.Mutate(ctx => ctx.DrawText("xxxxx", font, textColorBlack, Giai4));
                                count++;
                            }
                            else
                            {
                                KhoanCanh khoanCach = Center(item, font, 4);
                                var Giai4 = new PointF(khoanCach.Start + (khoanCach.TextWidth * count) + (khoanCach.Step * count), 1030);
                                image.Mutate(ctx => ctx.DrawText(item, font, textColorBlack, Giai4));
                                count++;
                            }
                        }
                        else
                        {
                            if (item.Equals(""))
                            {
                                KhoanCanh khoanCach = Center("xxxxx", font, 3);
                                var Giai4 = new PointF(khoanCach.Start + (khoanCach.TextWidth * (count - 4)) + (khoanCach.Step * (count - 4)), 1190);
                                image.Mutate(ctx => ctx.DrawText("xxxxx", font, textColorBlack, Giai4));
                                count++;
                            }
                            else
                            {
                                KhoanCanh khoanCach = Center(item, font, 3);
                                var Giai4 = new PointF(khoanCach.Start + (khoanCach.TextWidth * (count - 4)) + (khoanCach.Step * (count - 4)), 1190);
                                image.Mutate(ctx => ctx.DrawText(item, font, textColorBlack, Giai4));
                                count++;
                            }
                        }
                        
                    }
                    count = 0;
                    foreach (var item in model.Giai3)
                    {
                        if (item.Equals(""))
                        {
                            KhoanCanh khoanCach = Center("xxxxx", font, 2);
                            var Giai3 = new PointF(khoanCach.Start + (khoanCach.TextWidth * count) + (khoanCach.Step * count), 1388);
                            image.Mutate(ctx => ctx.DrawText("xxxxx", font, textColorBlack, Giai3));
                            count++;
                        }
                        else
                        {
                            KhoanCanh khoanCach = Center(item, font, 2);
                            var Giai3 = new PointF(khoanCach.Start + (khoanCach.TextWidth * count) + (khoanCach.Step * count), 1388);
                            image.Mutate(ctx => ctx.DrawText(item, font, textColorBlack, Giai3));
                            count++;
                        }
                    }

                    if (model.Giai2 == null || model.Giai2.Equals(""))
                    {
                        var Giai2 = new PointF(Center("xxxxx", font, 1).Start, 1570);                       
                        image.Mutate(ctx => ctx.DrawText("xxxxx", font, textColorBlack, Giai2));
                    }
                    else
                    {
                        var Giai2 = new PointF(Center(model.Giai2, font, 1).Start, 1570);
                        image.Mutate(ctx => ctx.DrawText(model.Giai2, font, textColorBlack, Giai2));
                    }

                    if (model.Giai1 == null || model.Giai1.Equals(""))
                    {
                        var Giai1 = new PointF(Center("xxxxx", font, 1).Start, 1755);
                        image.Mutate(ctx => ctx.DrawText("xxxxx", font, textColorBlack, Giai1));
                    }
                    else
                    {
                        var Giai1 = new PointF(Center(model.Giai1, font, 1).Start, 1755);
                        image.Mutate(ctx => ctx.DrawText(model.Giai1, font, textColorBlack, Giai1));
                    }

                    if (model.GiaiDB == null || model.GiaiDB.Equals(""))
                    {
                        var GiaiDB = new PointF(Center("xxxxxx", fontBig, 1).Start, 1935);
                        image.Mutate(ctx => ctx.DrawText("xxxxxx", fontBig, textColorRed, GiaiDB));
                    }
                    else
                    {
                        var GiaiDB = new PointF(Center(model.GiaiDB, fontBig, 1).Start, 1935);
                        image.Mutate(ctx => ctx.DrawText(model.GiaiDB, fontBig, textColorRed, GiaiDB));
                    }

                    // Lưu ảnh đã được chỉnh sửa
                    MemoryStream stream = new MemoryStream();
                    IImageEncoder encoder = new JpegEncoder();
                    image.Save(stream, encoder);
                    string id = model.Id.ToString();
                    FileShort fileshort = await _fileMinioService.UploadImg(stream, "KQXS-" + day);
                    return fileshort;

                }
            }
            catch (ResponseMessageException e)
            {
                throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage(e.ResultString).WithDetail(e.Error);
            }
        }
        public KhoanCanh Center(string giatri, Font font, int count)
        {
            float textWidth = MeasureTextWidth(giatri, font) * count;
            float empty = 1562 - textWidth;
            float halfempty = empty / (count + 1);            
            float center = halfempty + 608;
            var KhoanCanh = new KhoanCanh();
            KhoanCanh.Start = center;
            KhoanCanh.Step = halfempty;
            KhoanCanh.TextWidth =textWidth/count;
            return KhoanCanh;
        }

        static float MeasureTextWidth(string text, Font font)
        {
            var rendererOptions = new TextOptions(font);
            return TextMeasurer.MeasureAdvance(text, rendererOptions).Width;
        }

        public async Task<dynamic> GetModel(string ketqua, DateTime date)
        {
            var builder = Builders<KetQuaXS>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Eq("IsDeleted", false));
            if ((!date.Equals("") || !date.Equals(null)) && (date != DateTime.MinValue))
            {             
                int dateshow = FormatDate.ConvertDatetimeQG(date);
                filter = builder.And(filter, builder.Regex("DateShow", dateshow.ToString()));
                if (!String.IsNullOrEmpty(ketqua))
                {
                    filter = builder.And(filter,
                        (builder.Regex("Giai8", ketqua)
                        |
                         builder.Regex("Giai7", ketqua) |
                         builder.Regex("Giai6", ketqua) |
                         builder.Regex("Giai5", ketqua) |
                         builder.Regex("Giai4", ketqua) |
                         builder.Regex("Giai3", ketqua) |
                         builder.Regex("Giai2", ketqua) |
                         builder.Regex("Giai1", ketqua) |
                         builder.Regex("GiaiDB", ketqua)
                        ));
                    //var list = await _context.KQXOSO.Find(filter).ToList().Select(x => new { GiaiTam = x.Giai8}).ToList();
                }
            }           
            var img = await _context.KQXOSO.Find(filter).ToListAsync();
            return img;
        }

        public async Task<dynamic> GetByDate(DateTime date)
        {
            try
            {
                if (date.Equals("") || date.Equals(null) || (date == DateTime.MinValue))
                {
                    var builder = Builders<KetQuaXS>.Filter;
                    var filter = builder.Empty;
                    filter = builder.And(filter, builder.Eq("IsDeleted", false));
                    var content = _context.KQXOSO.Find(filter).SortByDescending(x => x.DateShow).FirstOrDefault();
                    return content;
                }
                else
                {
                    //DateTime newdate = date.AddHours(8);
                    var builder = Builders<KetQuaXS>.Filter;
                    var filter = builder.Empty;
                    filter = builder.And(filter, builder.Eq("IsDeleted", false));

                    int dateshow = FormatDate.ConvertDatetimeQG(date);
                    filter = builder.And(filter, builder.Eq("DateShow", dateshow));

                    var model = await _context.KQXOSO.Find(filter).FirstOrDefaultAsync();
                    return model;
                }
                throw new ResponseMessageException().WithException(DefaultCode.DATA_EXISTED);
            }
            catch (ResponseMessageException ex) {
                throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage(ex.ResultString).WithDetail(ex.Error);
            }

        }
    }
}
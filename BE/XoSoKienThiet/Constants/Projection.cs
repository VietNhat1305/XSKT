using MongoDB.Driver;
using XoSoKienThiet.Models.Core;
using XoSoKienThiet.Models.NghiepVu;

namespace XoSoKienThiet.Constants;

public class Projection
{

    public static ProjectionDefinition<CommonModel> Projection_BasicCommon = Builders<CommonModel>.Projection
        .Include(x=>x.Id)
        .Include(x=>x.Code)
        .Include(x =>x.Name);



    public static ProjectionDefinition<ModelContent> Projection_Post = Builders<ModelContent>.Projection
        .Include(x => x.Id)
        .Include(x => x.MoTa)
        .Include(x => x.Name)
        .Include(x => x.FileImage);


}
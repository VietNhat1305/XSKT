using XoSoKienThiet.Installers;
using XoSoKienThiet.Interfaces.Common;
using XoSoKienThiet.Models.Core;
using XoSoKienThiet.Services.Core;

namespace XoSoKienThiet.Services.Common;

public class CommonService: CommmonRepository<CommonModel, string>, ICommonService
{
       
    public CommonService(DataContext context, IHttpContextAccessor contextAccessor) :
        base(context, contextAccessor)
    {
    }
    
        
}
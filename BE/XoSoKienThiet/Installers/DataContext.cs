using MongoDB.Driver;
using System.Runtime.Intrinsics.X86;
using XoSoKienThiet.Constants;
using XoSoKienThiet.Models.Auth;
using XoSoKienThiet.Models.Core;
using XoSoKienThiet.Models.NghiepVu;

namespace XoSoKienThiet.Installers
{
    public class DataContext
    {
      

        #region Auth 
        private readonly IMongoCollection<RefreshTokenModel> _refreshToken;
        private readonly IMongoCollection<User> _users;
        #endregion

        #region Common 

        private readonly IMongoCollection<CommonModel> _common;

        #endregion
        
        #region Core
        private readonly IMongoClient _mongoClient = null;
        private readonly IMongoDatabase _context = null;
        private readonly IMongoCollection<Menu> _menu;
        
        private readonly IMongoCollection<MenuCongDan> _menuCongDan;
        private readonly IMongoCollection<Menu> _danhMuc;
            
        private readonly IMongoCollection<DonVi> _donVi;
        private readonly IMongoCollection<LoggingModel> _logging;
        
     
        private readonly IMongoCollection<UnitRole> _unitRole;
        
        
        private readonly IMongoCollection<FileModel> _file;
        


        private readonly Dictionary<string,  IMongoCollection<CommonModel>> _listCommonCollection;



        #endregion

        #region Nghiep vu
        private readonly IMongoCollection<LienHeModel> _lienHe;
        private readonly IMongoCollection<ModelContent> _modelContent;
        private readonly IMongoCollection<Header> _header; 
        private readonly IMongoCollection<Slider> _slider;
        private readonly IMongoCollection<SuKien> _suKien;
        private readonly IMongoCollection<LinkModel> _link;
        private readonly IMongoCollection<ThuVienAnh> _thuVienAnh;
        private readonly IMongoCollection<LinkYtb> _linkYtb;
        private readonly IMongoCollection<LienKet> _lienKet;
        private readonly IMongoCollection<ImageHome> _imageHome;
        private readonly IMongoCollection<DiemLoTo> _diemLoTo;
        private readonly IMongoCollection<DiemBanLoTo> _diemBanLoTo;

        private readonly IMongoCollection<KetQuaXS> _ketQuaXOSO;
        private readonly IMongoCollection<BienBan> _bienBan;
        private readonly IMongoCollection<modelContentNoiBo> _modelContentNoiBo;
        private readonly IMongoCollection<LoaiAnh> _loaiAnh;


        #endregion



        public DataContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.
               GetValue<string>(ConfigurationDb.MONGO_CONNECTION_STRING));
            if (client != null)
            {
                #region Core
                _context = client.GetDatabase(configuration.GetValue<string>(ConfigurationDb.MONGO_DATABASE_NAME));
                _users = _context.GetCollection<User>(DefaultNameCollection.USERS);
                _refreshToken = _context.GetCollection<RefreshTokenModel>(DefaultNameCollection.REFRESHTOKEN);                
                _menu = _context.GetCollection<Menu>(DefaultNameCollection.MENU);
                _menuCongDan = _context.GetCollection<MenuCongDan>(DefaultNameCollection.MENU_CONG_DAN);
                _danhMuc = _context.GetCollection<Menu>(DefaultNameCollection.DANH_MUC);
                _unitRole =_context.GetCollection<UnitRole>(DefaultNameCollection.UNIT_ROLE);
                _donVi = _context.GetCollection<DonVi>(DefaultNameCollection.DONVI);
                _file = _context.GetCollection<FileModel>(DefaultNameCollection.FILES);
                _listCommonCollection = new Dictionary<string,  IMongoCollection<CommonModel>>();
                foreach ( ItemCommon item in ListCommon.listCommon)
                {
                    IMongoCollection<CommonModel> colection = Database.GetCollection<CommonModel>(item.Code);
                    _listCommonCollection.Add(item.Code, colection);
                }

                #endregion

                #region NghiepVu
                _logging = _context.GetCollection<LoggingModel>(DefaultNameCollection.LOGGING);
                _lienHe = _context.GetCollection<LienHeModel>(DefaultNameCollection.LIENHE);
                _modelContent = _context.GetCollection<ModelContent>(DefaultNameCollection.MODELCONTENT);            
                _slider = _context.GetCollection<Slider>(DefaultNameCollection.SLIDER);
                _header = _context.GetCollection<Header>(DefaultNameCollection.HEADER);
                _suKien = _context.GetCollection<SuKien>(DefaultNameCollection.SUKIEN);
                _link = _context.GetCollection<LinkModel>(DefaultNameCollection.LINK);

                _thuVienAnh = _context.GetCollection<ThuVienAnh>(DefaultNameCollection.THUVIENANH);
               _linkYtb = _context.GetCollection<LinkYtb>(DefaultNameCollection.LINKYTB);
               _lienKet = _context.GetCollection<LienKet>(DefaultNameCollection.LIENKET);
               _diemLoTo = _context.GetCollection<DiemLoTo>(DefaultNameCollection.DIEMLOTO);
                _diemBanLoTo = _context.GetCollection<DiemBanLoTo>(DefaultNameCollection.DIEMBANLOTO);
                _imageHome = _context.GetCollection<ImageHome>(DefaultNameCollection.IMAGEHOME);

                _ketQuaXOSO = _context.GetCollection<KetQuaXS>(DefaultNameCollection.KQXOSO);
                _bienBan = _context.GetCollection<BienBan>(DefaultNameCollection.BIENBAN);
                _modelContentNoiBo = _context.GetCollection<modelContentNoiBo>(DefaultNameCollection.MODELCONTENTNOIBO);
                _loaiAnh = _context.GetCollection<LoaiAnh>(DefaultNameCollection.LOAIANH);

                #endregion

            }
        }


        #region Core

              
        public IMongoDatabase Database
        {
            get { return _context; }
        }
        public IMongoClient Client
        {
            get { return _mongoClient; }
        }

        public IMongoCollection<RefreshTokenModel> REFRESHTOKEN { get => _refreshToken; }
        
        
        public IMongoCollection<UnitRole> UNIT_ROLE { get => _unitRole; }

        
        public IMongoCollection<DonVi> DONVI { get => _donVi; }
       

        public IMongoCollection<User> USERS { get => _users; }
        
        public IMongoCollection<LoggingModel> LOGGING { get => _logging; }
        
       
        
        
        

        public IMongoCollection<Menu> MENU { get => _menu; }
        
        public IMongoCollection<MenuCongDan> MENUCONGDAN { get => _menuCongDan; }
        public IMongoCollection<Menu> DANHMUC { get => _danhMuc; }
        
        public IMongoCollection<FileModel> FILES { get => _file; }
        
        private Dictionary<string,  IMongoCollection<CommonModel>> CommonCollection { get => _listCommonCollection; }
        public  IMongoCollection<CommonModel> GetCategoryCollectionAs(string collectionName)
        {
            return  CommonCollection[collectionName];
        }

        #endregion

        #region NghiepVu

        public IMongoCollection<LienHeModel> LIENHE { get => _lienHe; }
        public IMongoCollection<ModelContent> MODELCONTENT { get => _modelContent; }
        
        public IMongoCollection<modelContentNoiBo> MODELCONTENTNOIBO { get => _modelContentNoiBo; }
      
        
        public IMongoCollection<Header> HEADER { get => _header; }
        
        public IMongoCollection<Slider> SLIDER { get => _slider; }
        public IMongoCollection<SuKien> SUKIEN { get => _suKien; }
        public IMongoCollection<LinkModel> LINK { get => _link; }
        public IMongoCollection<ThuVienAnh> THUVIENANH { get => _thuVienAnh; }
        public IMongoCollection<LinkYtb> LINKYTB { get => _linkYtb; }
        public IMongoCollection<LienKet> LIENKET { get => _lienKet; }
        public IMongoCollection<DiemLoTo> DIEMLOTO { get => _diemLoTo; }
        public IMongoCollection<DiemBanLoTo> DIEMBANLOTO { get => _diemBanLoTo; }
        public IMongoCollection<ImageHome> IMAGEHOME { get => _imageHome; }


        public IMongoCollection<KetQuaXS> KQXOSO { get => _ketQuaXOSO; }
        public IMongoCollection<BienBan> BIENBAN { get => _bienBan; }
        
        public IMongoCollection<LoaiAnh> LOAIANH { get => _loaiAnh; }
        #endregion






    }


}
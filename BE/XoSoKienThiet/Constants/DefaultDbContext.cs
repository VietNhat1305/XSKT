namespace XoSoKienThiet.Constants
{
    public static class DefaultNameCollection
    {
        public const string USERS = "CR_USERS";
        public const string REFRESHTOKEN = "CR_REFRESHTOKEN";
        
        public const string DONVI = "CR_DONVI";
        public const string DIAGIOIHANHCHINH = "CR_DIAGIOIHANHCHINH";
        public const string LOGGING = "CR_LOGGING";
        public const string MENU = "CR_MENU";
        public const string MENU_CONG_DAN = "CR_MENU_CONG_DAN";
        public const string DANH_MUC = "CR_DANH_MUC";
        public const string MODULE = "CR_MODULE";
        public const string UNIT_ROLE = "CR_UNIT_ROLE";

        public const string FILES = "CR_FILES";
        
        
        #region nghiepvu
        public const string LIENHE = "NV_LIENHE";  
        public const string HEADER= "NV_HEADER";
        public const string SLIDER= "NV_SLIDER";
        public const string MODELCONTENT = "NV_MODELCONTENT";
        public const string MODELTABLE = "NV_MODELTABLE";
        public const string SUKIEN = "NV_SUKIEN";
        public const string LINK = "NV_LINK";
        public const string THUVIENANH = "NV_THUVIENANH";
        public const string LINKYTB = "NV_LINKYTB";
        public const string LIENKET = "NV_LIENKET";
        public const string IMAGEHOME = "NV_IMAGEHOME";
        public const string DIEMLOTO = "NV_DIEMLOTO";
        public const string BIENBAN = "NV_BIENBAN";
        public const string DIEMBANLOTO = "NV_DIEMBANLOTO";
        public const string LOAIANH = "NV_LOAIANH";


        public const string KQXOSO = "NV_KQXOSO";
        
        public const string MODELCONTENTNOIBO = "NV_MODELCONTENTNOIBO";
        

        #endregion



    }

    public static class ConfigurationDb
    {
        #region MONGODB 
        public const string MONGO_CONNECTION_STRING = "DbSettings:ConnectionString";
        public const string MONGO_DATABASE_NAME = "DbSettings:DatabaseName";
        public const string MONGO_DATABASE_DOMAIN= "GetDomain:Domain";
        #endregion

        #region POSTGRE
        public const string POSTGRE_CONNECTION = "DbSettings:PostgreConnection";
        #endregion                                                                                                      
    }
}
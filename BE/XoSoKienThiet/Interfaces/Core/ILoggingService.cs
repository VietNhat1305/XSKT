namespace XoSoKienThiet.Interfaces.Core
{
    public interface ILoggingService
    {
       
        
        Task<dynamic> Create(string content , string code, string casename);
    }
}
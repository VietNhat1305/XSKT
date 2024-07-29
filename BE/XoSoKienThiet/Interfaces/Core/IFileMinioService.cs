using XoSoKienThiet.Models.Core;

namespace XoSoKienThiet.Interfaces.Core
{
    public interface IFileMinioService
    {
        public Task<string> UploadFileCK(IFormFile file);
        public Task<FileShort> UploadIFormFile(IFormFile file);

        public Task<FileShort> UploadImg(Stream stream, string id);
        public Task<FileShort> UploadFileImage(IFormFile file);
        public Task<string> DeleteFile(string id);
       
        public Task<byte[]> Dowload(string id);
        //public Task<byte[]> Dowloadview(string id);
        public Task<FileModel> GetById(string id);

        public Task<FileShort> SaveFileAsyncMinio(string fileName, string saveName, string path, string pathFolder, long fileSize, string fileExt, string foreignKey);

    }
}
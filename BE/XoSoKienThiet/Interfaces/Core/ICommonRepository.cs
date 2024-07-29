using XoSoKienThiet.FromBodyModels;
using XoSoKienThiet.Models.PagingParam;

namespace XoSoKienThiet.Interfaces.Core;

public interface ICommonRepository<TEntity, UEntityId>
{
    Task<long> CountAsync();
    Task<TEntity> GetByIdAsync(IdFromBodyCommonModel model);
    Task<TEntity> CreateAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity model);
    Task DeleteAsync(TEntity model);

    Task<dynamic> GetAsync(string collectionName);
    Task<dynamic> GetPagingAsync(CommonPaging param);
}
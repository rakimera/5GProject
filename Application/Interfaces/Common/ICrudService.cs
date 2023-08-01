using Application.DataObjects;

namespace Application.Interfaces.Common;

public interface ICrudService<T>
{
    BaseResponse<IEnumerable<T>> GetAll();
    Task<BaseResponse<string>> CreateAsync(T model);
    Task<BaseResponse<T>> GetByOid(string oid);
    Task<BaseResponse<bool>> Delete(string oid);
}
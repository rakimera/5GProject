using Application.DataObjects;

namespace Application.Interfaces;

public interface IWordService
{
    public Task<BaseResponse<bool>> GetLoadXlsx();
    public Task<BaseResponse<bool>> ReadExcel();
    public Task<BaseResponse<bool>> ProjectWord();

}
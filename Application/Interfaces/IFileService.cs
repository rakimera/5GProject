using Application.DataObjects;

namespace Application.Interfaces;

public interface IFileService
{
    public Task<BaseResponse<bool>> GetLoadXlsx();
    public Task<BaseResponse<bool>> ReadExcel();
    public Task<BaseResponse<bool>> ProjectWord();
    public Task<BaseResponse<bool>> CreateGrafic();


}
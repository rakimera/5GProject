using Application.DataObjects;
using DevExpress.XtraRichEdit.API.Native;
using Domain.Entities;

namespace Application.Interfaces;

public interface IFileService
{
    public Task<BaseResponse<bool>> GetLoadXlsx();
    public Task<BaseResponse<bool>> ReadExcel();
    public Task<BaseResponse<bool>> ProjectWord();
    public Task<BaseResponse<bool>> CreateGrafic(Document document,DocumentPosition position);


}
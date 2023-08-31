using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models.ExecutiveCompany;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;

namespace Application.Interfaces;

public interface IExecutiveCompanyService : ICrudService<ExecutiveCompanyDto>
{
    Task<LoadResult> GetLoadResult(DataSourceLoadOptionsBase loadOptions);
    Task<BaseResponse<ExecutiveCompanyDto>> UpdateExecutiveCompany(UpdateExecutiveCompanyDto model, string editor);
}
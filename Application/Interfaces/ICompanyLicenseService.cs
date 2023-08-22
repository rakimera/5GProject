using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models.CompanyLicense;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;

namespace Application.Interfaces;

public interface ICompanyLicenseService : ICrudService<CompanyLicenseDto>
{
    Task<LoadResult> GetLoadResult(DataSourceLoadOptionsBase loadOptions);
    Task<BaseResponse<CompanyLicenseDto>> UpdateLicense(UpdateCompanyLicenseDto model);
}
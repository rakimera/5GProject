using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models.CompanyLicense;
using Application.Validation;
using AutoMapper;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Domain.Entities;

namespace Application.Services
{
    public class CompanyLicenseService : ICompanyLicenseService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly CompanyLicenseValidator _licenseValidator;

        public CompanyLicenseService(
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            CompanyLicenseValidator licenseValidator)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _licenseValidator = licenseValidator;
        }

        public BaseResponse<IEnumerable<CompanyLicenseDto>> GetAll()
        {
            IQueryable<CompanyLicense> licenses = _repositoryWrapper.CompanyLicenseRepository.GetAll();
            List<CompanyLicenseDto> models = _mapper.Map<List<CompanyLicenseDto>>(licenses);
            if (models.Count > 0)
            {
                return new BaseResponse<IEnumerable<CompanyLicenseDto>>(
                    Result: models,
                    Success: true,
                    Messages: new List<string> { "Лицензии успешно получены" });
            }

            return new BaseResponse<IEnumerable<CompanyLicenseDto>>(
                Result: models,
                Success: true,
                Messages: new List<string> { "Данные не были получены, возможно лицензии еще не созданы или удалены" });
        }

        public async Task<BaseResponse<string>> CreateAsync(CompanyLicenseDto model, string creator)
        {
            CompanyLicense license = _mapper.Map<CompanyLicense>(model);
            var result = await _licenseValidator.ValidateAsync(license);
            if (result.IsValid)
            {
                license.CreatedBy = creator;
                await _repositoryWrapper.CompanyLicenseRepository.CreateAsync(license);
                await _repositoryWrapper.Save();

                return new BaseResponse<string>(
                    Result: license.Id.ToString(),
                    Success: true,
                    Messages: new List<string> { "Лицензия успешно создана" });
            }

            List<string> messages = _mapper.Map<List<string>>(result.Errors);
            return new BaseResponse<string>(
                Result: "",
                Messages: messages,
                Success: false);
        }

        public async Task<BaseResponse<CompanyLicenseDto>> GetByOid(string oid)
        {
            CompanyLicense license =
                await _repositoryWrapper.CompanyLicenseRepository
                    .GetByCondition(x => x.Id.ToString() == oid);
            CompanyLicenseDto model = _mapper.Map<CompanyLicenseDto>(license);
            if (license == null)
            {
                return new BaseResponse<CompanyLicenseDto>(
                    Result: null,
                    Messages: new List<string> { "Лицензия не найдена" },
                    Success: true);
            }

            return new BaseResponse<CompanyLicenseDto>(
                Result: model,
                Success: true,
                Messages: new List<string> { "Лицензия успешно найдена" });
        }

        public async Task<BaseResponse<bool>> Delete(string oid)
        {
            CompanyLicense license =
                await _repositoryWrapper.CompanyLicenseRepository.GetByCondition(x => x.Id.ToString() == oid);
            if (license != null)
            {
                license.IsDelete = true;
                _repositoryWrapper.CompanyLicenseRepository.Update(license);
                await _repositoryWrapper.Save();

                return new BaseResponse<bool>(
                    Result: true,
                    Success: true,
                    Messages: new List<string> { "Лицензия успешно удалена" });
            }

            return new BaseResponse<bool>(
                Result: false,
                Messages: new List<string> { "Лицензия не существует" },
                Success: false);
        }

        public async Task<LoadResult> GetLoadResult(DataSourceLoadOptionsBase loadOptions)
        {
            var queryableLicenses = _repositoryWrapper.CompanyLicenseRepository.GetAll();
            return await DataSourceLoader.LoadAsync(queryableLicenses, loadOptions);
        }

        public async Task<BaseResponse<CompanyLicenseDto>> UpdateLicense(UpdateCompanyLicenseDto model)
        {
            BaseResponse<CompanyLicenseDto> getLicenseResponse = await GetByOid(model.Id);
            if (!getLicenseResponse.Success || getLicenseResponse.Result == null)
            {
                return new BaseResponse<CompanyLicenseDto>(
                    Result: null,
                    Messages: new List<string> { "Лицензия не найдена" },
                    Success: false);
            }
            
            CompanyLicenseDto existingLicenseDto = getLicenseResponse.Result;
            if (model.DateOfIssue == DateTime.MinValue)
            {
                model.DateOfIssue = existingLicenseDto.DateOfIssue;
            }

            _mapper.Map(model, existingLicenseDto);
            CompanyLicense license = await _repositoryWrapper.CompanyLicenseRepository
                .GetByCondition(x => x.Id.Equals(existingLicenseDto.Id));
            if (license == null)
            {
                return new BaseResponse<CompanyLicenseDto>(
                    Result: null,
                    Messages: new List<string> { "Лицензия не найдена" },
                    Success: false);
            }

            existingLicenseDto.LastModifiedBy = "Admin";
            _mapper.Map(existingLicenseDto, license);
            var result = await _licenseValidator.ValidateAsync(license);
            if (!result.IsValid)
            {
                return new BaseResponse<CompanyLicenseDto>(
                    Result: null,
                    Messages: _mapper.Map<List<string>>(result.Errors),
                    Success: false);
            }

            _repositoryWrapper.CompanyLicenseRepository.Update(license);
            await _repositoryWrapper.Save();

            return new BaseResponse<CompanyLicenseDto>(
                Result: existingLicenseDto,
                Success: true,
                Messages: new List<string> { "Лицензия успешно изменена" });
        }
    }
}
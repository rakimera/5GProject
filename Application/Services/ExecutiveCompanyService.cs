using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models.ExecutiveCompany;
using Application.Validation;
using AutoMapper;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Domain.Entities;

namespace Application.Services
{
    public class ExecutiveCompanyService : IExecutiveCompanyService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly ExecutiveCompanyValidator _executiveCompanyValidator;

        public ExecutiveCompanyService(
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            ExecutiveCompanyValidator executiveCompanyValidator)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _executiveCompanyValidator = executiveCompanyValidator;
        }

        public BaseResponse<IEnumerable<ExecutiveCompanyDto>> GetAll()
        {
            IQueryable<ExecutiveCompany> companies = _repositoryWrapper.ExecutiveCompanyRepository.GetAll();
            List<ExecutiveCompanyDto> models = _mapper.Map<List<ExecutiveCompanyDto>>(companies);
            if (models.Count > 0)
            {
                return new BaseResponse<IEnumerable<ExecutiveCompanyDto>>(
                    Result: models,
                    Success: true,
                    Messages: new List<string> { "Компании успешно получены" });
            }

            return new BaseResponse<IEnumerable<ExecutiveCompanyDto>>(
                Result: models,
                Success: true,
                Messages: new List<string> { "Данные не были получены, возможно компании еще не созданы или удалены" });
        }

        public async Task<BaseResponse<string>> CreateAsync(ExecutiveCompanyDto model, string creator)
        {
            ExecutiveCompany company = _mapper.Map<ExecutiveCompany>(model);
            var result = await _executiveCompanyValidator.ValidateAsync(company);
            if (result.IsValid)
            {
                company.CreatedBy = creator;
                await _repositoryWrapper.ExecutiveCompanyRepository.CreateAsync(company);
                await _repositoryWrapper.Save();

                return new BaseResponse<string>(
                    Result: company.Id.ToString(),
                    Success: true,
                    Messages: new List<string> { "Компания успешно создана" });
            }

            List<string> messages = _mapper.Map<List<string>>(result.Errors);
            return new BaseResponse<string>(
                Result: "",
                Messages: messages,
                Success: false);
        }

        public async Task<BaseResponse<ExecutiveCompanyDto>> GetByOid(string oid)
        {
            ExecutiveCompany company =
                await _repositoryWrapper.ExecutiveCompanyRepository
                    .GetByCondition(x => x.Id.ToString() == oid);
            ExecutiveCompanyDto model = _mapper.Map<ExecutiveCompanyDto>(company);
            if (company == null)
            {
                return new BaseResponse<ExecutiveCompanyDto>(
                    Result: null,
                    Messages: new List<string> { "Компания не найдена" },
                    Success: true);
            }

            return new BaseResponse<ExecutiveCompanyDto>(
                Result: model,
                Success: true,
                Messages: new List<string> { "Компания успешно найдена" });
        }

        public async Task<BaseResponse<bool>> Delete(string oid)
        {
            ExecutiveCompany company =
                await _repositoryWrapper.ExecutiveCompanyRepository
                    .GetByCondition(x => x.Id.ToString() == oid);
            if (company != null)
            {
                company.IsDelete = true;
                _repositoryWrapper.ExecutiveCompanyRepository.Update(company);
                await _repositoryWrapper.Save();

                return new BaseResponse<bool>(
                    Result: true,
                    Success: true,
                    Messages: new List<string> { "Компания успешно удалена" });
            }

            return new BaseResponse<bool>(
                Result: false,
                Messages: new List<string> { "Компания не существует" },
                Success: false);
        }

        public async Task<LoadResult> GetLoadResult(DataSourceLoadOptionsBase loadOptions)
        {
            var queryableCompanies = _repositoryWrapper.ExecutiveCompanyRepository.GetAll();
            return await DataSourceLoader.LoadAsync(queryableCompanies, loadOptions);
        }

        public async Task<BaseResponse<ExecutiveCompanyDto>> UpdateExecutiveCompany(UpdateExecutiveCompanyDto model)
        {
            BaseResponse<ExecutiveCompanyDto> getCompanyResponse = await GetByOid(model.Id);
            if (!getCompanyResponse.Success || getCompanyResponse.Result == null)
            {
                return new BaseResponse<ExecutiveCompanyDto>(
                    Result: null,
                    Messages: new List<string> { "Компания не найдена" },
                    Success: false);
            }

            ExecutiveCompanyDto existingCompanyDto = getCompanyResponse.Result;
            _mapper.Map(model, existingCompanyDto);
            ExecutiveCompany company =
                await _repositoryWrapper.ExecutiveCompanyRepository
                    .GetByCondition(x =>
                        x.Id.Equals(existingCompanyDto.Id));
            if (company == null)
            {
                return new BaseResponse<ExecutiveCompanyDto>(
                    Result: null,
                    Messages: new List<string> { "Компания не найдена" },
                    Success: false);
            }

            existingCompanyDto.LastModifiedBy = "Admin";
            _mapper.Map(existingCompanyDto, company);
            var result = await _executiveCompanyValidator.ValidateAsync(company);
            if (!result.IsValid)
            {
                return new BaseResponse<ExecutiveCompanyDto>(
                    Result: null,
                    Messages: _mapper.Map<List<string>>(result.Errors),
                    Success: false);
            }

            _repositoryWrapper.ExecutiveCompanyRepository.Update(company);
            await _repositoryWrapper.Save();

            return new BaseResponse<ExecutiveCompanyDto>(
                Result: existingCompanyDto,
                Success: true,
                Messages: new List<string> { "Компания успешно изменена" });
        }
    }
}
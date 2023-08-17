using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models.Roles;
using Application.Validation;
using AutoMapper;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Domain.Entities;

namespace Application.Services;

public class RoleService : IRoleService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly RoleValidator _roleValidator;

    public RoleService(
        IRepositoryWrapper repositoryWrapper,
        IMapper mapper,
        RoleValidator roleValidator)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _roleValidator = roleValidator;
    }

    public BaseResponse<IEnumerable<RoleDto>> GetAll()
    {
        var roles = _repositoryWrapper.RoleRepository.GetAll();
        List<RoleDto> models = _mapper.Map<List<RoleDto>>(roles);
        if (models.Count > 0)
        {
            return new BaseResponse<IEnumerable<RoleDto>>(
                Result: models,
                Success: true,
                Messages: new List<string> { "Роли успешно получены" });
        }

        return new BaseResponse<IEnumerable<RoleDto>>(
            Result: models,
            Success: true,
            Messages: new List<string>
                { "Данные не были получены, возможно роли еще не созданы или удалены" });
    }

    public async Task<BaseResponse<string>> CreateAsync(RoleDto model, string creator)
    {
        var mapRole = _mapper.Map<Role>(model);
        var result = await _roleValidator.ValidateAsync(mapRole);
        if (result.IsValid)
        {
            model.CreatedBy = creator;
            var user = _mapper.Map<Role>(model);
            await _repositoryWrapper.RoleRepository.CreateAsync(user);
            await _repositoryWrapper.Save();

            return new BaseResponse<string>(
                Result: user.Id.ToString(),
                Success: true,
                Messages: new List<string> { "Роль успешно создан" });
        }

        List<string> messages = _mapper.Map<List<string>>(result.Errors);
        return new BaseResponse<string>(
            Result: "",
            Messages: messages,
            Success: false);
    }

    public async Task<BaseResponse<RoleDto>> GetByOid(string oid)
    {
        var role = await _repositoryWrapper.RoleRepository
            .GetByCondition(x => x.Id.ToString() == oid);

        RoleDto? model = _mapper.Map<RoleDto>(role);

        if (role is null)
        {
            return new BaseResponse<RoleDto>(
                Result: null,
                Messages: new List<string> { "Роль не найден" },
                Success: false);
        }

        return new BaseResponse<RoleDto>(
            Result: model,
            Success: true,
            Messages: new List<string> { "Роль успешно найден" });
    }

    public async Task<BaseResponse<bool>> Delete(string oid)
    {
        var role = await _repositoryWrapper.RoleRepository
            .GetByCondition(x => x.Id.ToString() == oid);
        if (role is not null)
        {
            role.IsDelete = true;
            _repositoryWrapper.RoleRepository.Update(role);
            await _repositoryWrapper.Save();

            return new BaseResponse<bool>(
                Result: true,
                Success: true,
                Messages: new List<string> { "Роль успешно удален" });
        }

        return new BaseResponse<bool>(
            Result: false,
            Messages: new List<string> { "Роли не существует" },
            Success: false);
    }

    public async Task<LoadResult> GetLoadResult(DataSourceLoadOptionsBase loadOptions)
    {
        var queryableRoles = _repositoryWrapper.RoleRepository.GetAll();
        return await DataSourceLoader.LoadAsync(queryableRoles, loadOptions);
    }

    public async Task<BaseResponse<RoleDto>> UpdateRole(UpdateRoleDto model)
    {
        BaseResponse<RoleDto> getRoleResponse = await GetByOid(model.Id);
        if (!getRoleResponse.Success || getRoleResponse.Result == null)
        {
            return new BaseResponse<RoleDto>(
                Result: null,
                Messages: new List<string> { "Роль не найден" },
                Success: false);
        }

        RoleDto existingRoleDto = getRoleResponse.Result;
        _mapper.Map(model, existingRoleDto);

        var role = await _repositoryWrapper.RoleRepository
            .GetByCondition(x => x.Id.Equals(existingRoleDto.Id));
        if (role == null)
        {
            return new BaseResponse<RoleDto>(
                Result: null,
                Messages: new List<string> { "Роль не найден" },
                Success: false);
        }

        _mapper.Map(existingRoleDto, role);
        role.LastModifiedBy = "Admin";

        var result = await _roleValidator.ValidateAsync(role);
        if (!result.IsValid)
        {
            return new BaseResponse<RoleDto>(
                Result: null,
                Messages: _mapper.Map<List<string>>(result.Errors),
                Success: false);
        }

        _repositoryWrapper.RoleRepository.Update(role);
        await _repositoryWrapper.Save();

        return new BaseResponse<RoleDto>(
            Result: existingRoleDto,
            Success: true,
            Messages: new List<string> { "Роль успешно изменен" });
    }
}
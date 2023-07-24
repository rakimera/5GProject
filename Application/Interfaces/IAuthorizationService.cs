using Application.DataObjects;
using Application.Models;

namespace Application.Interfaces;

public interface IAuthorizationService
{
    public Task<BaseResponse<TokenDto>> Login(LoginDto loginModel);
}
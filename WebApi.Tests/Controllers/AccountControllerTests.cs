using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.DataObjects;
using Application.Interfaces;
using Application.Models.Users;
using WebApi.Controllers;
using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Data;
using AutoMapper;
using Moq;
using Xunit;

namespace WebApi.Tests.Controllers
{
    public class AccountControllerTests
    {
        [Fact]
        public Task Get_ReturnsOkResult_WhenServiceReturnsSuccess()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            var mapperMock = new Mock<IMapper>();

            var baseResponse = new BaseResponse<IEnumerable<UserDto>>(
                Result: new List<UserDto>(),
                Success: true,
                Messages: new List<string> { "Пользователи успешно получены" });

            userServiceMock.Setup(s => s.GetAll()).Returns(baseResponse);

            var serviceWrapperMock = new Mock<IServiceWrapper>();
            serviceWrapperMock.Setup(s => s.UserService).Returns(userServiceMock.Object);

            var accountController = new AccountController(serviceWrapperMock.Object, mapperMock.Object);

            // Act
            var result = accountController.Get() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var responseObject = Assert.IsType<BaseResponse<IEnumerable<UserDto>>>(result.Value);
            Assert.True(responseObject.Success);
            Assert.NotEmpty(responseObject.Messages);
            Assert.Empty(responseObject.Result);
            return Task.CompletedTask;
        }

        [Fact]
        public Task Get_ReturnsNotFound_WhenServiceReturnsFailure()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            var mapperMock = new Mock<IMapper>();

            var baseResponse = new BaseResponse<IEnumerable<UserDto>>(
                Result: null,
                Success: false,
                Messages: new List<string> { "Данные не были получены" });

            userServiceMock.Setup(s => s.GetAll()).Returns(baseResponse);

            var serviceWrapperMock = new Mock<IServiceWrapper>();
            serviceWrapperMock.Setup(s => s.UserService).Returns(userServiceMock.Object);

            var accountController = new AccountController(serviceWrapperMock.Object, mapperMock.Object);

            // Act
            var result = accountController.Get() as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);

            var responseObject = Assert.IsType<BaseResponse<IEnumerable<UserDto>>>(result.Value);
            Assert.False(responseObject.Success);
            Assert.NotEmpty(responseObject.Messages);
            Assert.Null(responseObject.Result);
            return Task.CompletedTask;
        }

        [Fact]
        public async Task GetByOid_ReturnsOkResult_WhenServiceReturnsSuccess()
        {
            // Arrange
            var oid = "some_oid";
            var userServiceMock = new Mock<IUserService>();
            var mapperMock = new Mock<IMapper>();

            var baseResponse = new BaseResponse<UserDto>(
                Result: new UserDto(),
                Success: true,
                Messages: new List<string> { "Пользователь успешно найден" });

            userServiceMock.Setup(s => s.GetByOid(oid)).ReturnsAsync(baseResponse);

            var serviceWrapperMock = new Mock<IServiceWrapper>();
            serviceWrapperMock.Setup(s => s.UserService).Returns(userServiceMock.Object);

            var accountController = new AccountController(serviceWrapperMock.Object, mapperMock.Object);

            // Act
            var result = await accountController.Get(oid) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var responseObject = Assert.IsType<BaseResponse<UserDto>>(result.Value);
            Assert.True(responseObject.Success);
            Assert.NotEmpty(responseObject.Messages);
            Assert.NotNull(responseObject.Result);
        }

        [Fact]
        public async Task GetByOid_ReturnsNotFound_WhenServiceReturnsFailure()
        {
            // Arrange
            var oid = "some_oid";
            var userServiceMock = new Mock<IUserService>();
            var mapperMock = new Mock<IMapper>();

            var baseResponse = new BaseResponse<UserDto>(
                Result: null,
                Success: false,
                Messages: new List<string> { "Пользователь не найден" });

            userServiceMock.Setup(s => s.GetByOid(oid)).ReturnsAsync(baseResponse);

            var serviceWrapperMock = new Mock<IServiceWrapper>();
            serviceWrapperMock.Setup(s => s.UserService).Returns(userServiceMock.Object);

            var accountController = new AccountController(serviceWrapperMock.Object, mapperMock.Object);

            // Act
            var result = await accountController.Get(oid) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);

            var responseObject = Assert.IsType<BaseResponse<UserDto>>(result.Value);
            Assert.False(responseObject.Success);
            Assert.NotEmpty(responseObject.Messages);
            Assert.Null(responseObject.Result);
        }

        [Fact]
        public async Task Post_ReturnsOkResult_WhenServiceReturnsSuccess()
        {
            // Arrange
            var mockHttpContext = new Mock<HttpContext>();
            var mockIdentity = new GenericIdentity("username");
            mockHttpContext.SetupGet(context => context.User.Identity).Returns(mockIdentity);

            var controllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };
            var createUserDto = new CreateUserDto();
            var mappedUserDto = new UserDto();
            var userServiceMock = new Mock<IUserService>();
            var mapperMock = new Mock<IMapper>();

            var baseResponse = new BaseResponse<string>(
                Result: "user_id",
                Success: true,
                Messages: new List<string> { "Пользователь успешно создан" });

            mapperMock
                .Setup(m => m.Map<UserDto>(createUserDto))
                .Returns(mappedUserDto);

            userServiceMock.Setup(s => s.CreateAsync(mappedUserDto, It.IsAny<string>()))
                .ReturnsAsync(baseResponse);

            var serviceWrapperMock = new Mock<IServiceWrapper>();
            serviceWrapperMock.Setup(s => s.UserService).Returns(userServiceMock.Object);

            var accountController = new AccountController(serviceWrapperMock.Object, mapperMock.Object)
            {
                ControllerContext = controllerContext
            };

            // Act
            var result = await accountController.Post(createUserDto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var responseObject = Assert.IsType<BaseResponse<string>>(result.Value);
            Assert.True(responseObject.Success);
            Assert.NotEmpty(responseObject.Messages);
            Assert.Equal("user_id", responseObject.Result);
        }

        [Fact]
        public async Task Post_ReturnsBadRequest_WhenServiceReturnsFailure()
        {
            // Arrange
            var mockHttpContext = new Mock<HttpContext>();
            var mockIdentity = new GenericIdentity("username");
            mockHttpContext.SetupGet(context => context.User.Identity).Returns(mockIdentity);

            var controllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };
            var createUserDto = new CreateUserDto();
            var mappedUserDto = new UserDto();
            var userServiceMock = new Mock<IUserService>();
            var mapperMock = new Mock<IMapper>();

            var baseResponse = new BaseResponse<string>(
                Result: "",
                Success: false,
                Messages: new List<string> { "Ошибка при создании пользователя" });

            mapperMock.Setup(m => m.Map<UserDto>(createUserDto)).Returns(mappedUserDto);

            userServiceMock.Setup(s => s.CreateAsync(mappedUserDto, It.IsAny<string>()))
                .ReturnsAsync(baseResponse);

            var serviceWrapperMock = new Mock<IServiceWrapper>();
            serviceWrapperMock.Setup(s => s.UserService).Returns(userServiceMock.Object);

            var accountController = new AccountController(serviceWrapperMock.Object, mapperMock.Object)
            {
                ControllerContext = controllerContext
            };

            // Act
            var result = await accountController.Post(createUserDto) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);

            var responseObject = Assert.IsType<BaseResponse<string>>(result.Value);
            Assert.False(responseObject.Success);
            Assert.NotEmpty(responseObject.Messages);
            Assert.Equal("", responseObject.Result);
        }

        [Fact]
        public async Task Put_ReturnsOkResult_WhenServiceReturnsSuccess()
        {
            // Arrange
            var updateUserDto = new UpdateUserDto();
            var userServiceMock = new Mock<IUserService>();
            var mapperMock = new Mock<IMapper>();

            var baseResponse = new BaseResponse<UserDto>(
                Result: new UserDto(),
                Success: true,
                Messages: new List<string> { "Пользователь успешно изменен" });

            userServiceMock.Setup(s => s.UpdateUser(updateUserDto)).ReturnsAsync(baseResponse);

            var serviceWrapperMock = new Mock<IServiceWrapper>();
            serviceWrapperMock.Setup(s => s.UserService).Returns(userServiceMock.Object);

            var accountController = new AccountController(serviceWrapperMock.Object, mapperMock.Object);

            // Act
            var result = await accountController.Put(updateUserDto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var responseObject = Assert.IsType<BaseResponse<UserDto>>(result.Value);
            Assert.True(responseObject.Success);
            Assert.NotEmpty(responseObject.Messages);
            Assert.NotNull(responseObject.Result);
        }

        [Fact]
        public async Task Put_ReturnsBadRequest_WhenServiceReturnsFailure()
        {
            // Arrange
            var updateUserDto = new UpdateUserDto();
            var userServiceMock = new Mock<IUserService>();
            var mapperMock = new Mock<IMapper>();

            var baseResponse = new BaseResponse<UserDto>(
                Result: null,
                Success: false,
                Messages: new List<string> { "Ошибка при изменении пользователя" });

            userServiceMock.Setup(s => s.UpdateUser(updateUserDto)).ReturnsAsync(baseResponse);

            var serviceWrapperMock = new Mock<IServiceWrapper>();
            serviceWrapperMock.Setup(s => s.UserService).Returns(userServiceMock.Object);

            var accountController = new AccountController(serviceWrapperMock.Object, mapperMock.Object);

            // Act
            var result = await accountController.Put(updateUserDto) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);

            var responseObject = Assert.IsType<BaseResponse<UserDto>>(result.Value);
            Assert.False(responseObject.Success);
            Assert.NotEmpty(responseObject.Messages);
            Assert.Null(responseObject.Result);
        }

        [Fact]
        public async Task Delete_ReturnsOkResult_WhenServiceReturnsSuccess()
        {
            // Arrange
            var oid = "some_oid";
            var userServiceMock = new Mock<IUserService>();
            var mapperMock = new Mock<IMapper>();

            var baseResponse = new BaseResponse<bool>(
                Result: true,
                Success: true,
                Messages: new List<string> { "Пользователь успешно удален" });

            userServiceMock.Setup(s => s.Delete(oid)).ReturnsAsync(baseResponse);

            var serviceWrapperMock = new Mock<IServiceWrapper>();
            serviceWrapperMock.Setup(s => s.UserService).Returns(userServiceMock.Object);

            var accountController = new AccountController(serviceWrapperMock.Object, mapperMock.Object);

            // Act
            var result = await accountController.Delete(oid) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var responseObject = Assert.IsType<BaseResponse<bool>>(result.Value);
            Assert.True(responseObject.Success);
            Assert.NotEmpty(responseObject.Messages);
            Assert.True(responseObject.Result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenServiceReturnsFailure()
        {
            // Arrange
            var oid = "some_oid";
            var userServiceMock = new Mock<IUserService>();
            var mapperMock = new Mock<IMapper>();

            var baseResponse = new BaseResponse<bool>(
                Result: false,
                Success: false,
                Messages: new List<string> { "Пользователя не существует" });

            userServiceMock.Setup(s => s.Delete(oid)).ReturnsAsync(baseResponse);

            var serviceWrapperMock = new Mock<IServiceWrapper>();
            serviceWrapperMock.Setup(s => s.UserService).Returns(userServiceMock.Object);

            var accountController = new AccountController(serviceWrapperMock.Object, mapperMock.Object);

            // Act
            var result = await accountController.Delete(oid) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);

            var responseObject = Assert.IsType<BaseResponse<bool>>(result.Value);
            Assert.False(responseObject.Success);
            Assert.NotEmpty(responseObject.Messages);
            Assert.False(responseObject.Result);
        }

        [Fact]
        public async Task GetLoadResult_ReturnsOkResult()
        {
            // Arrange
            var loadOptions = new DataSourceLoadOptionsBase();
            var userServiceMock = new Mock<IUserService>();
            var mapperMock = new Mock<IMapper>();

            var loadResult = new LoadResult();
            userServiceMock.Setup(s => s.GetLoadResult(loadOptions)).ReturnsAsync(loadResult);

            var serviceWrapperMock = new Mock<IServiceWrapper>();
            serviceWrapperMock.Setup(s => s.UserService).Returns(userServiceMock.Object);

            var accountController = new AccountController(serviceWrapperMock.Object, mapperMock.Object);

            // Act
            var result = await accountController.Get(loadOptions) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var responseObject = Assert.IsType<LoadResult>(result.Value);
            Assert.Same(loadResult, responseObject);
        }
    }
}
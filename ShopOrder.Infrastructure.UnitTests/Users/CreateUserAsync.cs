#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShopOrder.Domain.Core.Models.Users;
using ShopOrder.Domain.Interfaces.Users;
using ShopOrder.Infrastructure.Business.Users;
using ShopOrder.Services.Interfaces.Users;
using static ShopOrder.Infrastructure.UnitTests.Users.TestData.UsersTestData;
using UserValidationMessages = ShopOrder.Domain.Core.Infrastructure.Constants.Validation.Users;

namespace ShopOrder.Infrastructure.UnitTests.Users
{
    public class CreateUserAsync
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly IUserService _userService;
        private readonly CreateUserAsync_TestData TestData;

        private User User { get; set; }
        private ModelStateDictionary ModelState { get; set; }

        public CreateUserAsync()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object);
            TestData = new CreateUserAsync_TestData();

            User = TestData.Users.FirstOrDefault()!;
            ModelState = TestData.ModelState;
        }

        [Fact]
        public async Task Creating_user_with_null_should_return_BadRequest()
        {
            var actual = await _userService.CreateUserAsync(null, ModelState);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.BadRequest, actual.Result.Error.Key);
            Assert.Equal(CommonValidationMessages.IncorrectDataProvided(), actual.Result.Error.Value);
        }

        [Fact]
        public async Task Creating_user_with_invalid_model_should_return_BadRequest()
        {
            const string errorKey = "ErrorKey";
            const string errorValue = "ErrorValue";
            ModelState.AddModelError(errorKey, errorValue);

            var actual = await _userService.CreateUserAsync(User, ModelState);

            Assert.True(actual.Result.Failed);
            Assert.Equal(errorValue, actual.Result.Error.Value);
            Assert.Equal(HttpStatusCode.BadRequest, actual.Result.Error.Key);
        }

        [Fact]
        public async Task Creating_user_with_existing_email_should_return_ConflictResponse()
        {
            _userRepositoryMock.Setup(repo => repo.CheckIfSameEmailExistsAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(true));

            var actual = await _userService.CreateUserAsync(User, ModelState);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.Conflict, actual.Result.Error.Key);
            Assert.Equal(UserValidationMessages.SameUserExists(), actual.Result.Error.Value);
        }

        [Fact]
        public async Task User_creation_should_return_UserResponse_with_User()
        {
            _userRepositoryMock.Setup(repo => repo.CheckIfSameEmailExistsAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(false));
            _userRepositoryMock.Setup(repo => repo.CreateUserAsync(User))
                .Returns(Task.FromResult<User?>(User));

            var actual = await _userService.CreateUserAsync(User, ModelState);

            Assert.True(actual.Result.Succeeded);
            Assert.NotNull(actual.User);
        }

        [Fact]
        public async Task Creating_user_with_exception_should_return_InternalServerError()
        {
            _userRepositoryMock.Setup(repo => repo.CheckIfSameEmailExistsAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(false));
            _userRepositoryMock.Setup(repo => repo.CreateUserAsync(User))
                .Throws<Exception>();

            var actual = await _userService.CreateUserAsync(User, ModelState);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.InternalServerError, actual.Result.Error.Key);
        }
    }
}
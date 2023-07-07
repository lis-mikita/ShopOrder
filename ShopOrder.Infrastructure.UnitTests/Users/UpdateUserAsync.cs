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
    public class UpdateUserAsync
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly IUserService _userService;
        private readonly UpdateUserAsync_TestData TestData;

        private User User { get; set; }
        private int Id { get; set; }
        private ModelStateDictionary ModelState { get; set; }

        public UpdateUserAsync()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object);
            TestData = new UpdateUserAsync_TestData();

            User = TestData.Users.FirstOrDefault()!;
            Id = TestData.Id;
            ModelState = TestData.ModelState;
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task Update_user_with_invalid_id_should_return_BadRequest(int id)
        {
            var actual = await _userService.UpdateUserAsync(id, User, ModelState);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.BadRequest, actual.Result.Error.Key);
            Assert.Equal(UserValidationMessages.IncorrectId(id), actual.Result.Error.Value);
        }

        [Fact]
        public async Task Update_user_with_null_should_return_BadRequest()
        {
            var actual = await _userService.UpdateUserAsync(Id, null, ModelState);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.BadRequest, actual.Result.Error.Key);
            Assert.Equal(CommonValidationMessages.IncorrectDataProvided(), actual.Result.Error.Value);
        }

        [Fact]
        public async Task Update_user_with_invalid_model_should_return_BadRequest()
        {
            const string errorKey = "ErrorKey";
            const string errorValue = "ErrorValue";
            ModelState.AddModelError(errorKey, errorValue);

            var actual = await _userService.UpdateUserAsync(Id, User, ModelState);

            Assert.True(actual.Result.Failed);
            Assert.Equal(errorValue, actual.Result.Error.Value);
            Assert.Equal(HttpStatusCode.BadRequest, actual.Result.Error.Key);
        }

        [Fact]
        public async Task Update_user_with_changed_email_when_new_email_exists_should_return_ConflictResponse()
        {
            _userRepositoryMock.Setup(repo => repo.GetUserAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<User?>(User));
            _userRepositoryMock.Setup(repo => repo.CheckIfSameEmailExistsAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(true));
            var user = new User { Email = "test" };

            var actual = await _userService.UpdateUserAsync(Id, user, ModelState);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.Conflict, actual.Result.Error.Key);
            Assert.Equal(UserValidationMessages.UpdateEmailExists(user.Email), actual.Result.Error.Value);
        }

        [Fact]
        public async Task Updated_user_should_return_UserResponse_with_User()
        {
            _userRepositoryMock.Setup(repo => repo.GetUserAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<User?>(User));
            _userRepositoryMock.Setup(repo => repo.UpdateUserAsync(It.IsAny<int>(), User))
                .Returns(Task.FromResult<User?>(User));

            var actual = await _userService.UpdateUserAsync(Id, User, ModelState);

            Assert.True(actual.Result.Succeeded);
            Assert.NotNull(actual.User);
        }

        [Fact]
        public async Task Updated_user_not_should_return_NotFoundResponse()
        {
            _userRepositoryMock.Setup(repo => repo.GetUserAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<User?>(null));

            var actual = await _userService.UpdateUserAsync(Id, User, ModelState);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.NotFound, actual.Result.Error.Key);
            Assert.Equal(UserValidationMessages.UserNotFound(User.UserId), actual.Result.Error.Value);
        }

        [Fact]
        public async Task Updating_user_with_exception_should_return_InternalServerError()
        {
            _userRepositoryMock.Setup(repo => repo.GetUserAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<User?>(User));
            _userRepositoryMock.Setup(repo => repo.UpdateUserAsync(It.IsAny<int>(), User))
                .Throws<Exception>();

            var actual = await _userService.UpdateUserAsync(Id, User, ModelState);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.InternalServerError, actual.Result.Error.Key);
        }
    }
}
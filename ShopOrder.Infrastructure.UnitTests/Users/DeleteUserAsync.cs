using ShopOrder.Domain.Interfaces.Users;
using ShopOrder.Infrastructure.Business.Users;
using ShopOrder.Services.Interfaces.Users;
using static ShopOrder.Infrastructure.UnitTests.Users.TestData.UsersTestData;
using UserValidationMessages = ShopOrder.Domain.Core.Infrastructure.Constants.Validation.Users;

namespace ShopOrder.Infrastructure.UnitTests.Users
{
    public class DeleteUserAsync
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly IUserService _userService;
        private readonly DeleteUserAsync_TestData TestData;

        private int Id { get; set; }

        public DeleteUserAsync()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object);
            TestData = new DeleteUserAsync_TestData();

            Id = TestData.Id;
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task Delete_user_with_invalid_id_should_return_BadRequest(int id)
        {
            var actual = await _userService.DeleteUserAsync(id);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.BadRequest, actual.Result.Error.Key);
            Assert.Equal(CommonValidationMessages.IncorrectDataProvided(), actual.Result.Error.Value);
        }

        [Fact]
        public async Task Delete_user_with_valid_id_should_return_OkResponse()
        {
            _userRepositoryMock.Setup(repo => repo.DeleteUserAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(true));

            var actual = await _userService.DeleteUserAsync(Id);

            Assert.True(actual.Result.Succeeded);
        }

        [Fact]
        public async Task Delete_user_not_found_should_return_NotFoundResponse()
        {
            _userRepositoryMock.Setup(repo => repo.DeleteUserAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(false));

            var actual = await _userService.DeleteUserAsync(Id);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.NotFound, actual.Result.Error.Key);
            Assert.Equal(UserValidationMessages.UserNotFound(Id), actual.Result.Error.Value);
        }

        [Fact]
        public async Task Deleting_user_with_exception_should_return_InternalServerError()
        {
            _userRepositoryMock.Setup(repo => repo.DeleteUserAsync(It.IsAny<int>()))
                .Throws<Exception>();

            var actual = await _userService.DeleteUserAsync(Id);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.InternalServerError, actual.Result.Error.Key);
        }
    }
}
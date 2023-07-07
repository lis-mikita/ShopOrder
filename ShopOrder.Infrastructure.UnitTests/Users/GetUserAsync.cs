using ShopOrder.Domain.Interfaces.Users;
using ShopOrder.Infrastructure.Business.Users;
using ShopOrder.Services.Interfaces.Users;
using UserValidationMessages = ShopOrder.Domain.Core.Infrastructure.Constants.Validation.Users;
using static ShopOrder.Infrastructure.UnitTests.Users.TestData.UsersTestData;
using ShopOrder.Domain.Core.Models.Users;

namespace ShopOrder.Infrastructure.UnitTests.Users
{
    public class GetUserAsync
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly IUserService _userService;
        private readonly GetUserAsync_TestData TestData;

        private User User { get; set; }

        public GetUserAsync()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object);
            TestData = new GetUserAsync_TestData();

            User = TestData.Users.FirstOrDefault()!;
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task User_with_invalid_id_should_return_BadRequest(int id)
        {
            var actual = await _userService.GetUserAsync(id);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.BadRequest, actual.Result.Error.Key);
            Assert.Equal(UserValidationMessages.IncorrectId(id), actual.Result.Error.Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        public async Task User_with_valid_id_should_return_UserResponse_with_User(int id)
        {
            _userRepositoryMock.Setup(repo => repo.GetUserAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(TestData.Users.Find(u => u.UserId == id)));

            var actual = await _userService.GetUserAsync(id);

            Assert.True(actual.Result.Succeeded);
            Assert.NotNull(actual.User);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(int.MaxValue)]
        public async Task User_not_found_should_return_NotFoundResponse(int id)
        {
            _userRepositoryMock.Setup(repo => repo.GetUserAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<User?>(null));

            var actual = await _userService.GetUserAsync(id);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.NotFound, actual.Result.Error.Key);
            Assert.Equal(UserValidationMessages.UserNotFound(id), actual.Result.Error.Value);
        }

        [Fact]
        public async Task Exception_while_getting_user_should_return_InternalServerError()
        {
            _userRepositoryMock.Setup(repo => repo.GetUserAsync(It.IsAny<int>()))
                .Throws<Exception>();

            var actual = await _userService.GetUserAsync(User.UserId);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.InternalServerError, actual.Result.Error.Key);
        }
    }
}
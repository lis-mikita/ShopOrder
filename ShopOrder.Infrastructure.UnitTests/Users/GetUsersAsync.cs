using ShopOrder.Domain.Core.Models.Users;
using ShopOrder.Domain.Interfaces.Users;
using ShopOrder.Infrastructure.Business.Users;
using ShopOrder.Services.Interfaces.Users;
using static ShopOrder.Infrastructure.UnitTests.Users.TestData.UsersTestData;
using UserValidationMessages = ShopOrder.Domain.Core.Infrastructure.Constants.Validation.Users;

namespace ShopOrder.Infrastructure.UnitTests.Users
{
    public class GetUsersAsync
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly IUserService _userService;
        private readonly GetUsersAsync_TestData TestData;

        private IEnumerable<User> Users { get; set; }

        public GetUsersAsync()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object);
            TestData = new GetUsersAsync_TestData();

            Users = TestData.Users;
        }

        [Fact]
        public async Task GetUsersAsync_with_existing_users_should_return_list_of_users()
        {
            _userRepositoryMock.Setup(repo => repo.GetUsersAsync())
                .Returns(Task.FromResult(Users));

            var actual = await _userService.GetUsersAsync();

            Assert.True(actual.Result.Succeeded);
            Assert.NotNull(actual.Users);
        }

        [Fact]
        public async Task Users_not_found_should_return_NotFoundResponse()
        {
            _userRepositoryMock.Setup(repo => repo.GetUsersAsync())
                .Returns(Task.FromResult(Enumerable.Empty<User>()));

            var actual = await _userService.GetUsersAsync();

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.NotFound, actual.Result.Error.Key);
            Assert.Equal(UserValidationMessages.UsersNotFound(), actual.Result.Error.Value);
        }

        [Fact]
        public async Task Exception_while_getting_users_should_return_InternalServerError()
        {
            _userRepositoryMock.Setup(repo => repo.GetUsersAsync())
                .Throws<Exception>();

            var actual = await _userService.GetUsersAsync();

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.InternalServerError, actual.Result.Error.Key);
        }
    }
}
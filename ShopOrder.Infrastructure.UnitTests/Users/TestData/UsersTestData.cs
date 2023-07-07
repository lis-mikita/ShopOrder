using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShopOrder.Domain.Core.Models.Orders;
using ShopOrder.Domain.Core.Models.Users;

namespace ShopOrder.Infrastructure.UnitTests.Users.TestData
{
    static class UsersTestData
    {
        private const char TestChar = '-';
        private const int CharsCount = 10;

        public class CreateUserAsync_TestData
        {
            public List<User> Users = new()
            {
                new User
                {
                    FirstName = new string(TestChar, CharsCount),
                    LastName = new string(TestChar, CharsCount),
                    Email = new string(TestChar, CharsCount),
                    Password = new string(TestChar, CharsCount),
                    RegistrationDate = DateTime.Now
                }
            };

            public ModelStateDictionary ModelState = new();
        }

        public class GetUserAsync_TestData
        {
            public List<User> Users = new()
            {
                new User
                {
                    UserId = 1,
                    FirstName = new string(TestChar, CharsCount),
                    LastName = new string(TestChar, CharsCount),
                    Email = new string(TestChar, CharsCount),
                    Password = new string(TestChar, CharsCount),
                    RegistrationDate = DateTime.Now
                },
                new User
                {
                    UserId = 3,
                    FirstName = new string(TestChar, CharsCount),
                    LastName = new string(TestChar, CharsCount),
                    Email = new string(TestChar, CharsCount),
                    Password = new string(TestChar, CharsCount),
                    RegistrationDate = DateTime.Now
                }
            };
        }

        public class UpdateUserAsync_TestData
        {
            public List<User> Users = new()
            {
                new User
                {
                    FirstName = new string(TestChar, CharsCount),
                    LastName = new string(TestChar, CharsCount),
                    Email = new string(TestChar, CharsCount),
                    Password = new string(TestChar, CharsCount),
                    RegistrationDate = DateTime.Now
                }
            };

            public ModelStateDictionary ModelState = new();

            public int Id = 1;
        }

        public class DeleteUserAsync_TestData
        {
            public int Id = 1;
        }

        public class GetUsersAsync_TestData
        {
            public List<User> Users = new()
            {
                new User
                {
                    UserId = 1,
                    FirstName = new string(TestChar, CharsCount),
                    LastName = new string(TestChar, CharsCount),
                    Email = new string(TestChar, CharsCount),
                    Password = new string(TestChar, CharsCount),
                    RegistrationDate = DateTime.Now
                }
            };
        }

        public class GetUserWithOrdersAsync_TestData
        {
            public List<User> Users = new()
            {
                new User
                {
                    UserId = 1,
                    FirstName = new string(TestChar, CharsCount),
                    LastName = new string(TestChar, CharsCount),
                    Email = new string(TestChar, CharsCount),
                    Password = new string(TestChar, CharsCount),
                    RegistrationDate = DateTime.Now,
                    Orders = new[]
                    {
                        new Order
                        {
                            OrderId = 1,
                            UserId = 1,
                            OrderDate = DateTime.Now,
                            TotalAmount = 0,
                            Status = Domain.Core.Infrastructure.Enums.OrderStatus.New,
                            DeliveryAddress = new string(TestChar, CharsCount),
                        }
                    }
                }
            };
        }
    }
}
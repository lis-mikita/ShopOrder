#pragma warning disable S3400 // Methods should not return constants

namespace ShopOrder.Domain.Core.Infrastructure
{
    public static class Constants
    {
        public static class Validation
        {
            public static class Users
            {
                public static string IncorrectId(int id) =>
                    $"Incorrect user id {id}!";
                public static string UserNotFound(int id) =>
                    $"User with id {id} not found!";
                public static string UsersNotFound() =>
                    "No users in database!";
                public static string SameUserExists() =>
                    "Same user already exists!";

                public static string UpdateEmailExists(string email) =>
                    $"User with new email {email} already exists!";

                public const string FirstNameMaxLength = "Sorry, but max length of first name is 30!";
                public const string LastNameMaxLength = "Sorry, but max length of last name is 30!";
                public const string EmailMaxLength = "Sorry, but max length of email is 30!";
                public const string EmailError = "Check whether your email is correct!";
                public const string PasswordMaxLength = "Sorry, but max length of password is 30!";
                public const string PasswordMinLength = "Sorry, but min length of password is 8!";
            }

            public static class Orders
            {
                public static string IncorrectId(int id) =>
                    $"Incorrect order id {id}!";
                public static string IncorrectUserId(int userId) =>
                    $"Incorrect user id {userId} in order! User with this id doesn't exist!";
                public static string OrderNotFound(int id) =>
                    $"Order with id {id} not found!";
                public static string OrdersNotFound() =>
                    "No orders in database!";

                public const string DeliveryAddressMaxLength = "Sorry, but max length of delivery address is 60!";
                public const string TotalAmountNegative = "Sorry, but value of total amount must be greater than 0!";
            }

            public static class OrderDetails
            {
                public static string IncorrectId(int id) =>
                    $"Incorrect order detail id {id}!";
                public static string IncorrectOrderId(int orderId) =>
                    $"Incorrect order id {orderId} in order detail! Order with this id doesn't exist!";
                public static string OrderDetailNotFound(int id) =>
                    $"Order detail with id {id} not found!";
                public static string OrderDetailsNotFound() =>
                    "No order details in database!";

                public const string ProductNameMaxLength = "Sorry, but max length of product name is 30!";
                public const string QuantityNegative = "Sorry, but min value of quantity is 1!";
                public const string PriceNegative = "Sorry, but value of price must be greater than 0!";
                public const string SubtotalNegative = "Sorry, but value of subtotal must be greater than 0!";
            }

            public static class CommonErrors
            {
                public static string ServerError(string message) =>
                    $"Seems that something went wrong with server: {message}";

                public static string SQLError(string message) =>
                    $"Seems that something went wrong with database: {message}";

                public static string IncorrectDataProvided() =>
                    "Incorrect data provided!";

                public static string SavingError(string message) =>
                    $"Seem that something went wrong with save changes in database! {message}";
            }
        }
    }
}

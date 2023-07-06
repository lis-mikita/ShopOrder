using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using ShopOrder.Domain.Core.Messages;
using ShopOrder.Domain.Core.Messages.Users;
using ShopOrder.Domain.Core.Models.Users;
using ShopOrder.Domain.Interfaces.Users;
using ShopOrder.Services.Interfaces.Users;
using System.Net;
using System.Data.SqlClient;
using static ShopOrder.Domain.Core.Infrastructure.Constants;

namespace ShopOrder.Infrastructure.Business.Users
{
    public class UserService : IUserService
    {
        private const int MIN_USER_ID = 1;
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) => _userRepository = userRepository;

        public async Task<UserResponse> CreateUserAsync(User user, ModelStateDictionary modelState)
        {
            try
            {
                if (user is null)
                {
                    return BaseResponse.Failure<UserResponse>(HttpStatusCode.BadRequest,
                        Validation.CommonErrors.IncorrectDataProvided());
                }

                if (!modelState.IsValid)
                {
                    var errorMessages = string.Join("; ", modelState.Values
                        .SelectMany(entry => entry.Errors)
                        .Select(error => error.ErrorMessage));

                    return BaseResponse.Failure<UserResponse>(HttpStatusCode.BadRequest, errorMessages);
                }

                if (await _userRepository.CheckIfSameEmailExistsAsync(user.Email!))
                {
                    return BaseResponse.Failure<UserResponse>(HttpStatusCode.Conflict,
                        Validation.Users.SameUserExists());
                }

                user.RegistrationDate = DateTime.Now;
                var userNew = await _userRepository.CreateUserAsync(user);

                return new UserResponse { User = userNew };
            }
            catch (DbUpdateException ex)
            {
                return BaseResponse.Failure<UserResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.SavingError(ex.Message));
            }
            catch (SqlException ex)
            {
                return BaseResponse.Failure<UserResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.SQLError(ex.Message));
            }
            catch (Exception ex)
            {
                return BaseResponse.Failure<UserResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.ServerError(ex.Message));
            }
        }

        public async Task<UserResponse> GetUserAsync(int id)
        {
            try
            {
                if (id < MIN_USER_ID)
                {
                    return BaseResponse.Failure<UserResponse>(HttpStatusCode.BadRequest,
                        Validation.Users.IncorrectId(id));
                }

                var user = await _userRepository.GetUserAsync(id);
                if (user is null)
                {
                    return BaseResponse.Failure<UserResponse>(HttpStatusCode.NotFound,
                        Validation.Users.UserNotFound(id));
                }

                return new UserResponse() { User = user };
            }
            catch (SqlException ex)
            {
                return BaseResponse.Failure<UserResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.SQLError(ex.Message));
            }
            catch (Exception ex)
            {
                return BaseResponse.Failure<UserResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.ServerError(ex.Message));
            }
        }

        public async Task<UserResponse> UpdateUserAsync(int id, User user, ModelStateDictionary modelState)
        {
            try
            {
                if (id < MIN_USER_ID)
                {
                    return BaseResponse.Failure<UserResponse>(HttpStatusCode.BadRequest,
                        Validation.Users.IncorrectId(id));
                }

                if (user is null)
                {
                    return BaseResponse.Failure<UserResponse>(HttpStatusCode.BadRequest,
                        Validation.CommonErrors.IncorrectDataProvided());
                }

                if (!modelState.IsValid)
                {
                    var errorMessages = string.Join("; ", modelState.Values
                        .SelectMany(entry => entry.Errors)
                        .Select(error => error.ErrorMessage));

                    return BaseResponse.Failure<UserResponse>(HttpStatusCode.BadRequest, errorMessages);
                }

                var existedUser = await _userRepository.GetUserAsync(id);

                if (existedUser is null)
                {
                    return BaseResponse.Failure<UserResponse>(HttpStatusCode.NotFound,
                        Validation.Users.UserNotFound(user.UserId));
                }

                if (!string.Equals(existedUser.Email, user.Email, StringComparison.OrdinalIgnoreCase))
                {
                    bool isSameEmailExists = await _userRepository.CheckIfSameEmailExistsAsync(user.Email!);
                    if (isSameEmailExists)
                    {
                        return BaseResponse.Failure<UserResponse>(HttpStatusCode.Conflict,
                            Validation.Users.UpdateEmailExists(user.Email!));
                    }
                }

                var updatedUser = await _userRepository.UpdateUserAsync(id, user);

                return new UserResponse { User = updatedUser };

            }
            catch (DbUpdateException ex)
            {
                return BaseResponse.Failure<UserResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.SavingError(ex.Message));
            }
            catch (SqlException ex)
            {
                return BaseResponse.Failure<UserResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.SQLError(ex.Message));
            }
            catch (Exception ex)
            {
                return BaseResponse.Failure<UserResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.ServerError(ex.Message));
            }
        }

        public async Task<BaseResponse> DeleteUserAsync(int id)
        {
            try
            {
                if (id < MIN_USER_ID)
                {
                    return BaseResponse.Failure(HttpStatusCode.BadRequest,
                        Validation.CommonErrors.IncorrectDataProvided());
                }

                var deleted = await _userRepository.DeleteUserAsync(id);

                if (!deleted)
                {
                    return BaseResponse.Failure(HttpStatusCode.NotFound,
                        Validation.Users.UserNotFound(id));
                }

                return BaseResponse.Success;
            }
            catch (DbUpdateException ex)
            {
                return BaseResponse.Failure(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.SavingError(ex.Message));
            }
            catch (SqlException ex)
            {
                return BaseResponse.Failure(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.SQLError(ex.Message));
            }
            catch (Exception ex)
            {
                return BaseResponse.Failure(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.ServerError(ex.Message));
            }
        }

        public async Task<UsersResponse> GetUsersAsync()
        {
            try
            {
                var users = await _userRepository.GetUsersAsync();

                if (users is null || !users.Any())
                {
                    return BaseResponse.Failure<UsersResponse>(HttpStatusCode.NotFound,
                        Validation.Users.UsersNotFound());
                }

                return new UsersResponse { Users = users };
            }
            catch (SqlException ex)
            {
                return BaseResponse.Failure<UsersResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.SQLError(ex.Message));
            }
            catch (Exception ex)
            {
                return BaseResponse.Failure<UsersResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.ServerError(ex.Message));
            }
        }

        public async Task<UserResponse> GetUserWithOrdersAsync(int id)
        {
            try
            {
                if (id < MIN_USER_ID)
                {
                    return BaseResponse.Failure<UserResponse>(HttpStatusCode.BadRequest,
                        Validation.Users.IncorrectId(id));
                }

                var user = await _userRepository.GetUserWithOrdersAsync(id);
                if (user is null)
                {
                    return BaseResponse.Failure<UserResponse>(HttpStatusCode.NotFound,
                        Validation.Users.UserNotFound(id));
                }

                return new UserResponse() { User = user };
            }
            catch (SqlException ex)
            {
                return BaseResponse.Failure<UserResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.SQLError(ex.Message));
            }
            catch (Exception ex)
            {
                return BaseResponse.Failure<UserResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.ServerError(ex.Message));
            }
        }
    }
}
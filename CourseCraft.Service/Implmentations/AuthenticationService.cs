using System.Text;
using CourseCraft.Repository.Intefaces;
using CourseCraft.Repository.Models;
using CourseCraft.Repository.ViewModels;
using CourseCraft.Service.Intefaces;
using Microsoft.EntityFrameworkCore;

namespace CourseCraft.Service.Implmentations;

public class AuthenticationService(IUsersRepository usersRepository) : IAuthenticationService
{
    private readonly IUsersRepository _usersRepository = usersRepository;

    #region AuthenticateUserUsingEmailPassword

    public async Task<UserLoginViewModel?> AuthenticateUserUsingEmailPasswordAsync(string userEmail, string userPassword)
    {
        string? hashedPassword = HashPassword(userPassword);
        User? user = await _usersRepository.GetAllUserAsQueryable()
                                                .FirstOrDefaultAsync(u => u.UserEmail.ToLower() == userEmail.ToLower()
                                                                && u.UserPassword.ToLower() == hashedPassword.ToLower());

        if (user == null)
            return null;

        UserLoginViewModel userLoginViewModel = new()
        {
            UserEmail = user.UserEmail,
            UserPassword = user.UserPassword,
            UserRole = user.UserRole,
        };

        return userLoginViewModel;
    }
    #endregion

    #region CheckIfUserExists

    public async Task<bool> CheckIfUserExistsAsync(string userEmail)
    {
        User? user = await _usersRepository.GetAllUserAsQueryable()
                                                 .FirstOrDefaultAsync(u => u.UserEmail.ToLower() == userEmail.ToLower());
        if (user == null)
            return false;

        return true;
    }

    #endregion

    #region RegisterUser

    public async Task<bool> RegisterUserAsync(AddUserViewModel addUserViewModel)
    {
        if (addUserViewModel == null)
            return false;

        User user = new()
        {
            UserName = addUserViewModel.UserName,
            UserEmail = addUserViewModel.UserEmail,
            UserPassword = HashPassword(addUserViewModel.UserPassword),
            UserRole = addUserViewModel.UserRole,
        };

        return await _usersRepository.AddUserAsync(user);
    }

    #endregion

    #region Helper Methods

    private static string HashPassword(string password)
    {
        return Convert.ToBase64String(System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(password)));
    }

    #endregion

}

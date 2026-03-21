using CourseCraft.Repository.ViewModels;

namespace CourseCraft.Service.Interfaces;

public interface IAuthenticationService
{
    /// <summary>
    /// Authenticates a user using their email and password.
    /// </summary>
    /// <param name="email">The email of the user.</param>
    /// <param name="password">The password of the user.</param>
    /// <returns>A task that returns the authenticated user if found, otherwise null.</returns>
    Task<UserLoginViewModel?> AuthenticateUserUsingEmailPasswordAsync(string userEmail, string userPassword);

    /// <summary>
    /// Checks if a user exists by their email.
    /// </summary>
    /// <param name="email">The email of the user to check.</param>
    /// <returns>A task that returns true if the user exists, otherwise false.</returns>
    Task<bool> CheckIfUserExistsAsync(string userEmail);

    /// <summary>
    /// Adds a new user asynchronously to the database.
    /// </summary>
    /// <param name="addUserViewModel">The addUserViewModel modal to add.</param>
    /// <returns>A task that returns true if the update was successful, otherwise false.</returns>
    Task<bool> RegisterUserAsync(AddUserViewModel addUserViewModel);
}

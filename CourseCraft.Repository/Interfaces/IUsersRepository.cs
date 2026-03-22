using CourseCraft.Repository.Models;

namespace CourseCraft.Repository.Interfaces;

public interface IUsersRepository
{
    /// <summary>
    /// Retrieves all user as queryable.
    /// </summary>
    /// <returns>All user as queryable.</returns>
    IQueryable<User> GetAllUserAsQueryable();

    /// <summary>
    /// Adds a new user asynchronously to the database.
    /// </summary>
    /// <param name="user">The user entity to add.</param>
    /// <returns>A task that returns true if the update was successful, otherwise false.</returns>
    Task<bool> AddUserAsync(User user);
}

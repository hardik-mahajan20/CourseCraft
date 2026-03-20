using CourseCraft.Repository.Models;

namespace CourseCraft.Repository.Intefaces;

public interface IUsersRepository
{
    /// <summary>
    /// Retrieves all user as quearyable.
    /// </summary>
    /// <returns>All user as quearyable.</returns>
    IQueryable<User> GetAllUserAsQueryable();
}

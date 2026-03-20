using CourseCraft.Repository.Intefaces;
using CourseCraft.Repository.Models;

namespace CourseCraft.Repository.Implmentations;

public class UsersRepository(ApplicationDbContext applicationDbContext) : IUsersRepository
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

    #region GetAllUserAsQueryable

    public IQueryable<User> GetAllUserAsQueryable()
    {
        return _applicationDbContext.Users.AsQueryable();
    }

    #endregion

    #region AddUser

    public async Task<bool> AddUserAsync(User user)
    {
        await _applicationDbContext.Users.AddAsync(user);
        return await _applicationDbContext.SaveChangesAsync() > 0;
    }

    #endregion
}


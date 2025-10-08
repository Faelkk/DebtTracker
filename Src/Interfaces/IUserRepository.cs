using DebtTrack.Models;

namespace DebtTrack.Interfaces;

public interface IUserRepository
{
    Task<UserModel?>  CreateAsync(UserModel user);
    Task<UserModel?> GetByIdAsync(string userId);
    Task<IEnumerable<UserModel>> GetAllAsync();
    Task<UserModel?> GetByEmailAsync(string email);
    Task DeleteAsync(string userId);
}
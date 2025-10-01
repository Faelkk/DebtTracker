using DebtTrack.Models;

namespace DebtTrack.Interfaces;

public interface IPasswordHasher
{
    string HashPassword(UserModel user, string password);
    bool VerifyPassword(UserModel user, string password);
}
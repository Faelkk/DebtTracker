using DebtTrack.Interfaces;
using DebtTrack.Models;

namespace DebtTrack.Services;

using Microsoft.AspNetCore.Identity;

public class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<object> _hasher = new();

    public string HashPassword(UserModel user, string password)
    {
        return _hasher.HashPassword(user, password);
    }

    public bool VerifyPassword(UserModel user, string password)
    {
        var result = _hasher.VerifyHashedPassword(user, user.Password, password);
        return result == PasswordVerificationResult.Success;
    }
}
using DebtTrack.Models;

namespace DebtTrack.Interfaces;

public interface IJwtService
{
    string GenerateToken(UserModel user);
}
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using DebtTrack.Interfaces;
using DebtTrack.Models;

namespace DebtTrack.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDynamoDBContext _context;

    public UserRepository(IDynamoDBContext context)
    {
        _context = context;
    }

    public async Task<UserModel?> CreateAsync(UserModel user)
    {
        await _context.SaveAsync(user);
        return user; 
    }


    public async Task<UserModel?> GetByIdAsync(string userId)
    {
        return await _context.LoadAsync<UserModel>(userId);
    }

    public async Task<UserModel?> GetByEmailAsync(string email)
    {
        var search = _context.ScanAsync<UserModel>(
            new List<ScanCondition> { new ScanCondition("Email", ScanOperator.Equal, email) }
        );
        var result = await search.GetRemainingAsync();
        return result.FirstOrDefault();
    }


    public async Task DeleteAsync(string userId)
    {
        await _context.DeleteAsync<UserModel>(userId);
    }

    public async Task<IEnumerable<UserModel>> GetAllAsync()
    {
        var scan = _context.ScanAsync<UserModel>(new List<ScanCondition>());
        return await scan.GetRemainingAsync();
    }
}
using Amazon.DynamoDBv2.DataModel;
using DebtTrack.Interfaces;
using DebtTrack.Models;

namespace DebtTrack.Repositories;

public class DebtRepository : IDebtRepository
{
    private readonly IDynamoDBContext _context;

    public DebtRepository(IDynamoDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DebtModel>> GetAllAsync(string userId)
    {
        return await _context.QueryAsync<DebtModel>(userId).GetRemainingAsync();
    }

    public async Task<DebtModel?> GetByIdAsync(string id, string userId)
    {
        return await _context.LoadAsync<DebtModel>(userId, id);
    }

    public async Task<DebtModel> CreateAsync(DebtModel debt)
    {
        await _context.SaveAsync(debt);
        return debt;
    }

    public async Task<DebtModel?> UpdateAsync(DebtModel debt)
    {
        var existing = await _context.LoadAsync<DebtModel>(debt.UserId, debt.DebtId);
        if (existing == null) return null;

        await _context.SaveAsync(debt);
        return debt;
    }

    public async Task<bool> DeleteAsync(string id, string userId)
    {
        var existing = await _context.LoadAsync<DebtModel>(userId, id);
        if (existing == null) return false;

        await _context.DeleteAsync(existing);
        return true;
    }
}

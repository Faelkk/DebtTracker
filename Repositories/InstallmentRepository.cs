using Amazon.DynamoDBv2.DataModel;
using DebtTrack.Interfaces;
using DebtTrack.Models;

namespace DebtTrack.Repositories;

public class InstallmentRepository : IInstallmentRepository
{
    private readonly IDynamoDBContext _context;

    public InstallmentRepository(IDynamoDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<InstallmentModel>> GetAllAsync()
    {
        return await _context.ScanAsync<InstallmentModel>(new List<ScanCondition>()).GetRemainingAsync();
    }

    public async Task<InstallmentModel?> GetByIdAsync(string id)
    {
        return await _context.LoadAsync<InstallmentModel>(id);
    }

    public async Task<InstallmentModel> CreateAsync(InstallmentModel model)
    {
        await _context.SaveAsync(model);
        return model;
    }

    public async Task<InstallmentModel?> UpdateAsync(InstallmentModel model)
    {
        var existing = await _context.LoadAsync<InstallmentModel>(model.InstallmentId);
        if (existing == null) return null;

        await _context.SaveAsync(model);
        return model;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var existing = await _context.LoadAsync<InstallmentModel>(id);
        if (existing == null) return false;

        await _context.DeleteAsync(existing);
        return true;
    }
}
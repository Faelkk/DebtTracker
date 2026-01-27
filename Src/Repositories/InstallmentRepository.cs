using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
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

    public async Task<IEnumerable<InstallmentModel>> GetAllAsync(string? debtId)
{
    var conditions = new List<ScanCondition>();

    if (!string.IsNullOrEmpty(debtId))
    {
        conditions.Add(new ScanCondition(
            nameof(InstallmentModel.DebtId),
            ScanOperator.Equal,
            debtId
        ));
    }

    return await _context
        .ScanAsync<InstallmentModel>(conditions)
        .GetRemainingAsync();
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
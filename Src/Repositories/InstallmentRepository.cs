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

    public async Task<IEnumerable<InstallmentModel>> GetAllAsync(string? debtId, string userId
   )
    {
        var installments = await _context
            .QueryAsync<InstallmentModel>(userId)
            .GetRemainingAsync();

        if (!string.IsNullOrEmpty(debtId))
            installments = installments
                .Where(i => i.DebtId == debtId)
                .ToList();

        return installments;
    }


    public async Task<InstallmentModel?> GetByIdAsync(string id, string userId)
    {
        return await _context.LoadAsync<InstallmentModel>(userId, id);
    }


    public async Task<InstallmentModel> CreateAsync(InstallmentModel model)
    {
        await _context.SaveAsync(model);
        return model;
    }

    public async Task CreateManyAsync(IEnumerable<InstallmentModel> installments)
    {
        foreach (var chunk in installments.Chunk(25))
        {
            var batch = _context.CreateBatchWrite<InstallmentModel>();
            batch.AddPutItems(chunk);
            await batch.ExecuteAsync();
        }
    }

    public async Task<InstallmentModel?> UpdateAsync(InstallmentModel model)
    {
        var existing = await _context
            .LoadAsync<InstallmentModel>(model.UserId, model.InstallmentId);

        if (existing == null)
            return null;

        await _context.SaveAsync(model);
        return model;
    }


    public async Task<bool> DeleteAsync(string id, string userId)
    {
        var existing = await _context.LoadAsync<InstallmentModel>(userId, id);
        if (existing == null)
            return false;

        await _context.DeleteAsync(existing);
        return true;
    }


    public async Task DeleteManyAsync(IEnumerable<InstallmentModel> installments)
    {
        var batchWrite = _context.CreateBatchWrite<InstallmentModel>();
        foreach (var installment in installments)
        {
            batchWrite.AddDeleteItem(installment);
        }
        await batchWrite.ExecuteAsync();
    }
}
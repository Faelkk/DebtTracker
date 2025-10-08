
namespace DebtTrack.Repositories;
using Amazon.DynamoDBv2.DataModel;
using DebtTrack.Interfaces;
using DebtTrack.Models;

public class DebtRepository : IDebtRepository
    {
        private readonly IDynamoDBContext _context;

        public DebtRepository(IDynamoDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DebtModel>> GetAllAsync()
        {
            var conditions = new List<ScanCondition>();
            return await _context.ScanAsync<DebtModel>(conditions).GetRemainingAsync();
        }

        public async Task<DebtModel?> GetByIdAsync(string id)
        {
            return await _context.LoadAsync<DebtModel>(id);
        }

        public async Task<DebtModel> CreateAsync(DebtModel debt)
        {
            await _context.SaveAsync(debt);
            return debt;
        }

        public async Task<DebtModel?> UpdateAsync(DebtModel debt)
        {
            var existing = await _context.LoadAsync<DebtModel>(debt.DebtId);
            if (existing == null)
                return null;

            await _context.SaveAsync(debt);
            return debt;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var existing = await _context.LoadAsync<DebtModel>(id);
            if (existing == null)
                return false;

            await _context.DeleteAsync(existing);
            return true;
        }
    }


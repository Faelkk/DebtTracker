using DebtTrack.Models;

namespace DebtTrack.Interfaces;

public interface IDebtRepository
    {
        Task<IEnumerable<DebtModel>> GetAllAsync();
        Task<DebtModel?> GetByIdAsync(string id);
        Task<DebtModel> CreateAsync(DebtModel debt);
        Task<DebtModel?> UpdateAsync(DebtModel debt);
        Task<bool> DeleteAsync(string id);
    }

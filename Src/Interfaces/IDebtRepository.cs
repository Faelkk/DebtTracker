using DebtTrack.Models;

namespace DebtTrack.Interfaces;

public interface IDebtRepository
    {
        Task<IEnumerable<DebtModel>> GetAllAsync(string userId);
        Task<DebtModel?> GetByIdAsync(string id, string userId);
        Task<DebtModel> CreateAsync(DebtModel debt);
        Task<DebtModel?> UpdateAsync(DebtModel debt);
        Task<bool> DeleteAsync(string id,string userId);

    }

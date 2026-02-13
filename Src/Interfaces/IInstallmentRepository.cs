using DebtTrack.Models;

namespace DebtTrack.Interfaces;

public interface IInstallmentRepository
{
   Task<IEnumerable<InstallmentModel>> GetAllAsync(string? debtId, string userId);

    Task<InstallmentModel?> GetByIdAsync(string id, string userId);
    Task<InstallmentModel> CreateAsync(InstallmentModel model);

       Task CreateManyAsync(IEnumerable<InstallmentModel> installments);
    Task<InstallmentModel?> UpdateAsync(InstallmentModel model);
    Task<bool> DeleteAsync(string id,string userId);

     Task DeleteManyAsync(IEnumerable<InstallmentModel> installments);
}
using DebtTrack.Models;

namespace DebtTrack.Interfaces;

public interface IInstallmentRepository
{
    Task<IEnumerable<InstallmentModel>> GetAllAsync();
    Task<InstallmentModel?> GetByIdAsync(string id);
    Task<InstallmentModel> CreateAsync(InstallmentModel model);
    Task<InstallmentModel?> UpdateAsync(InstallmentModel model);
    Task<bool> DeleteAsync(string id);
}
using DebtTrack.Dtos.Installment;

namespace DebtTrack.Interfaces;

public interface IInstallmentService
{
    Task<IEnumerable<InstallmentDto>> GetAllAsync(string debtId, string userId);

    Task<InstallmentDto?> GetByIdAsync(string id, string userId);
    Task<InstallmentDto> CreateAsync(InstallmentCreateDto dto);
    Task<InstallmentDto?> UpdateAsync(string id, string userId, InstallmentUpdateDto dto);
    Task<bool> DeleteAsync(string id, string userId);
} 
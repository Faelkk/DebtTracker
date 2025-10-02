using DebtTrack.Dtos.Installment;

namespace DebtTrack.Interfaces;

public interface IInstallmentService
{
    Task<IEnumerable<InstallmentDto>> GetAllAsync();
    Task<InstallmentDto?> GetByIdAsync(string id);
    Task<InstallmentDto> CreateAsync(InstallmentCreateDto dto);
    Task<InstallmentDto?> UpdateAsync(string id, InstallmentUpdateDto dto);
    Task<bool> DeleteAsync(string id);
} 
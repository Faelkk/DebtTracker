using DebtTrack.Dtos.Debt;

namespace DebtTrack.Interfaces
{
    public interface IDebtService
    {
        Task<IEnumerable<DebtDto>> GetAllAsync(string userId);
        Task<DebtDto?> GetByIdAsync(string id,string? userId);
        Task<DebtDto> CreateAsync(DebtCreateDto dto,string userId);
        Task<DebtDto?> UpdateAsync(string id, string userId, DebtUpdateDto dto);
        Task<bool> Delete(string id, string userId);
    }
}


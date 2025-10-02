using DebtTrack.Dtos.Debt;

namespace DebtTrack.Interfaces
{
    public interface IDebtService
    {
        Task<IEnumerable<DebtDto>> GetAllAsync();
        Task<DebtDto?> GetByIdAsync(string id);
        Task<DebtDto> CreateAsync(DebtCreateDto dto);
        Task<DebtDto?> UpdateAsync(string id, DebtUpdateDto dto);
        Task<bool> Delete(string id);
    }
}


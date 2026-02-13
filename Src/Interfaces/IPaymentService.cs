using DebtTrack.Dtos.Payment;

namespace DebtTrack.Interfaces;

public interface IPaymentService
{
    Task<IEnumerable<PaymentDto>> GetAllAsync(string userId, string? debtId, string? installmentId);

    Task<PaymentDto?> GetByIdAsync(string id, string userId);
    Task<PaymentDto> CreateAsync(PaymentCreateDto dto, string userId);
    Task<bool> Delete(string id, string userId);
}
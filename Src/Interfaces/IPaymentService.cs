using DebtTrack.Dtos.Payment;

namespace DebtTrack.Interfaces;

public interface IPaymentService
{
    Task<IEnumerable<PaymentDto>> GetAllAsync();
    Task<PaymentDto?> GetByIdAsync(string id);
    Task<PaymentDto> CreateAsync(PaymentCreateDto dto);
    Task<bool> Delete(string id);
}
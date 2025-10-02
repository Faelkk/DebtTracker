using DebtTrack.Models;

namespace DebtTrack.Interfaces;

public interface IPaymentRepository
{
    Task<IEnumerable<PaymentModel>> GetAllAsync();
    Task<PaymentModel?> GetByIdAsync(string id);
    Task<PaymentModel> CreateAsync(PaymentModel payment);
    Task<PaymentModel?> UpdateAsync(PaymentModel payment);
    Task<bool> DeleteAsync(string id);
}
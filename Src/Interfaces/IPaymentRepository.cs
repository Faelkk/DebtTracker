using DebtTrack.Models;

namespace DebtTrack.Interfaces;

public interface IPaymentRepository
{
    Task<IEnumerable<PaymentModel>> GetAllAsync(string? debtId, string? installmentId);

    Task<PaymentModel?> GetByIdAsync(string id);
    Task<PaymentModel> CreateAsync(PaymentModel payment);
    Task<bool> DeleteAsync(string id);
}
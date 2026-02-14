using DebtTrack.Models;

namespace DebtTrack.Interfaces;

public interface IPaymentRepository
{
    Task<IEnumerable<PaymentModel>> GetAllAsync(string userId,string? debtId, string? installmentId);

    Task<PaymentModel?> GetByIdAsync(string id,string userId,string? debtId, string? installmentId);
    Task<PaymentModel> CreateAsync(PaymentModel payment);
    Task<bool> DeleteAsync(string id,string userId);

     Task DeleteManyAsync(IEnumerable<PaymentModel> payments);
}
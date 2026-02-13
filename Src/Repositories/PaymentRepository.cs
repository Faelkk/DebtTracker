using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using DebtTrack.Interfaces;
using DebtTrack.Models;

namespace DebtTrack.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IDynamoDBContext _context;

        public PaymentRepository(IDynamoDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PaymentModel>> GetAllAsync(
     string userId,
     string? debtId,
     string? installmentId)
        {
            var payments = await _context
                .QueryAsync<PaymentModel>(userId)
                .GetRemainingAsync();

            if (!string.IsNullOrEmpty(debtId))
                payments = payments.Where(p => p.DebtId == debtId).ToList();

            if (!string.IsNullOrEmpty(installmentId))
                payments = payments.Where(p => p.InstallmentId == installmentId).ToList();

            return payments;
        }

        public async Task<PaymentModel?> GetByIdAsync(string id, string userId)
        {
            return await _context.LoadAsync<PaymentModel>(userId, id);
        }
        public async Task<PaymentModel> CreateAsync(PaymentModel payment)
        {
            await _context.SaveAsync(payment);
            return payment;
        }


        public async Task<bool> DeleteAsync(string id, string userId)
        {
            var existing = await _context.LoadAsync<PaymentModel>(userId, id);
            if (existing == null)
                return false;

            await _context.DeleteAsync(existing);
            return true;
        }


        public async Task DeleteManyAsync(IEnumerable<PaymentModel> payments)
        {
            var batch = _context.CreateBatchWrite<PaymentModel>();

            foreach (var payment in payments)
            {
                batch.AddDeleteItem(payment);
            }

            await batch.ExecuteAsync();
        }

    }

}




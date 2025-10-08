using Amazon.DynamoDBv2.DataModel;
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

        public async Task<IEnumerable<PaymentModel>> GetAllAsync()
        {
            var conditions = new List<ScanCondition>();
            return await _context.ScanAsync<PaymentModel>(conditions).GetRemainingAsync();
        }

        public async Task<PaymentModel?> GetByIdAsync(string id)
        {
            return await _context.LoadAsync<PaymentModel>(id);
        }

        public async Task<PaymentModel> CreateAsync(PaymentModel payment)
        {
            await _context.SaveAsync(payment);
            return payment;
        }
        

        public async Task<bool> DeleteAsync(string id)
        {
            var existing = await _context.LoadAsync<PaymentModel>(id);
            if (existing == null) return false;

            await _context.DeleteAsync(existing);
            return true;
        }
    }
}
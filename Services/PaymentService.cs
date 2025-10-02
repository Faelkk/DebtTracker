

namespace DebtTrack.Services;
using DebtTrack.Dtos.Payment;
using DebtTrack.Interfaces;
using DebtTrack.Models;


public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<IEnumerable<PaymentDto>> GetAllAsync()
        {
            var payments = await _paymentRepository.GetAllAsync();
            return payments.Select(p => new PaymentDto
            {
                PaymentId = p.PaymentId,
                DebtId = p.DebtId,
                InstallmentId = p.InstallmentId,
                Amount = p.Amount,
                PaidAt = p.PaidAt
            });
        }

        public async Task<PaymentDto?> GetByIdAsync(string id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            if (payment == null) return null;

            return new PaymentDto
            {
                PaymentId = payment.PaymentId,
                DebtId = payment.DebtId,
                InstallmentId = payment.InstallmentId,
                Amount = payment.Amount,
                PaidAt = payment.PaidAt
            };
        }

        public async Task<PaymentDto> CreateAsync(PaymentCreateDto dto)
        {
            var model = new PaymentModel
            {
                DebtId = dto.DebtId,
                InstallmentId = dto.InstallmentId,
                Amount = dto.Amount
            };

            var created = await _paymentRepository.CreateAsync(model);

            return new PaymentDto
            {
                PaymentId = created.PaymentId,
                DebtId = created.DebtId,
                InstallmentId = created.InstallmentId,
                Amount = created.Amount,
                PaidAt = created.PaidAt
            };
        }

        public async Task<PaymentDto?> UpdateAsync(string id, PaymentUpdateDto dto)
        {
            var existing = await _paymentRepository.GetByIdAsync(id);
            if (existing == null) return null;

            if (dto.Amount.HasValue) existing.Amount = dto.Amount.Value;
            if (dto.PaidAt.HasValue) existing.PaidAt = dto.PaidAt.Value;

            var updated = await _paymentRepository.UpdateAsync(existing);
            if (updated == null) return null;

            return new PaymentDto
            {
                PaymentId = updated.PaymentId,
                DebtId = updated.DebtId,
                InstallmentId = updated.InstallmentId,
                Amount = updated.Amount,
                PaidAt = updated.PaidAt
            };
        }

        public async Task<bool> Delete(string id)
        {
            return await _paymentRepository.DeleteAsync(id);
        }
    }


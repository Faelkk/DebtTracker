

namespace DebtTrack.Services;
using DebtTrack.Dtos.Payment;
using DebtTrack.Interfaces;
using DebtTrack.Models;


public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IInstallmentRepository _installmentRepository;
        public PaymentService(IPaymentRepository paymentRepository,IInstallmentRepository installmentRepository)
        {
            _paymentRepository = paymentRepository;
            _installmentRepository = installmentRepository;
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


            var installment = await _installmentRepository.GetByIdAsync(dto.InstallmentId);
            if (installment != null)
            {
                installment.PaidAmount += dto.Amount;
                installment.IsPaid = installment.PaidAmount >= installment.Amount;
                await _installmentRepository.UpdateAsync(installment);
            }

            return new PaymentDto
            {
                PaymentId = created.PaymentId,
                DebtId = created.DebtId,
                InstallmentId = created.InstallmentId,
                Amount = created.Amount,
                PaidAt = created.PaidAt
            };
        }

        

        public async Task<bool> Delete(string id)
        {
            return await _paymentRepository.DeleteAsync(id);
        }
    }




namespace DebtTrack.Services;

using DebtTrack.Dtos.Payment;
using DebtTrack.Interfaces;
using DebtTrack.Models;


public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IInstallmentRepository _installmentRepository;
    private readonly IDebtRepository _debtRepository;
    public PaymentService(IPaymentRepository paymentRepository, IInstallmentRepository installmentRepository, IDebtRepository debtRepository)
    {
        _paymentRepository = paymentRepository;
        _installmentRepository = installmentRepository;
        _debtRepository = debtRepository;
    }

public async Task<IEnumerable<PaymentDto>> GetAllAsync(
    string userId,
    string debtId,
    string installmentId
)
{
    var payments = await _paymentRepository.GetAllAsync(userId, debtId, installmentId);


    Console.WriteLine($"Service Payments Count: {payments.Count()} for UserId: {userId}, DebtId: {debtId}, InstallmentId: {installmentId}"); // Debug log

    return payments.Select(p => new PaymentDto
    {
        PaymentId = p.PaymentId,
        DebtId = p.DebtId,
        InstallmentId = p.InstallmentId,
        Amount = p.Amount,
        PaidAt = p.PaidAt,
        UserId = p.UserId,
    });
}


    public async Task<PaymentDto?> GetByIdAsync(string id,string userId,string debtId, string installmentId)
    {
        var payment = await _paymentRepository.GetByIdAsync(id,userId,debtId,installmentId);
        if (payment == null) return null;

        return new PaymentDto
        {
            PaymentId = payment.PaymentId,
            DebtId = payment.DebtId,
            InstallmentId = payment.InstallmentId,
            Amount = payment.Amount,
            PaidAt = payment.PaidAt,
            UserId = payment.UserId

        };
    }

    public async Task<PaymentDto> CreateAsync(PaymentCreateDto dto,string userId)
    {
         var debt = await _debtRepository.GetByIdAsync(dto.DebtId, userId);
         if (debt == null)
        throw new KeyNotFoundException("Divida nao encontrada.");

         var installment = await _installmentRepository.GetByIdAsync(dto.InstallmentId, userId);
         if (installment == null)
        throw new KeyNotFoundException("Parcela não encontrada.");

        var model = new PaymentModel
        {
            DebtId = dto.DebtId,
            InstallmentId = dto.InstallmentId,
            Amount = dto.Amount,UserId = userId

        };

        var created = await _paymentRepository.CreateAsync(model);



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
            PaidAt = created.PaidAt,
            UserId = created.UserId
        };
    }



  public async Task<bool> Delete(string id, string userId)
{
    
    var payment = await _paymentRepository.GetByIdAsync(id, userId,null,null);
    if (payment == null)
        return false;


    var installment = await _installmentRepository.GetByIdAsync(payment.InstallmentId, userId);
    if (installment != null)
    {
        installment.PaidAmount -= payment.Amount;
        if (installment.PaidAmount < 0) installment.PaidAmount = 0; 
        installment.IsPaid = installment.PaidAmount >= installment.Amount;
        await _installmentRepository.UpdateAsync(installment);
    }

    return await _paymentRepository.DeleteAsync(id, userId);
}
}


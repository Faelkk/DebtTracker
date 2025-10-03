using DebtTrack.Dtos.Debt;
using DebtTrack.Interfaces;
using DebtTrack.Models;

namespace DebtTrack.Services;

public class DebtService : IDebtService
{
    private readonly IDebtRepository _debtRepository;
    private readonly IInstallmentRepository _installmentRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUserRepository _userRepository;
    public DebtService(
        IDebtRepository debtRepository,
        IInstallmentRepository installmentRepository,
        IPaymentRepository paymentRepository,IUserRepository userRepository)
    {
        _debtRepository = debtRepository;
        _installmentRepository = installmentRepository;
        _paymentRepository = paymentRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<DebtDto>> GetAllAsync()
    {
        var debts = await _debtRepository.GetAllAsync();
        return debts.Select(d => new DebtDto
        {
            DebtId = d.DebtId,
            DebtorId = d.DebtorId,
            CreditorId = d.CreditorId,
            Description = d.Description,
            TotalAmount = d.TotalAmount,
            Installments = d.Installments,
            InstallmentValue = d.InstallmentValue,
            CreatedAt = d.CreatedAt,
            DueDate = d.DueDate,
            DebtorConfirmedPayment = d.DebtorConfirmedPayment,
            CreditorConfirmedReceipt = d.CreditorConfirmedReceipt,
            IsPaid = d.IsPaid
        });
    }

    public async Task<DebtDto?> GetByIdAsync(string id)
    {
        var debt = await _debtRepository.GetByIdAsync(id);
        if (debt == null) return null;

        return new DebtDto
        {
            DebtId = debt.DebtId,
            DebtorId = debt.DebtorId,
            CreditorId = debt.CreditorId,
            Description = debt.Description,
            TotalAmount = debt.TotalAmount,
            Installments = debt.Installments,
            InstallmentValue = debt.InstallmentValue,
            CreatedAt = debt.CreatedAt,
            DueDate = debt.DueDate,
            DebtorConfirmedPayment = debt.DebtorConfirmedPayment,
            CreditorConfirmedReceipt = debt.CreditorConfirmedReceipt,
            IsPaid = debt.IsPaid
        };
    }

    public async Task<DebtDto> CreateAsync(DebtCreateDto dto)
    {
        var debtor = await _userRepository.GetByIdAsync(dto.DebtorId);
        var creditor = await _userRepository.GetByIdAsync(dto.CreditorId);

        if (debtor == null || creditor == null)
            throw new Exception("Debtor or creditor user not found");
        
        var installmentValue = dto.TotalAmount / dto.Installments;

        var model = new DebtModel
        {
            DebtorId = dto.DebtorId,
            CreditorId = dto.CreditorId,
            Description = dto.Description,
            TotalAmount = dto.TotalAmount,
            Installments = dto.Installments,
            InstallmentValue = installmentValue,
            DueDate = dto.DueDate
        };

        var created = await _debtRepository.CreateAsync(model);
        
        for (int i = 1; i <= created.Installments; i++)
        {
            var installment = new InstallmentModel
            {
                DebtId = created.DebtId,
                Number = i,
                Amount = created.InstallmentValue,
                DueDate = created.DueDate.AddMonths(i - 1),
            };
            await _installmentRepository.CreateAsync(installment);
        }

        return new DebtDto
        {
            DebtId = created.DebtId,
            DebtorId = created.DebtorId,
            CreditorId = created.CreditorId,
            Description = created.Description,
            TotalAmount = created.TotalAmount,
            Installments = created.Installments,
            InstallmentValue = created.InstallmentValue,
            CreatedAt = created.CreatedAt,
            DueDate = created.DueDate,
            DebtorConfirmedPayment = created.DebtorConfirmedPayment,
            CreditorConfirmedReceipt = created.CreditorConfirmedReceipt,
            IsPaid = created.IsPaid
        };
    }

    public async Task<DebtDto?> UpdateAsync(string id, DebtUpdateDto dto)
    {
        var existing = await _debtRepository.GetByIdAsync(id);
        if (existing == null) return null;

        if (!string.IsNullOrEmpty(dto.Description))
            existing.Description = dto.Description;

        if (dto.DebtorConfirmedPayment.HasValue)
            existing.DebtorConfirmedPayment = dto.DebtorConfirmedPayment.Value;

        if (dto.CreditorConfirmedReceipt.HasValue)
            existing.CreditorConfirmedReceipt = dto.CreditorConfirmedReceipt.Value;


        if (existing.DebtorConfirmedPayment && existing.CreditorConfirmedReceipt)
            existing.IsPaid = true;

        var updated = await _debtRepository.UpdateAsync(existing);
        if (updated == null) return null;

        return new DebtDto
        {
            DebtId = updated.DebtId,
            DebtorId = updated.DebtorId,
            CreditorId = updated.CreditorId,
            Description = updated.Description,
            TotalAmount = updated.TotalAmount,
            Installments = updated.Installments,
            InstallmentValue = updated.InstallmentValue,
            CreatedAt = updated.CreatedAt,
            DueDate = updated.DueDate,
            DebtorConfirmedPayment = updated.DebtorConfirmedPayment,
            CreditorConfirmedReceipt = updated.CreditorConfirmedReceipt,
            IsPaid = updated.IsPaid
        };
    }


    public async Task<bool> Delete(string id)
    {

        var existing = await _debtRepository.GetByIdAsync(id);
        if (existing == null) return false;
      
        var allPayments = await _paymentRepository.GetAllAsync();
        var paymentsToDelete = allPayments.Where(p => p.DebtId == id);
        foreach (var p in paymentsToDelete)
        {
            await _paymentRepository.DeleteAsync(p.PaymentId);
        }


        var allInstallments = await _installmentRepository.GetAllAsync();
        var installmentsToDelete = allInstallments.Where(i => i.DebtId == id);
        foreach (var i in installmentsToDelete)
        {
            await _installmentRepository.DeleteAsync(i.InstallmentId);
        }

     
        return await _debtRepository.DeleteAsync(id);
    }
}

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
        IsMyDebt = d.IsMyDebt,
        InvolvedPartyName = d.InvolvedPartyName,
        Description = d.Description,
        TotalAmount = d.TotalAmount,
        Installments = d.Installments,
        InstallmentValue = d.InstallmentValue,
        CreatedAt = d.CreatedAt,
        DueDate = d.DueDate,
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
        IsMyDebt = debt.IsMyDebt,
        InvolvedPartyName = debt.InvolvedPartyName,
        Description = debt.Description,
        TotalAmount = debt.TotalAmount,
        Installments = debt.Installments,
        InstallmentValue = debt.InstallmentValue,
        CreatedAt = debt.CreatedAt,
        DueDate = debt.DueDate,
        IsPaid = debt.IsPaid
    };
}


    public async Task<DebtDto> CreateAsync(DebtCreateDto dto)
{
    var installmentValue = dto.TotalAmount / dto.Installments;

    var model = new DebtModel
    {
        IsMyDebt = dto.IsMyDebt,
        InvolvedPartyName = dto.InvolvedPartyName,
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
        IsMyDebt = created.IsMyDebt,
        InvolvedPartyName = created.InvolvedPartyName,
        Description = created.Description,
        TotalAmount = created.TotalAmount,
        Installments = created.Installments,
        InstallmentValue = created.InstallmentValue,
        CreatedAt = created.CreatedAt,
        DueDate = created.DueDate,
        IsPaid = created.IsPaid
    };
}


   public async Task<DebtDto?> UpdateAsync(string id, DebtUpdateDto dto)
{
    var existing = await _debtRepository.GetByIdAsync(id);
    if (existing == null) return null;

    if (!string.IsNullOrEmpty(dto.Description))
        existing.Description = dto.Description;

    existing.IsPaid = dto.IsPaid;

    var updated = await _debtRepository.UpdateAsync(existing);
    if (updated == null) return null;

    return new DebtDto
    {
        DebtId = updated.DebtId,
        IsMyDebt = updated.IsMyDebt,
        InvolvedPartyName = updated.InvolvedPartyName,
        Description = updated.Description,
        TotalAmount = updated.TotalAmount,
        Installments = updated.Installments,
        InstallmentValue = updated.InstallmentValue,
        CreatedAt = updated.CreatedAt,
        DueDate = updated.DueDate,
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

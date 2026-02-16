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
        IPaymentRepository paymentRepository, IUserRepository userRepository)
    {
        _debtRepository = debtRepository;
        _installmentRepository = installmentRepository;
        _paymentRepository = paymentRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<DebtDto>> GetAllAsync(string userId)
{
    var debts = await _debtRepository.GetAllAsync(userId);

    return debts.Select(d => new DebtDto
    {
        DebtId = d.DebtId,
        IsMyDebt = d.IsMyDebt,
        CreditorName = d.CreditorName,
        DebtorName = d.DebtorName,
        Description = d.Description,
        TotalAmount = d.TotalAmount,
        Installments = d.Installments,
        InstallmentValue = d.InstallmentValue,
        CreatedAt = d.CreatedAt,
        DueDate = d.DueDate,
        IsPaid = d.IsPaid,
        UserId = d.UserId
    });
}



    public async Task<DebtDto?> GetByIdAsync(string id,string? userId)
    {
        var debt = await _debtRepository.GetByIdAsync(id,userId);
        if (debt == null) return null;

        return new DebtDto
        {
            DebtId = debt.DebtId,
            IsMyDebt = debt.IsMyDebt,
            CreditorName = debt.CreditorName,
            DebtorName = debt.DebtorName,
            Description = debt.Description,
            TotalAmount = debt.TotalAmount,
            Installments = debt.Installments,
            InstallmentValue = debt.InstallmentValue,
            CreatedAt = debt.CreatedAt,
            DueDate = debt.DueDate,
            IsPaid = debt.IsPaid,
            UserId = debt.UserId
        };
    }


    public async Task<DebtDto> CreateAsync(DebtCreateDto dto, string userId)
    {
        var installmentValue = dto.TotalAmount / dto.Installments;

        var model = new DebtModel
        {
            IsMyDebt = dto.IsMyDebt,
            CreditorName = dto.CreditorName,
            DebtorName = dto.DebtorName,
            Description = dto.Description,
            TotalAmount = dto.TotalAmount,
            Installments = dto.Installments,
            InstallmentValue = installmentValue,
            DueDate = dto.DueDate,
            UserId = userId

        };

        var created = await _debtRepository.CreateAsync(model);

        Console.WriteLine($"Debt created with ID: {created.DebtId}");

        var installments = GenerateInstallments(created);

        await _installmentRepository.CreateManyAsync(installments);

        Console.WriteLine($"Generated {installments.Count} installments for Debt ID: {created.DebtId}");


        return new DebtDto
        {
            DebtId = created.DebtId,
            IsMyDebt = created.IsMyDebt,
            CreditorName = created.CreditorName,
            DebtorName = created.DebtorName,
            Description = created.Description,
            TotalAmount = created.TotalAmount,
            Installments = created.Installments,
            InstallmentValue = created.InstallmentValue,
            CreatedAt = created.CreatedAt,
            DueDate = created.DueDate,
            IsPaid = created.IsPaid,
            UserId = created.UserId
        };
    }


    public async Task<DebtDto?> UpdateAsync(string id, string userId, DebtUpdateDto dto)
    {
        var existing = await _debtRepository.GetByIdAsync(id, userId);
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
            CreditorName = updated.CreditorName,
            DebtorName = updated.DebtorName,
            Description = updated.Description,
            TotalAmount = updated.TotalAmount,
            Installments = updated.Installments,
            InstallmentValue = updated.InstallmentValue,
            CreatedAt = updated.CreatedAt,
            DueDate = updated.DueDate,
            IsPaid = updated.IsPaid,
            UserId = updated.UserId
        };
    }



    public async Task<bool> Delete(string id, string userId)
    {
        var existing = await _debtRepository.GetByIdAsync(id, userId);
        if (existing == null) return false;

        var paymentsToDelete = await _paymentRepository.GetAllAsync(id, userId,null);

        if (paymentsToDelete.Any())
            await _paymentRepository.DeleteManyAsync(paymentsToDelete);

        var installmentsToDelete = await _installmentRepository.GetAllAsync(id,userId);

        if (installmentsToDelete.Any())
            await _installmentRepository.DeleteManyAsync(installmentsToDelete);

        return await _debtRepository.DeleteAsync(id,userId);
    }

    private List<InstallmentModel> GenerateInstallments(DebtModel debt)
    {
        var installments = new List<InstallmentModel>();

        for (int i = 1; i <= debt.Installments; i++)
        {
            installments.Add(new InstallmentModel
            {
                DebtId = debt.DebtId,
                Number = i,
                Amount = debt.InstallmentValue,
                DueDate = debt.DueDate.AddMonths(i - 1),
                UserId = debt.UserId
            });
        }

        return installments;
    }

}



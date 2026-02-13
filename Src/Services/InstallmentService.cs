using DebtTrack.Dtos.Installment;
using DebtTrack.Interfaces;
using DebtTrack.Models;

namespace DebtTrack.Services;

public class InstallmentService : IInstallmentService
{
    private readonly IInstallmentRepository _repository;
    private readonly IPaymentRepository _paymentRepository;

    public InstallmentService(
        IInstallmentRepository repository,
        IPaymentRepository paymentRepository)
    {
        _repository = repository;
        _paymentRepository = paymentRepository;
    }

    public async Task<IEnumerable<InstallmentDto>> GetAllAsync(string? debtId, string userId)
{
    var models = await _repository.GetAllAsync(debtId, userId);

    return models.Select(m => new InstallmentDto
    {
        InstallmentId = m.InstallmentId,
        DebtId = m.DebtId,
        Number = m.Number,
        DueDate = m.DueDate,
        Amount = m.Amount,
        PaidAmount = m.PaidAmount,
        IsPaid = m.IsPaid,
        UserId = m.UserId
        
    });
}


    public async Task<InstallmentDto?> GetByIdAsync(string id, string userId)
    {
        var model = await _repository.GetByIdAsync(id, userId);
        if (model == null) return null;

        return new InstallmentDto
        {
            InstallmentId = model.InstallmentId,
            DebtId = model.DebtId,
            Number = model.Number,
            DueDate = model.DueDate,
            Amount = model.Amount,
            PaidAmount = model.PaidAmount,
            IsPaid = model.IsPaid,
            UserId = model.UserId
        };
    }

    public async Task<InstallmentDto> CreateAsync(InstallmentCreateDto dto)
    {
        var model = new InstallmentModel
        {
            DebtId = dto.DebtId,
            Number = dto.Number,
            DueDate = dto.DueDate,
            Amount = dto.Amount,
            UserId = dto.UserId

        };

        var created = await _repository.CreateAsync(model);

        return new InstallmentDto
        {
            InstallmentId = created.InstallmentId,
            DebtId = created.DebtId,
            Number = created.Number,
            DueDate = created.DueDate,
            Amount = created.Amount,
            PaidAmount = created.PaidAmount,
            IsPaid = created.IsPaid
        };
    }

    public async Task<InstallmentDto?> UpdateAsync(string id, string userId, InstallmentUpdateDto dto)
    {
        var existing = await _repository.GetByIdAsync(id, userId);
        if (existing == null) return null;
        
        existing.Amount = dto.Amount;
        existing.PaidAmount = dto.PaidAmount;
        existing.IsPaid = dto.IsPaid;

        var updated = await _repository.UpdateAsync(existing);

        return new InstallmentDto
        {
            InstallmentId = updated!.InstallmentId,
            DebtId = updated.DebtId,
            Number = updated.Number,
            DueDate = updated.DueDate,
            Amount = updated.Amount,
            PaidAmount = updated.PaidAmount,
            IsPaid = updated.IsPaid,
            UserId = updated.UserId
        };
    }

    public async Task<bool> DeleteAsync(string id, string userId)
    {
        var existing = await _repository.GetByIdAsync(id, userId);
        if (existing == null) return false;
        
       var paymentsToDelete = await _paymentRepository.GetAllAsync(userId, id,null);

       await _paymentRepository.DeleteManyAsync(paymentsToDelete);


        return await _repository.DeleteAsync(id,userId);
    }
}



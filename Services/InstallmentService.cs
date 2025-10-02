using DebtTrack.Dtos.Installment;
using DebtTrack.Interfaces;
using DebtTrack.Models;

namespace DebtTrack.Services;

public class InstallmentService : IInstallmentService
{
    private readonly IInstallmentRepository _repository;

    public InstallmentService(IInstallmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<InstallmentDto>> GetAllAsync()
    {
        var models = await _repository.GetAllAsync();
        return models.Select(m => new InstallmentDto
        {
            InstallmentId = m.InstallmentId,
            DebtId = m.DebtId,
            Number = m.Number,
            DueDate = m.DueDate,
            Amount = m.Amount,
            PaidAmount = m.PaidAmount,
            IsPaid = m.IsPaid
        });
    }

    public async Task<InstallmentDto?> GetByIdAsync(string id)
    {
        var model = await _repository.GetByIdAsync(id);
        if (model == null) return null;

        return new InstallmentDto
        {
            InstallmentId = model.InstallmentId,
            DebtId = model.DebtId,
            Number = model.Number,
            DueDate = model.DueDate,
            Amount = model.Amount,
            PaidAmount = model.PaidAmount,
            IsPaid = model.IsPaid
        };
    }

    public async Task<InstallmentDto> CreateAsync(InstallmentCreateDto dto)
    {
        var model = new InstallmentModel
        {
            DebtId = dto.DebtId,
            Number = dto.Number,
            DueDate = dto.DueDate,
            Amount = dto.Amount
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

    public async Task<InstallmentDto?> UpdateAsync(string id, InstallmentUpdateDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return null;

        existing.DueDate = dto.DueDate;
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
            IsPaid = updated.IsPaid
        };
    }

    public async Task<bool> DeleteAsync(string id)
    {
        return await _repository.DeleteAsync(id);
    }
}

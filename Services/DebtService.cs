using DebtTrack.Dtos.Debt;
using DebtTrack.Interfaces;
using DebtTrack.Models;

namespace DebtTrack.Services;

public class DebtService : IDebtService
    {
        private readonly IDebtRepository _debtRepository;

        public DebtService(IDebtRepository debtRepository)
        {
            _debtRepository = debtRepository;
        }

        public async Task<IEnumerable<DebtDto>> GetAllAsync()
        {
            var debts = await _debtRepository.GetAllAsync();
            return debts.Select(d => new DebtDto
            {
                DebtId = d.DebtId,
                UserId = d.UserId,
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
                UserId = debt.UserId,
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
                UserId = dto.UserId,
                Description = dto.Description,
                TotalAmount = dto.TotalAmount,
                Installments = dto.Installments,
                InstallmentValue = installmentValue,
                DueDate = dto.DueDate
            };

            var created = await _debtRepository.CreateAsync(model);

            return new DebtDto
            {
                DebtId = created.DebtId,
                UserId = created.UserId,
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

            if (dto.TotalAmount.HasValue)
                existing.TotalAmount = dto.TotalAmount.Value;

            if (dto.Installments.HasValue)
                existing.Installments = dto.Installments.Value;
            
            if (dto.TotalAmount.HasValue || dto.Installments.HasValue)
                existing.InstallmentValue = existing.TotalAmount / existing.Installments;

            if (dto.DueDate.HasValue)
                existing.DueDate = dto.DueDate.Value;

            if (dto.IsPaid.HasValue)
                existing.IsPaid = dto.IsPaid.Value;

            var updated = await _debtRepository.UpdateAsync(existing);
            if (updated == null) return null;

            return new DebtDto
            {
                DebtId = updated.DebtId,
                UserId = updated.UserId,
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
            return await _debtRepository.DeleteAsync(id);
        }
    }


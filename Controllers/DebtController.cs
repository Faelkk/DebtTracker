using DebtTrack.Dtos.Debt;
using DebtTrack.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DebtTrack.Controllers;

[ApiController]
[Route("[controller]")]
public class DebtController : ControllerBase
{
    private readonly IDebtService _debtService;

    public DebtController(IDebtService debtService)
    {
        _debtService = debtService;
    }

    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var debts = await _debtService.GetAllAsync();
            return Ok(debts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var debt = await _debtService.GetByIdAsync(id);
            if (debt == null)
                return NotFound("Dívida não encontrada");

            return Ok(debt);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] DebtCreateDto debtCreateDto)
    {
        try
        {
            var createdDebt = await _debtService.CreateAsync(debtCreateDto);
            return CreatedAtAction(nameof(GetById), new { id = createdDebt.DebtId }, createdDebt);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(string id, [FromBody] DebtUpdateDto debtUpdateDto)
    {
        try
        {
            var updatedDebt = await _debtService.UpdateAsync(id, debtUpdateDto);
            if (updatedDebt == null)
                return NotFound("Dívida não encontrada");

            return Ok(updatedDebt);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var deleted = await _debtService.Delete(id);
            if (!deleted)
                return NotFound("Dívida não encontrada");

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}

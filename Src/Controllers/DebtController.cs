using DebtTrack.Dtos.Debt;
using DebtTrack.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DebtTrack.Shared;

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
          var userId = User.GetUserId();
        if (string.IsNullOrEmpty(userId))
            return BadRequest("Usuário não autenticado.");

            var debts = await _debtService.GetAllAsync(userId!);

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
       var userId = User.GetUserId();
        if (string.IsNullOrEmpty(userId))
            return BadRequest("Usuário não autenticado.");

        if (string.IsNullOrEmpty(id))
            return BadRequest("DebtId é obrigatório.");

        var debt = await _debtService.GetByIdAsync(id, userId);

        if (debt == null)
            return NotFound("Dívida não encontrada");

        return Ok(debt);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] DebtCreateDto debtCreateDto)
    {
        try
        {
             var userId = User.GetUserId();
        if (string.IsNullOrEmpty(userId))
            return BadRequest("Usuário não autenticado.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);


            Console.WriteLine($"UserId in controller: {userId}");

            var createdDebt = await _debtService.CreateAsync(debtCreateDto, userId!);
            return CreatedAtAction(nameof(GetById), new { id = createdDebt.DebtId }, createdDebt);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [Authorize]
    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(string id, [FromBody] DebtUpdateDto debtUpdateDto)
    {
        try
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Usuário não autenticado.");

            if (string.IsNullOrEmpty(id))
                return BadRequest("DebtId é obrigatório.");




            var updatedDebt = await _debtService.UpdateAsync(id, userId!, debtUpdateDto);
            if (updatedDebt == null)
                return NotFound("Dívida não encontrada");

            return Ok(updatedDebt);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Usuário não autenticado.");

            if (string.IsNullOrEmpty(id))
                return BadRequest("DebtId é obrigatório.");


            var deleted = await _debtService.Delete(id, userId);
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

using DebtTrack.Dtos.Payment;
using DebtTrack.Interfaces;
using DebtTrack.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DebtTrack.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string debtId, [FromQuery] string installmentId)
    {
        try
        {

            if(string.IsNullOrEmpty(debtId) && string.IsNullOrEmpty(installmentId))
                return BadRequest(new { message = "At least one of debtId or installmentId must be provided." });
            

            var userId = User.GetUserId();
            var payments = await _paymentService.GetAllAsync(userId, debtId, installmentId);
            return Ok(payments);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id,[FromQuery] string debtId, [FromQuery] string installmentId)
    {
        try
        {

               if(string.IsNullOrEmpty(debtId) && string.IsNullOrEmpty(installmentId))
                return BadRequest(new { message = "At least one of debtId or installmentId must be provided." });
            
            var userId = User.GetUserId();
            var payment = await _paymentService.GetByIdAsync(id, userId, debtId, installmentId);
            if (payment == null) return NotFound(new { message = "Payment not found" });
            return Ok(payment);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PaymentCreateDto dto)
    {
        try
        {
            var userId = User.GetUserId();
            var created = await _paymentService.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = created.PaymentId }, created);
        }
        catch (KeyNotFoundException knfEx)
        {
            return BadRequest(new { message = knfEx.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var userId = User.GetUserId();
            var deleted = await _paymentService.Delete(id, userId);
            if (!deleted) return NotFound(new { message = "Payment not found" });
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}

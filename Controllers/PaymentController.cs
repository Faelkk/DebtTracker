using DebtTrack.Dtos.Payment;
using DebtTrack.Interfaces;
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
    public async Task<IActionResult> Get()
    {
        var payments = await _paymentService.GetAllAsync();
        return Ok(payments);
    }

    
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var payment = await _paymentService.GetByIdAsync(id);
        if (payment == null) return NotFound("Payment not found");
        return Ok(payment);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PaymentCreateDto dto)
    {
        var created = await _paymentService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.PaymentId }, created);
    }

    
    
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var deleted = await _paymentService.Delete(id);
        if (!deleted) return NotFound("Payment not found");
        return NoContent();
    }
}
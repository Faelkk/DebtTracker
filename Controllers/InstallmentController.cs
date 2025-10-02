using DebtTrack.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DebtTrack.Controllers;

[ApiController]
[Route("[controller]")]
public class InstallmentController : ControllerBase
{

    private readonly IInstallmentService  _installmentService;

    public InstallmentController(IInstallmentService installmentService)
    {
        this._installmentService = installmentService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var installments = await _installmentService.GetAllAsync();
            return Ok(installments);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{id}")]

    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var installment = await _installmentService.GetByIdAsync(id);
            if (installment == null)
                return NotFound("Emprestimo não encontrado");

            return Ok(installment);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
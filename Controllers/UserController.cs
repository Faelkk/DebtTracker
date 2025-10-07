using DebtTrack.Dtos.User;
using DebtTrack.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DebtTrack.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult>  Get()
    {
        try
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult>  Get(Guid id)
    {
        try
        {
            var user = await _userService.GetById(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("create")]
    public async Task<IActionResult>  Signup([FromBody] UserCreateDto userCreateDto)
    {
        try
        {
            var createdUser = await _userService.Create(userCreateDto);
            return Ok(new { token = createdUser.Token });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    
    [HttpPost("login")]
    public  async Task<IActionResult> Signin([FromBody] UserLoginDto userLoginDto)
    {
        try
        {
            var createdUser = await _userService.Login(userLoginDto);
            return Ok(new { token = createdUser.Token });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var deleted = await _userService.Delete(id);

            if (!deleted)
                return NotFound("Usuário não encontrado");

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }


}
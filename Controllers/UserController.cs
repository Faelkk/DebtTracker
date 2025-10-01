using DebtTrack.Dtos.User;
using DebtTrack.Interfaces;
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

    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        try
        {
            var user = _userService.GetById(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("{/create}")]
    public IActionResult Signup([FromBody] UserCreateDto userCreateDto)
    {
        try
        {
            var createdUser = _userService.Create(userCreateDto);
            return CreatedAtAction(nameof(Get), new { id = createdUser.Id }, createdUser);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPost("{/login}")]
    public IActionResult Signin([FromBody] UserLoginDto userLoginDto)
    {
        try
        {
            var createdUser = _userService.Login(userLoginDto);
            return CreatedAtAction(nameof(Get), new { id = createdUser.Id }, createdUser);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

 
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
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
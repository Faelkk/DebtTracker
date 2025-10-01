using DebtTrack.Dtos.User;
using DebtTrack.Interfaces;
using DebtTrack.Models;

namespace DebtTrack.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher,IJwtService jwtService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

   public async Task<IEnumerable<UserDto>> GetAll()
   {
       var users = await _userRepository.GetAllAsync();

    
       return users.Select(u => new UserDto
       {
           UserId = u.UserId,
           Name = u.Name,
           Email = u.Email
       });
   }
   public async Task<UserDto> GetById(int id)
   {
       var user = await _userRepository.GetByIdAsync(id.ToString());
       if (user == null)
           throw new Exception("Usuário não encontrado");

       return new UserDto
       {
           UserId = user.UserId,
           Name = user.Name,
           Email = user.Email
       };
   }

   public async Task<UserTokenDto> Create(UserCreateDto userDto)
   {
       var existingUser = await _userRepository.GetByEmailAsync(userDto.Email);
       if (existingUser is not null)
           throw new Exception("E-mail já cadastrado.");

       var id = Guid.NewGuid();

       var hashedPassword = _passwordHasher.HashPassword(null!, userDto.Password);

       var user = new UserModel
       {
           UserId = id.ToString(),
           Name = userDto.Name,
           Email = userDto.Email,
           Password = hashedPassword,
       };

       var createdUser = await _userRepository.CreateAsync(user);

       var token = _jwtService.GenerateToken(user); 

       return new UserTokenDto { Token = token };
   }

    public async Task<UserTokenDto> Login(UserLoginDto userDto)
    {
        var user = await _userRepository.GetByEmailAsync(userDto.Email);
        if (user == null)
            throw new Exception("Usuário não encontrado.");

        var isValidPassword = _passwordHasher.VerifyPassword(user, userDto.Password);
        if (!isValidPassword)
            throw new Exception("Usuário ou senha inválidos.");

        var token = _jwtService.GenerateToken(user);

        return new UserTokenDto { Token = token };
    }
    
   public async Task<bool> Delete(int id)
   {
   var user = await _userRepository.GetByIdAsync(id.ToString());

       if (user == null)
           return false;

       await _userRepository.DeleteAsync(user.UserId);
       return true;
   }
}
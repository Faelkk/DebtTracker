using DebtTrack.Dtos.User;

namespace DebtTrack.Interfaces;

public interface IUserService
{
    
    Task<IEnumerable<UserDto>> GetAll();
    Task<UserDto> GetById(int id);
    
    Task<UserTokenDto> Create(UserCreateDto userDto);
    
    Task<UserTokenDto> Login(UserLoginDto userDto);
    
    Task<bool> Delete(int id);
}
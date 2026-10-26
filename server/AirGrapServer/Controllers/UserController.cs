using Microsoft.AspNetCore.Mvc;
using AirGrap.Exceptions;
using AirGrap.Log;

namespace AirGrap.Controllers;

[ApiController]
[Route("user")]
public class UserController(IDbService _dbservise) : ControllerBase
{
  //private readonly IUserService _userService;

  [HttpGet("registration")]
  public async Task<IActionResult> Registration(string login,string Password,string nickname)
  {
    if (login == null  || login.Length < 4) return BadRequest("login is too short < 4 characters");
    if (Password == null  || Password.Length < 8) return BadRequest("the password is too short < 8 characters");
    else if (!Password.Any(char.IsLetter)) return BadRequest("The password contains no letters.");
    if (nickname == null  || nickname.Length < 4) return BadRequest("nickname is too short < 4 characters");
    try
    {
      await _dbservise.Registration(login,Password,nickname);
      return Ok();
    }
    catch (UserAlreadyExistsException ex)
    {
      return Conflict(new {error = ex.Message});
    }
    catch(Exception ex)
    {
      Logger.AddLog(String.Concat(ex.Message,ex.Data.ToString()));
      return StatusCode(500, new { error = "Internal server error pliase try again"});
    }
    
    
  }
}
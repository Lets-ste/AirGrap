using Microsoft.AspNetCore.Mvc;
using AirGrap.Exceptions;
using AirGrap.Log;
using AirGrap.DTOs;

namespace AirGrap.Controllers;

[ApiController]
[Route("user")]
public class UserController(IDbService _dbservise) : ControllerBase
{
  [HttpPost("registration")]
  public async Task<IActionResult> Registration([FromBody] RegistrationDTO body)
  {
    if (body.login == null  || body.login.Length < 4) return BadRequest("login is too short < 4 characters");
    if (body.password == null  || body.password.Length < 8) return BadRequest("the password is too short < 8 characters");
    else if (!body.password.Any(char.IsLetter)) return BadRequest("The password contains no letters.");
    if (body.nickname == null  || body.nickname.Length < 4) return BadRequest("nickname is too short < 4 characters");
    try
    {
      await _dbservise.Registration(body.login,body.password,body.nickname);
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
  [HttpPost("auntification")]
  public async Task<IActionResult> Auntification(AuntificationDTO body)
  {
    if (body.login == null) return BadRequest("login is null");
    if (body.password == null ) return BadRequest("the password is null");
    try
    {
      string? token = await _dbservise.Auntification(body.login,body.password);
      return (token == null || token == String.Empty)? NotFound("user not found"): Ok(token);
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
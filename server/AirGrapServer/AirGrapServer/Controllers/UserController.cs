using Microsoft.AspNetCore.Mvc;
using AirGrap.Services;
using AirGrap.DTO;
namespace AirGrap.Controllers;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
  private readonly IUserService _userService;

  public UserController(IUserService userService)
  {
    _userService = userService;
  }

  [HttpGet]
  public IActionResult GetAll()
  {
    return Ok(_userService.GetAll());
  }
  [HttpGet("{id}")]
  public IActionResult GetById(int id)
  {
    if (id <= 0) 
      return BadRequest("Некорректный id");
    var person = _userService.GetById(id);
    if (person is null)
      return NotFound("Пользователь не найден");
    return Ok(person);
  }
  [HttpPost]
  public IActionResult Create([FromBody] CreateUserRequest request){
    if (string.IsNullOrWhiteSpace(request.Name)) return BadRequest("incorect request: NullOrWhiteSpace name");
    if (request.Age < 1 || request.Age > 150) return BadRequest($"incorect request: Age {(request.Age < 1 ? "< 1" : "> 150")}");
    Person p = _userService.Create(request.Name, request.Age);
    return CreatedAtAction(nameof(GetById), new { id = p.Id }, p);
  }
  [HttpPut("{Id}")]
  public IActionResult Replase([FromRoute]int id,[FromBody] CreateUserRequest request){
    if (string.IsNullOrWhiteSpace(request.Name)) return BadRequest("incorect request: NullOrWhiteSpace name");
    if (request.Age < 1 || request.Age > 150) return BadRequest($"incorect request: Age {(request.Age < 1 ? "< 1" : "> 150")}");
    if (_userService.Update(id,request.Name,request.Age))
      return NoContent();
    else
      return NotFound();
  }
  [HttpDelete("{Id}")]
  public IActionResult Delete(int Id){
    if (_userService.Delete(Id))
      return NoContent();
    else
      return NotFound();
  }
}
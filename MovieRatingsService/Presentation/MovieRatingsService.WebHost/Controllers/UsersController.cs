using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieRatingsService.Application.Models.User;
using MovieRatingsService.WebHost.Requests.User;
using MovieRatingsService.WebHost.Responses.User;
using MovieRatingsService.Application.Service.Abstractions;

namespace MovieRatingsService.WebHost.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsersController(IUsersApplicationService usersService, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserResponse>))]
        public async Task<IActionResult> GetAllUsers([FromQuery] string? username, CancellationToken cancellationToken)
        {
            var users = await usersService.GetUsersAsync(username, cancellationToken);
            return Ok(mapper.Map<IEnumerable<UserResponse>>(users));
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> GetUserById(Guid id, CancellationToken cancellationToken)
        {
            var user = await usersService.GetUserByIdAsync(id, cancellationToken);
            if (user is null)
                return NotFound($"Пользователь с id:{id} не найден");
            return Ok(mapper.Map<UserResponse>(user));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateUser(CreateUserRequest request, CancellationToken cancellationToken)
        {
            var model = mapper.Map<CreateUserModel>(request);
            var created = await usersService.CreateUserAsync(model, cancellationToken);
            if (created is null)
                return BadRequest("Пользователь не может быть создан");

            var response = mapper.Map<UserResponse>(created);
            return CreatedAtAction(nameof(GetUserById), new { id = response.Id }, response);
        }
    }
}
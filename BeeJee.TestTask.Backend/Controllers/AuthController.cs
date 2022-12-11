using BeeJee.TestTask.Backend.Dto;
using BeeJee.TestTask.Backend.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BeeJee.TestTask.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IValidator<LoginRequestDto> _validator;
        private readonly ITokenService _tokenService;
        public AuthController(ITokenService tokenService, IValidator<LoginRequestDto> validator, ILogger<AuthController> logger)
        {
            _tokenService = tokenService;
            _validator = validator;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<object>> Login(LoginRequestDto dto, CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ResponseMessageDto<object>(ResponseStatus.Error)
                {
                    Message = validationResult.Errors.Select(x => new { x.PropertyName, x.ErrorMessage }).ToArray()
                });
            }

            //TODO
            //There are no users here yet, so the login is hardcoded
            if (string.Equals(dto.UserName, "admin") && string.Equals(dto.Password, "123"))
            {
                _logger.LogInformation("Успешный вход пользователя {UserName}", dto.UserName);

                var accessToken = _tokenService.GenerateToken(dto.UserName);
                return Ok(new LoginResponseDto()
                {
                    Token = accessToken,
                });
            }

            return Unauthorized(new ResponseMessageDto<string>(ResponseStatus.Error)
            {
                Message = "Неверный логин или пароль"
            });
        }
    }
}

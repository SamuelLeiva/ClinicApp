using Microservices.Services.AuthAPI.Models.DTOs;
using Microservices.Services.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Services.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDto _responseDto;

        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
            _responseDto = new ResponseDto();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            var errorMessage = await _authService.Register(registrationRequestDto);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = errorMessage;
                return BadRequest(_responseDto);
            }

            _responseDto.Result = registrationRequestDto.Email;
            _responseDto.Message = "User registered successfully.";

            return Ok(_responseDto);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var loginResponse = await _authService.Login(loginRequestDto);
            if (loginResponse.User == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Username or password is incorrect.";
                return BadRequest(_responseDto);
            }
            _responseDto.Result = loginResponse;
            _responseDto.Message = "Login successful.";
            return Ok(_responseDto);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestWithRoleDto registrationRequestDto)
        {
            var assignRoleSuccessfully = await _authService.AssignRole(registrationRequestDto.Email, registrationRequestDto.Role);
            if (!assignRoleSuccessfully)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Role assignment failed.";
                return BadRequest(_responseDto);
            }

            _responseDto.Result = registrationRequestDto.Email;
            _responseDto.Message = "Role assigned successfully.";
            return Ok(_responseDto);
        }
    }
}

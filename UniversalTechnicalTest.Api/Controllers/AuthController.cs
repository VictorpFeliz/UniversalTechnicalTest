using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using UniversalTechnicalTest.Api.DTOs.Auth;
using UniversalTechnicalTest.Api.Services.Interfaces;
using RegisterRequest = UniversalTechnicalTest.Api.DTOs.Auth.RegisterRequest;

namespace UniversalTechnicalTest.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IValidator<DTOs.Auth.RegisterRequest> _registerValidator;


        public AuthController(IAuthService authService, IValidator<DTOs.Auth.RegisterRequest> registerValidator)
        {
            _authService = authService;
            _registerValidator = registerValidator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var validationResult = await _registerValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    Errors = validationResult.Errors
                    .Select(x => x.ErrorMessage)
                });
            }


            var response = await _authService.RegisterAsync(request);


            return Ok(response);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(DTOs.Auth.LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);

            return Ok(response);
        }
    }


}

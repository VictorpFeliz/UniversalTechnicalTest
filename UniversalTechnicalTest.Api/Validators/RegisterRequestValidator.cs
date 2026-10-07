using FluentValidation;
using UniversalTechnicalTest.Api.DTOs.Auth;

namespace UniversalTechnicalTest.Api.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator(IConfiguration configuration)
        {
            var emailRegex = configuration["Validation:EmailRegex"];
            var passwordRegex = configuration["Validation:PasswordRegex"];

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("El nombre es requerido");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("El correo es requerido")
                .Matches(emailRegex!)
                .WithMessage("El correo no es válido");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("La contraseña es requerida")
                .Matches(passwordRegex!)
                .WithMessage("La contraseña debe tener al menos 8 caracteres, una letra mayúscula, una letra minúscula, un número y un carácter especial");
        }
    }
}

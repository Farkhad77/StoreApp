using FluentValidation;
using StoreApp.Application.DTOs.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Validations.UserValidations
{
    public class UserRegisterDtoValidator : AbstractValidator<UserRegisterDto>
    {
        public UserRegisterDtoValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Fullname is required.")
                .MinimumLength(3).WithMessage("Fullname must be at least 3 characters long.")
                .MaximumLength(50).WithMessage("Fullname must not exceed 50 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .MaximumLength(20).WithMessage("Password must not exceed 20 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
    
        }
    }
}

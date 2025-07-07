using FluentValidation;
using StoreApp.Application.DTOs.ReviewDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Validations.ReviewValidations
{
    public class ReviewCreateDtoValidator : AbstractValidator<ReviewCreateDto>
    {
        public ReviewCreateDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Məhsul ID-si boş ola bilməz.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Rəy məzmunu boş ola bilməz.")
                .MaximumLength(1000).WithMessage("Rəy maksimum 1000 simvol ola bilər.");
        }
    }
}

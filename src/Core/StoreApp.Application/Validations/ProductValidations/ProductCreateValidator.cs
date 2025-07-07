using FluentValidation;
using StoreApp.Application.DTOs.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Validations.ProductValidations
{
    public class ProductCreateDtoValidator : AbstractValidator<ProductCreateDto>
    {
        public ProductCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Məhsul adı boş ola bilməz.")
                .MaximumLength(100).WithMessage("Məhsul adı maksimum 100 simvol ola bilər.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Qiymət sıfırdan böyük olmalıdır.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Təsvir maksimum 500 simvol ola bilər.")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stok mənfi ola bilməz.");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Kateqoriya seçilməlidir.");

            RuleFor(x => x.ImageUrl)
                .NotEmpty().WithMessage("Şəkil URL-si boş ola bilməz.")
                .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .WithMessage("Şəkil URL-si düzgün formatda olmalıdır.");
        }
    }
}

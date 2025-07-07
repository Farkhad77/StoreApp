using FluentValidation;
using StoreApp.Application.DTOs.OrderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Validations.OrderValidations
{
    public class OrderCreateDtoValidator : AbstractValidator<OrderCreateDto>
    {
        public OrderCreateDtoValidator()
        {
            RuleFor(x => x.ProductIds)
                .NotNull().WithMessage("Məhsul siyahısı boş ola bilməz.")
                .Must(ids => ids != null && ids.Any()).WithMessage("Məhsul siyahısı boş ola bilməz.");

            RuleFor(x => x.OrderCount)
                .GreaterThan(0).WithMessage("Sifariş sayı sıfırdan böyük olmalıdır.");
        }
    }
}

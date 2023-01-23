using DogAPI.Common.DTO;
using DogAPI.DAL.Entities;

using FluentValidation;

using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DogAPI.Validators;

public class DogValidator : AbstractValidator<DogDTO>
{
    public DogValidator()
    {
        RuleFor(d => d.Name).NotEmpty()
                            .MinimumLength(1)
                            .MaximumLength(50)
                            .Matches(@"([A-Z]|[А-Я]){1}([a-z]|[а-я])*");

        RuleFor(d => d.TailLength).GreaterThan(0);

        RuleFor(d => d.Weight).GreaterThan(0);

        RuleFor(d => d.Color).NotEmpty()
            .MinimumLength(1)
            .MaximumLength(50);
    }
}
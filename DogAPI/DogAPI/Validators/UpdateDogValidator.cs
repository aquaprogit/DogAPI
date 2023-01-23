using DogAPI.Common.DTO;

using FluentValidation;

namespace DogAPI.Validators;

public class UpdateDogValidator : AbstractValidator<UpdateDogDTO>
{
    public UpdateDogValidator()
    {
        RuleFor(d => d.Color).NotEmpty()
            .MinimumLength(1)
            .MaximumLength(50);

        RuleFor(d => d.TailLength).GreaterThan(0);

        RuleFor(d => d.Weight).GreaterThan(0);
    }
}

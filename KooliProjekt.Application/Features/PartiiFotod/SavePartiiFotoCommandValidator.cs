using FluentValidation;

namespace KooliProjekt.Application.Features.PartiiFotod
{
    public class SavePartiiFotoCommandValidator : AbstractValidator<SavePartiiFotoCommand>
    {
        public SavePartiiFotoCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThanOrEqualTo(0);
            RuleFor(x => x.FailiTee).NotEmpty().MaximumLength(500);
            RuleFor(x => x.PartiiId).GreaterThan(0);
        }
    }
}

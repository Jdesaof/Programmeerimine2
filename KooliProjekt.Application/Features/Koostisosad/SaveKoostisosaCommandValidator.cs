using FluentValidation;

namespace KooliProjekt.Application.Features.Koostisosad
{
    public class SaveKoostisosaCommandValidator : AbstractValidator<SaveKoostisosaCommand>
    {
        public SaveKoostisosaCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Nimetus).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Uhik).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Kirjeldus).MaximumLength(255);
            RuleFor(x => x.PartiiId).GreaterThan(0);
            RuleFor(x => x.Hind).GreaterThanOrEqualTo(0m).PrecisionScale(18, 4, true);
            RuleFor(x => x.Kogus).GreaterThan(0m).PrecisionScale(18, 4, true);
        }
    }
}

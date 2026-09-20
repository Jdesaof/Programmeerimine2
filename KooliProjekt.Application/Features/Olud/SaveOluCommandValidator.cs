using FluentValidation;

namespace KooliProjekt.Application.Features.Olud
{
    public class SaveOluCommandValidator : AbstractValidator<SaveOluCommand>
    {
        public SaveOluCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Nimi).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Kirjeldus).MaximumLength(255);
            RuleFor(x => x.Tuup).MaximumLength(50);
            RuleFor(x => x.Alkoholiprotsent).InclusiveBetween(0m, 100m).PrecisionScale(18, 2, true);
        }
    }
}

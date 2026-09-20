using FluentValidation;

namespace KooliProjekt.Application.Features.Partiid
{
    public class SavePartiiCommandValidator : AbstractValidator<SavePartiiCommand>
    {
        public SavePartiiCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Kood).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Kuupaev).NotEmpty();
            RuleFor(x => x.Kirjeldus).MaximumLength(255);
            RuleFor(x => x.Tulemus).MaximumLength(500);
            RuleFor(x => x.OluId).GreaterThan(0);
        }
    }
}

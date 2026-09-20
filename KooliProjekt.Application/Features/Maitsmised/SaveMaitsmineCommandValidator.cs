using FluentValidation;

namespace KooliProjekt.Application.Features.Maitsmised
{
    public class SaveMaitsmineCommandValidator : AbstractValidator<SaveMaitsmineCommand>
    {
        public SaveMaitsmineCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Kuupaev).NotEmpty();
            RuleFor(x => x.Degusteerija).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Kommentaar).MaximumLength(255);
            RuleFor(x => x.PartiiId).GreaterThan(0);
            RuleFor(x => x.Hinne).InclusiveBetween(1, 10);
        }
    }
}

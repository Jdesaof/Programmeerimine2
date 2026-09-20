using FluentValidation;

namespace KooliProjekt.Application.Features.PruulimisLogid
{
    public class SavePruulimisLogiCommandValidator : AbstractValidator<SavePruulimisLogiCommand>
    {
        public SavePruulimisLogiCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Kuupaev).NotEmpty();
            RuleFor(x => x.Kasutaja).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Kirjeldus).NotEmpty().MaximumLength(2000);
            RuleFor(x => x.PartiiId).GreaterThan(0);
        }
    }
}

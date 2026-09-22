using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Koostisosad
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class DeleteKoostisosaCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}

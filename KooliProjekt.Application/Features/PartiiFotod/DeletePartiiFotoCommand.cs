using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.PartiiFotod
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class DeletePartiiFotoCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}

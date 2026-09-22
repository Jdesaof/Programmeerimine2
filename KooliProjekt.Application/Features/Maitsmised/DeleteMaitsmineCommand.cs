using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Maitsmised
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class DeleteMaitsmineCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}

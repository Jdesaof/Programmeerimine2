using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Olud
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class DeleteOluCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}

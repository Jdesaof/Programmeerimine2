using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Partiid
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class DeletePartiiCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}

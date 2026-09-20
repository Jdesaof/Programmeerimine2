using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Maitsmised
{
    public class DeleteMaitsmineCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}

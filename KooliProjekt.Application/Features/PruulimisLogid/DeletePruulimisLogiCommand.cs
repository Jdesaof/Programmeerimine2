using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.PruulimisLogid
{
    public class DeletePruulimisLogiCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}

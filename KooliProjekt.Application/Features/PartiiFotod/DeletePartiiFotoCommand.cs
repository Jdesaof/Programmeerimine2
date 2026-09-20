using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.PartiiFotod
{
    public class DeletePartiiFotoCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}

using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Koostisosad
{
    public class DeleteKoostisosaCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}

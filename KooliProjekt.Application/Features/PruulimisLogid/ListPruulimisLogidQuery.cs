using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.PruulimisLogid
{
    public class ListPruulimisLogidQuery
        : IRequest<OperationResult<List<PruulimisLogi>>>
    {
    }
}
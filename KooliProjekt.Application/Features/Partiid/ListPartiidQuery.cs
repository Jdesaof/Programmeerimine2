using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Partiid
{
    public class ListPartiidQuery
        : IRequest<OperationResult<List<Partii>>>
    {
    }
}
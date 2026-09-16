using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Maitsmised
{
    public class ListMaitsmisedQuery
        : IRequest<OperationResult<List<Maitsmine>>>
    {
    }
}
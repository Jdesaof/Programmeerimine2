using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Koostisosad
{
    public class ListKoostisosadQuery
        : IRequest<OperationResult<List<Koostisosa>>>
    {
    }
}
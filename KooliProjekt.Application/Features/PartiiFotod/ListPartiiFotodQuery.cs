using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.PartiiFotod
{
    public class ListPartiiFotodQuery
        : IRequest<OperationResult<List<PartiiFoto>>>
    {
    }
}
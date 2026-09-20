using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Partiid
{
    public class DeletePartiiCommandHandler : IRequestHandler<DeletePartiiCommand, OperationResult>
    {
        private readonly ApplicationDbContext _context;

        public DeletePartiiCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult> Handle(DeletePartiiCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var result = new OperationResult();
            if (request.Id <= 0)
                return result.AddPropertyError(nameof(request.Id), "Id must be greater than zero.");

            var entity = await _context.Partiid
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            // Deleting a record that is already absent is a successful no-op.
            if (entity == null) return result;

            _context.Partiid.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Olud
{
    public class DeleteOluCommandHandler : IRequestHandler<DeleteOluCommand, OperationResult>
    {
        private readonly ApplicationDbContext _context;

        public DeleteOluCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult> Handle(DeleteOluCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var result = new OperationResult();
            if (request.Id <= 0)
                return result.AddPropertyError(nameof(request.Id), "Id must be greater than zero.");

            var entity = await _context.Olud
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            // Deleting a record that is already absent is a successful no-op.
            if (entity == null) return result;

            _context.Olud.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}

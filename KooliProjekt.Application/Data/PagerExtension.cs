using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Infrastructure.Paging;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    public static class PagerExtension
    {
        public static async Task<PagedResult<T>> GetPagedAsync<T>(
            this IQueryable<T> query,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            if (page <= 0)
            {
                throw new ArgumentException(
                    "Page must be greater than zero.", nameof(page));
            }

            if (pageSize <= 0 || pageSize > 100)
            {
                throw new ArgumentException(
                    "PageSize must be between 1 and 100.",
                    nameof(pageSize));
            }

            int skip = checked((page - 1) * pageSize);

            var rowCount = await query.CountAsync(cancellationToken);

            var results = await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = rowCount,
                PageCount = (int)Math.Ceiling(
                    rowCount / (double)pageSize),
                Results = results
            };
        }
    }
}
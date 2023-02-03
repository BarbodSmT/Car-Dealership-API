using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Utilities
{
    public class Paginated<T>
    {
        public List<T> content { get; set; }
        public int totalPages { get; set; }
        public int pageSize { get; set; }

    }
    public static class QueryExtensions
    {
        public static async Task<Paginated<T>> ToPaginateAsync<T>(this IQueryable<T> queryable, CancellationToken cancellationToken, int PageNumber, int PageSize = 10)
        {
            var pagedData = await queryable
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync(cancellationToken);

            var totalData = await queryable.CountAsync(cancellationToken);
            var paginated = new Paginated<T>
            {
                content = pagedData,
                totalPages = totalData,
                pageSize = PageSize
            };

            return paginated;
        }
    }
}

using ApplicationCore.Helper.Models;
using Microsoft.EntityFrameworkCore;

namespace Services.Helper
{
    public static class PagingHelper
    {
        public static async Task<List<T>> ToListPagedAsync<T>(this IQueryable<T> query, int? pageIndex, int? pageSize, PagingResult resultData) where T : class
        {
            resultData.Paging = new Paging
            {
                Page = pageIndex ?? 0,
                TotalRow = await query.CountAsync()
            };

            var pageCount = (double)resultData.Paging.TotalRow / (pageSize ?? 1);
            resultData.Paging.TotalPage = (int)Math.Ceiling(pageCount);

            var skip = ((pageIndex ?? 0) - 1) * (pageSize ?? 1);
            return await query.Skip(skip)
                .Take(pageSize ?? 1)
                .ToListAsync();
        }
    }
}

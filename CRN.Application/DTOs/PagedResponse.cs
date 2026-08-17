using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN.Application.DTOs
{
    public class PagedResponse<T>
    {
        public IReadOnlyList<T> Items { get; set; } = [];

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages => PageSize == 0? 0: (int)Math.Ceiling(
                    TotalCount / (double)PageSize);
    }
}

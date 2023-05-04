using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.ModelsDto.Order
{
    public class OrderPaginationDto
    {
        public int TotalOrder { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get; set; }
        public int Page { get; set; }
        public List<OrderDto> Orders { get; set; } = new List<OrderDto>();
    }
}

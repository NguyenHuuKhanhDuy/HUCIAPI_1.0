using ApplicationCore.Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.ModelsDto.Order
{
    public class OrderPaginationDto : PagingResult
    {
        public List<OrderDto> Orders { get; set; } = new List<OrderDto>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN.Application.DTOs.Item
{
    public class CreateItemRequest
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelMe.Shared.Models
{
    public class OrderRequest
    {
        public List<int> Items { get; set; } = new();
    }
}
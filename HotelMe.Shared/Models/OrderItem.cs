using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelMe.Shared.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}

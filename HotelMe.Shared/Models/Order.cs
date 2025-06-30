using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelMe.Shared.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = "Pending";
        public List<OrderItem> Items { get; set; } = new();
    }
}
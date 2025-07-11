using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        [NotMapped]
        public string? RoomNumber { get; set; }

        [NotMapped]
        public string? ItemsSummary { get; set; }
    }
}
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelMe.Shared.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        
        public string UserId { get; set; } = null!;
        
        public string Message { get; set; } = null!;
        
        public DateTime SentAt { get; set; }
        
        [NotMapped]
        public bool HasActiveBooking { get; set; }
        
        [NotMapped]
        public string? RoomNumber { get; set; }
    }

}

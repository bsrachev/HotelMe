namespace HotelMe.Shared.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public string UserId { get; set; } // Linked to ApplicationUser

        public string RoomNumber { get; set; }

        public DateTime CheckInDate { get; set; }

        public DateTime CheckOutDate { get; set; }

        public string Status { get; set; } // "Pending", "CheckedIn", "CheckedOut"
    }
}
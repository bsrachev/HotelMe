namespace HotelMe.Shared.Models
{
    public class BookingRequest
    {
        public string RoomNumber { get; set; } = null!;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}

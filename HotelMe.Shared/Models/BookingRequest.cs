using System.ComponentModel.DataAnnotations;

namespace HotelMe.Shared.Models
{
    /// <summary>
    /// Request payload for creating a booking from the PWA.
    /// Includes validation so the Blazor form can show errors client side.
    /// </summary>
    public class BookingRequest : IValidatableObject
    {
        [Required]
        public string RoomNumber { get; set; } = null!;

        [Required]
        [DataType(DataType.Date)]
        public DateTime? CheckInDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? CheckOutDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CheckInDate.HasValue && CheckOutDate.HasValue && CheckOutDate < CheckInDate)
            {
                yield return new ValidationResult(
                    "Check-out date must be after check-in date",
                    new[] { nameof(CheckOutDate) });
            }
        }
    }
}

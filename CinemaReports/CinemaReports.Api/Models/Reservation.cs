namespace CinemaReports.Api.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int ScreeningId { get; set; }
        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public int SeatNumber { get; set; }
        public DateTime ReservedUtc { get; set; }
    }
}

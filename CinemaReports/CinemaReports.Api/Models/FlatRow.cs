namespace CinemaReports.Api.Models
{
    public class FlatRow
    {
        public int ScreeningId { get; set; }
        public string MovieTitle { get; set; } = "";
        public DateTime StartsAt { get; set; }
        public string Auditorium { get; set; } = "";
        public int NumberOfSeats { get; set; }
        public decimal TicketPrice { get; set; }
        public int? SeatNumber { get; set; }
        public string? ActualCustomerName { get; set; }
    }
}
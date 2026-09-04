namespace CinemaReports.Api.Models
{
    public class ScreeningDetails
    {
        public int ScreeningId { get; set; }
        public string MovieTitle { get; set; } = "";
        public DateTime StartsAt { get; set; }
        public string Auditorium { get; set; } = "";
        public int NumberOfSeats { get; set; }
        public decimal TicketPrice { get; set; }
        public List<ReservationInfo> Reservations { get; set; } = new();
    }
}
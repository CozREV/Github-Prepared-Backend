namespace CinemaReports.Api.Models
{
    public class Screening
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public DateTime StartsAt { get; set; }
        public string Auditorium { get; set; } = "";
        public int NumberOfSeats { get; set; }
        public decimal TicketPrice { get; set; }
    }
}

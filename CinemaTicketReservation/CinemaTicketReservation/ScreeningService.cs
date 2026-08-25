namespace CinemaTicketReservation
{
    public class ScreeningService
    {
        private readonly IScreeningRepository _screeningRepository;

        public ScreeningService(IScreeningRepository screeningRepository)
        {
            _screeningRepository = screeningRepository;
        }

        public Result<ReservationReceipt> ReserveSeat(int screeningId, string customerName, int seatNumber)
        {
            var screening = _screeningRepository.Find(screeningId);
            if (screening == null)
            {
                return Result<ReservationReceipt>.Failure("Kinovisningen finnes ikke.");
            }

            if (string.IsNullOrWhiteSpace(customerName))
            {
                return Result<ReservationReceipt>.Failure("Kunde må ha et navn.");
            }

            if (seatNumber < 1 || seatNumber > screening.NumberOfSeats)
            {
                return Result<ReservationReceipt>.Failure("Ugyldig setenummer.");
            }

            if (screening.ReservedSeats.Contains(seatNumber))
            {
                return Result<ReservationReceipt>.Failure("Setet er allerede reservert");
            }

            screening.ReservedSeats.Add(seatNumber);
            _screeningRepository.Save(screening);

            var receipt = new ReservationReceipt
            {
                ScreeningId = screening.Id,
                MovieTitle = screening.MovieTitle,
                CustomerName = customerName,
                SeatNumber = seatNumber
            };

            return Result<ReservationReceipt>.Success(receipt);
        }
    }
}

namespace CinemaTicketReservation.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void ReserveSeat_Success()
        {
            var repository = new FakeScreeningRepository();
            var screening = new Screening
            {
                Id = 1,
                MovieTitle = "Interstellar",
                NumberOfSeats = 10,
                ReservedSeats = new List<int> { 2, 5 }
            };
            repository.Save(screening);

            var service = new ScreeningService(repository);

            var result = service.ReserveSeat(1, "Ada", 7);
        }
    }
}

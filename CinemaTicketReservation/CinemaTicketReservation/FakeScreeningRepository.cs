namespace CinemaTicketReservation
{
    public class FakeScreeningRepository : IScreeningRepository
    {
        private readonly List<Screening> _screenings = new();

        public Screening? Find(int id)
        {
            return _screenings.FirstOrDefault(s => s.Id == id);
        }

        public void Save(Screening screening)
        {
            var existing = _screenings.FirstOrDefault(s => s.Id == screening.Id);
            if (existing != null)
            {
                _screenings.Remove(existing);
            }
            _screenings.Add(screening);
        }
    }
}

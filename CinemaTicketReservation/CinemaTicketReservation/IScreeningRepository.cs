namespace CinemaTicketReservation
{
    public interface IScreeningRepository
    {
        Screening? Find(int id);
        void Save(Screening screening);
    }
}

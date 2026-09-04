using Dapper;
using Microsoft.Data.SqlClient;
using CinemaReports.Api.Models;

namespace CinemaReports.Api
{
    public class ScreeningRepository
    {
        private readonly string _connectionString;

        public ScreeningRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CinemaReports")
                ?? throw new InvalidOperationException("Connection string mangler.");
        }

        public async Task<IEnumerable<ScreeningWithMovie>> GetAllAsync()
        {
            const string sql = """
                SELECT s.Id AS ScreeningId, m.Title AS MovieTitle, s.StartsAt,
                       s.Auditorium, s.NumberOfSeats, s.TicketPrice
                FROM Screenings s
                JOIN Movies m ON m.Id = s.MovieId
                ORDER BY s.StartsAt;
                """;

            await using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<ScreeningWithMovie>(sql);
        }

        public async Task<ScreeningDetails?> FindAsync(int id)
        {
            const string sql = """
                SELECT
                    s.Id AS ScreeningId,
                    m.Title AS MovieTitle,
                    s.StartsAt,
                    s.Auditorium,
                    s.NumberOfSeats,
                    s.TicketPrice,
                    r.SeatNumber,
                    COALESCE(c.Name, r.CustomerName) AS ActualCustomerName
                FROM Screenings s
                JOIN Movies m ON m.Id = s.MovieId
                LEFT JOIN Reservations r ON r.ScreeningId = s.Id
                LEFT JOIN Customers c ON c.Id = r.CustomerId
                WHERE s.Id = @Id;
                """;

            await using var connection = new SqlConnection(_connectionString);
            var rows = (await connection.QueryAsync<FlatRow>(sql, new { Id = id })).ToList();

            if (!rows.Any()) return null;

            var first = rows.First();

            return new ScreeningDetails
            {
                ScreeningId = first.ScreeningId,
                MovieTitle = first.MovieTitle,
                StartsAt = first.StartsAt,
                Auditorium = first.Auditorium,
                NumberOfSeats = first.NumberOfSeats,
                TicketPrice = first.TicketPrice,
                Reservations = rows
                    .Where(r => r.SeatNumber != null)
                    .Select(r => new ReservationInfo
                    {
                        SeatNumber = r.SeatNumber!.Value,
                        CustomerName = r.ActualCustomerName ?? ""
                    })
                    .ToList()
            };
        }
    }
}
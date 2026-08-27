namespace CouponApi.Api

{
    using Dapper;
    using Microsoft.Data.SqlClient;

    public class SqlCouponRepository : ICouponRepository
    {
        private readonly string _connectionString;

        public SqlCouponRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Coupon")
                ?? throw new InvalidOperationException("Connection string mangler.");
        }

        public async Task<IEnumerable<Coupon>> GetAllAsync()
        {
            const string sql = "SELECT Id, Code, Description, RemainingUses, IsActive FROM Coupons;";
            await using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<Coupon>(sql);
        }

        public async Task<Coupon?> FindAsync(int id)
        {
            const string sql = "SELECT Id, Code, Description, RemainingUses, IsActive FROM Coupons WHERE Id = @Id;";
            await using var connection = new SqlConnection(_connectionString);
            return await connection.QuerySingleOrDefaultAsync<Coupon>(sql, new { Id = id });
        }

        public async Task<Coupon?> FindByCodeAsync(string code)
        {
            const string sql = "SELECT Id, Code, Description, RemainingUses, IsActive FROM Coupons WHERE Code = @Code;";
            await using var connection = new SqlConnection(_connectionString);
            return await connection.QuerySingleOrDefaultAsync<Coupon>(sql, new { Code = code });
        }

        public async Task<int> CreateAsync(Coupon coupon)
        {
            const string sql = """
                INSERT INTO Coupons (Code, Description, RemainingUses, IsActive)
                OUTPUT INSERTED.Id
                VALUES (@Code, @Description, @RemainingUses, @IsActive);
                """;

            await using var connection = new SqlConnection(_connectionString);
            return await connection.QuerySingleAsync<int>(sql, coupon);
        }

        public async Task<bool> TryUseAsync(int id)
        {
            const string sql = """
                UPDATE Coupons SET
                RemainingUses = RemainingUses - 1
                WHERE Id = @Id AND IsActive = 1 AND RemainingUses > 0;
                """;
            
            await using var connection = new SqlConnection(_connectionString);
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected == 1;
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            const string sql = """
                UPDATE Coupons SET
                IsActive = 0
                WHERE Id = @Id;
                """;

            await using var connection = new SqlConnection(_connectionString);
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected == 1;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = """
                DELETE FROM Coupons
                WHERE Id = @Id;
                """;

            await using var connection = new SqlConnection(_connectionString);
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected == 1;
        }
    }
}

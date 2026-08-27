namespace CouponApi.Api
{
    public interface ICouponRepository
    {
        Task<IEnumerable<Coupon>> GetAllAsync();
        Task<Coupon?> FindAsync(int id);
        Task<Coupon?> FindByCodeAsync(string code);
        Task<int> CreateAsync(Coupon coupon);
        Task<bool> TryUseAsync(int id);
        Task<bool> DeactivateAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}

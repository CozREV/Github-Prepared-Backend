namespace CouponApi.Api.Services
{
    public class CouponService(ICouponRepository repo)
    {
        public Task<IEnumerable<Coupon>> GetAllAsync()
        {
            return repo.GetAllAsync();
        }

        public Task<Coupon?> FindAsync(int id)
        {
            return repo.FindAsync(id);
        }

        public async Task<Result<Coupon>> CreateCouponAsync(Coupon coupon)
        {
            if (string.IsNullOrWhiteSpace(coupon.Code))
            {
                return Result<Coupon>.Failure("Code must be given.");
            }

            var existing = await repo.FindByCodeAsync(coupon.Code);
            if (existing != null)
            {
                return Result<Coupon>.Failure("Code is already in use.");
            }

            if (string.IsNullOrWhiteSpace(coupon.Description))
            {
                return Result<Coupon>.Failure("Description must be given.");
            }

            if (coupon.RemainingUses <= 0)
            {
                return Result<Coupon>.Failure("RemainingUses must be greater than zero.");
            }

            var id = await repo.CreateAsync(coupon);
            coupon.Id = id;
            return Result<Coupon>.Success(coupon);
        }

        public async Task<Result<Coupon>> UseCouponAsync(int id)
        {
            var used = await repo.TryUseAsync(id);

            if (used)
            {
                var coupon = await repo.FindAsync(id);
                return Result<Coupon>.Success(coupon!);
            }

            var couponAfterFailedUse = await repo.FindAsync(id);
            
            if (couponAfterFailedUse == null)
            {
                return Result<Coupon>.Failure("Coupon does not exist.");
            }

            if (couponAfterFailedUse.IsActive == false)
            {
                return Result<Coupon>.Failure("Coupon is deactivated");
            }

            return Result<Coupon>.Failure("Coupon has been used up");
        }

        public async Task<Result<Coupon>> DeactivateCouponAsync(int id)
        {
            var deactivated = await repo.DeactivateAsync(id);

            if (!deactivated)
            {
                return Result<Coupon>.Failure("Coupon does not exist");
            }

            var coupon = await repo.FindAsync(id);
            return Result<Coupon>.Success(coupon!);
        }

        public async Task<Result<Coupon>> DeleteCouponAsync(int id)
        {
            var coupon = await repo.FindAsync(id);
            
            if (coupon == null)
            {
                return Result<Coupon>.Failure("Coupon does not exist");
            }

            await repo.DeleteAsync(id);
            return Result<Coupon>.Success(coupon);
        }
    }
}

namespace CouponApi.Api.DTO
{
    public class CreateCouponDto
    {
        public string Code { get; set; } = "";
        public string Description { get; set; } = "";
        public int RemainingUses { get; set; }
    }
}

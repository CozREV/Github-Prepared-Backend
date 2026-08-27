using CouponApi.Api;
using CouponApi.Api.DTO;
using CouponApi.Api.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Coupon")
    ?? throw new InvalidOperationException("Connection string 'Coupon' is missing.");

builder.Services.AddScoped<ICouponRepository, SqlCouponRepository>();
builder.Services.AddScoped<CouponService>();

var app = builder.Build();

app.MapGet("/coupons", async (CouponService service) =>
{
    var coupons = await service.GetAllAsync();
    return Results.Ok(coupons);
});

app.MapGet("/coupons/{id:int}", async (int id, CouponService service) =>
{
    var coupon = await service.FindAsync(id);

    return coupon is null
        ? Results.NotFound("Coupon does not exist.")
        : Results.Ok(coupon);
});

app.MapPost("/coupons", async (CreateCouponDto dto, CouponService service) =>
{
    var coupon = new Coupon
    {
        Code = dto.Code,
        Description = dto.Description,
        RemainingUses = dto.RemainingUses
    };

    var result = await service.CreateCouponAsync(coupon);

    return result.IsSuccess
        ? Results.Created($"/coupons/{result.Value!.Id}", result.Value)
        : Results.BadRequest(result.ErrorMessage);
});

app.MapPost("/coupons/{id:int}/use", async (int id, CouponService service) =>
{
    var result = await service.UseCouponAsync(id);
    return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.ErrorMessage);
});

app.MapPatch("/coupons/{id:int}/deactivate", async (int id, CouponService service) =>
{
    var result = await service.DeactivateCouponAsync(id);
    return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.ErrorMessage);
});

app.MapDelete("/coupons/{id:int}", async (int id, CouponService service) =>
{
    var result = await service.DeleteCouponAsync(id);
    return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.ErrorMessage);
});

app.Run();
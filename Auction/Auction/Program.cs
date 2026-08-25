using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

var auctions = new List<Auction>();

app.MapGet("/auctions", () => auctions);

app.MapPost("/auctions/{id}/bids", async (int id, PlaceBidDto dto) =>
{
    var auction = auctions.FirstOrDefault(a => a.Id == id);
    if (auction == null) return Results.NotFound();

    var result = auction.PlaceBid(dto.BidderName, dto.Amount);

    if (!result.Success)
    {
        return Results.BadRequest(result.ErrorMessage);
    }

    await SaveAuctionAsync(auctions);
    return Results.Ok(auction);
});

async Task SaveAuctionAsync(List<Auction> list)
{
    var json = JsonSerializer.Serialize(list);
    await File.WriteAllTextAsync("auctions.json", json);
}

async Task<List<Auction>> LoadAuctionsAsync()
{
    if (!File.Exists("auctions.json")) return new List<Auction>();
    var json = await File.ReadAllTextAsync("auctions.json");
    return JsonSerializer.Deserialize<List<Auction>>(json) ?? new List<Auction>();
}

app.Run();



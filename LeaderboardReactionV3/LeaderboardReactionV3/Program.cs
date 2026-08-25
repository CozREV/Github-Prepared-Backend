using LeaderboardReactionV3;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

var scores = new List<Score>
{
    new Score { Id = 1, PlayerName = "Ada", Milliseconds = 287 },
    new Score { Id = 2, PlayerName = "Linus", Milliseconds = 311 },
    new Score { Id = 3, PlayerName = "Grace", Milliseconds = 264 },

};

app.MapGet("/scores", () =>
{
    return scores.OrderBy(s => s.Milliseconds);
});

app.MapPost("/scores", (CreateScoreDto dto) =>
{
    var newScore = new Score
    {
        Id = scores.Count + 1,
        PlayerName = dto.PlayerName,
        Milliseconds = dto.Milliseconds
    };

    scores.Add(newScore);
    return newScore;
});

app.Run();


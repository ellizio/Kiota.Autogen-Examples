using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using ScoreService.Client;
using ScoreService.Client.Models;

using var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("http://localhost:5085");

var provider = new AnonymousAuthenticationProvider();
using var adapter = new HttpClientRequestAdapter(provider, httpClient: httpClient);
var client = new ScoreClient(adapter);

try
{
    var footballScores = await client.Scores.GetAsync(r => r.QueryParameters.TypeAsGameType = GameType.Football);
    Console.WriteLine("Football:");
    foreach (var score in footballScores)
        Console.WriteLine($"{score.GameName}: {score.Home} - {score.Away}");
}
catch (ScoreService.Client.Models.ProblemDetails ex) when (ex.Status == 404)
{
    Console.WriteLine($"Football: {ex.Detail}");
}
Console.WriteLine();

try
{
    var hockeyScores = await client.Scores.GetAsync(r => r.QueryParameters.TypeAsGameType = GameType.Hockey);
    Console.WriteLine("Hockey:");
    foreach (var score in hockeyScores)
        Console.WriteLine($"{score.GameName}: {score.Home} - {score.Away}");
}
catch (ScoreService.Client.Models.ProblemDetails ex) when (ex.Status == 404)
{
    Console.WriteLine($"Hockey: {ex.Detail}");
    throw;
}


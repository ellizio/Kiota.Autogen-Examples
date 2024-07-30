using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.Configure<JsonOptions>(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/scores", ([FromQuery] GameType type) =>
    {
        var random = new Random();
        
        Score[]? scores = null;
        switch (type)
        {
            case GameType.Football:
                scores = Enumerable.Range(1, random.Next(5)).Select(_ => new Score(GameType.Football, $"Team{random.Next(1, 10)} - Team{random.Next(1, 10)}", random.Next(0, 5), random.Next(0, 5))).ToArray();
                break;
            case GameType.Hockey:
                scores = Enumerable.Range(1, random.Next(5)).Select(_ => new Score(GameType.Hockey, $"Team{random.Next(1, 10)} - Team{random.Next(1, 10)}", random.Next(0, 8), random.Next(0, 8))).ToArray();
                break;
            case GameType.Basketball:
                scores = Enumerable.Range(1, random.Next(5)).Select(_ => new Score(GameType.Basketball, $"Team{random.Next(1, 10)} - Team{random.Next(1, 10)}", random.Next(20, 100), random.Next(20, 100))).ToArray();
                break;
        }

        return scores?.Length != 0 ? Results.Ok(scores) : Results.Problem(statusCode: StatusCodes.Status404NotFound, detail: "Scores not found");
    })
    .WithName("GetScores")
    .Produces<Score[]>()
    .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
    .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
    .WithOpenApi();

app.Run();

record Score(GameType GameType, string GameName, int Home, int Away);

enum GameType
{
    Football,
    Hockey,
    Basketball
}
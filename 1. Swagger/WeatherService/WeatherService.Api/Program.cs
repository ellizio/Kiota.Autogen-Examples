using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/forecast", ([FromQuery] int month, [FromQuery] int day) =>
    {
        if (month < 1 || month > 12 || day < 1)
            return Results.ValidationProblem(new Dictionary<string, string[]>());
        
        if (DateTime.DaysInMonth(DateTime.UtcNow.Year, month) < day)
            return Results.ValidationProblem(new Dictionary<string, string[]>());

        var random = new Random();
        var forecast = new Forecast(month, day, random.Next(-30, 40));

        return Results.Ok(forecast);
    })
    .WithName("GetForecast")
    .Produces<Forecast>()
    .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
    .WithOpenApi();

app.MapPost("/forecast", ([FromBody] NewForecast forecast) =>
    {
        var month = forecast.Month;
        var day = forecast.Day;

        if (month < 1 || month > 12 || day < 1)
            return Results.ValidationProblem(new Dictionary<string, string[]>());
        
        if (DateTime.DaysInMonth(DateTime.UtcNow.Year, month) < day)
            return Results.ValidationProblem(new Dictionary<string, string[]>());

        return Results.Ok(Guid.NewGuid());
    })
    .WithName("CreateForecast")
    .Produces<Guid>()
    .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
    .WithOpenApi();

app.Run();

record Forecast(int Month, int Day, int Temperature);

record NewForecast(int Month, int Day, int Temperature);
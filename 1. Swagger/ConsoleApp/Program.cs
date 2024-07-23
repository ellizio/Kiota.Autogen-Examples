using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using WeatherService.Client;
using WeatherService.Client.Models;

using var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("http://localhost:5085");

var provider = new AnonymousAuthenticationProvider();
using var adapter = new HttpClientRequestAdapter(provider, httpClient: httpClient);
var client = new WeatherClient(adapter);

var forecast = await client.Forecast.GetAsync(r =>
{
    r.QueryParameters.Day = 1;
    r.QueryParameters.Month = 1;
});
Console.WriteLine($"Temp = {forecast.Temperature}");

var result = await client.Forecast.PostAsync(new NewForecast
{
    Day = 10,
    Month = 5,
    Temperature = 21
});
Console.WriteLine($"Forecast Id = {result}");
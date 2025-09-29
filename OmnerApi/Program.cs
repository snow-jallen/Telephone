using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddHttpClient<AidanClient>(client =>
{
    client.BaseAddress = new Uri("http+https://aidanapi");
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/call", ([FromQuery]string message, AidanClient aidan) =>
{
    var myString = $"Red Herring: {message.Count() - Random.Shared.Next()%258} {message}";
    var response = aidan.MakeCall(myString);
    
    return response;
});

app.Run();

public class AidanClient(HttpClient client)
{
    public async Task<string> MakeCall(string message)
    {
        var response = await client.GetAsync("/call?message=" + message);
        // Simulate an API call to Aidan
        return await response.Content.ReadAsStringAsync();
    }
}

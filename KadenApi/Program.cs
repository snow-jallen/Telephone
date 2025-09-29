var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHttpClient<OmnerClient>(client =>
{
    client.BaseAddress = new Uri("https+http://omnerapi/");
});

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

app.MapGet("/Call", async (string message, OmnerClient omner) =>
{
    message = await omner.MakeCall(message);
    return message;
});

app.Run();

public class OmnerClient(HttpClient client, ILogger<OmnerClient> logger)
{
    public async Task<string> MakeCall(string message)
    {
        var modifiedMessage = $"{message} is a really cool thing to say there Jonathan!";
        logger.LogInformation("Got {message} sent {modified}", message, modifiedMessage);
        var response = await client.GetAsync($"/call?message={modifiedMessage}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
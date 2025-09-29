using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddHttpClient("kadenapi", client =>
{
    client.BaseAddress = new Uri("https+http://kadenapi");
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


bool retry = true;

app.MapGet("/Call", async (string message, IHttpClientFactory factory, ILogger<Program> logger) => {

    var client = factory.CreateClient("kadenapi");

    if (retry)
    {
        retry = false;
        logger.LogInformation("Got {original} and sent {modified}", message, message);
        var response = await client.GetAsync($"/Call?message={message}");
        return await response.Content.ReadAsStringAsync();
    }
    else
    {
        var modified = message + ", ends in aidanAPI";
        logger.LogInformation("Got {original} and sent {modified}", message, modified);
        return modified;

    }
});




app.Run();

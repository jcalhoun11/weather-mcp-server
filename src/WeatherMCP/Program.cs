using Microsoft.OpenApi;
using ModelContextProtocol.AspNetCore;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using WeatherMCP.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowCORS", policy =>
    {
        // Production CORS - configure for your domain
        policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        
    });
});

// Configure logging
builder.Logging.AddConsole(options =>
{
    options.LogToStandardErrorThreshold = LogLevel.Trace;
});

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Weather MCP Server API",
        Version = "v1",
        Description = "MCP Server providing weather tools for NOAA and Open Meteo APIs"
    });
});

// Register HTTP clients for weather services
builder.Services.AddHttpClient<INoaaWeatherService, NoaaWeatherService>(client =>
{
    client.BaseAddress = new Uri("https://api.weather.gov/");
    client.DefaultRequestHeaders.Add("User-Agent", "(WeatherAI MCP Server, weather@example.com)");
    client.DefaultRequestHeaders.Add("Accept", "application/geo+json");
});

builder.Services.AddHttpClient<IOpenMeteoMarineService, OpenMeteoMarineService>(client =>
{
    client.BaseAddress = new Uri("https://marine-api.open-meteo.com/");
});

builder.Services.AddHttpClient<IGeocodingService, GeocodingService>(client =>
{
    client.BaseAddress = new Uri("https://geocoding-api.open-meteo.com/");
});

// Register MCP Server with HTTP transport and tools from assembly
builder.Services
    .AddMcpServer(options =>
    {
        options.ServerInfo = new Implementation
        {
            Name = "Weather MCP Server",
            Version = "1.0.0",
            Description = "MCP Server providing NOAA and Open Meteo weather tools",
        };
    })
    .WithHttpTransport()
    .WithToolsFromAssembly();

var app = builder.Build();

app.UseCors("AllowCORS");
// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

// Map MCP endpoints for SSE transport
app.MapMcp();

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow }));

// API info endpoint
app.MapGet("/info", () => Results.Ok(new
{
    Name = "Weather MCP Server",
    Version = "1.0.0",
    Description = "MCP Server providing NOAA and Open Meteo weather tools",
    Endpoints = new
    {
        Mcp = "/mcp",
        Sse = "/sse",
        Health = "/health",
        Swagger = "/swagger"
    }
}));

app.Run();

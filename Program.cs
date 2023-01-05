using Microsoft.AspNetCore.Http.Json;
using MongoDB.Driver;
using WorkQueueAPI.Converters;
using WorkQueueAPI.Model;
using WorkQueueAPI.Services;
using static System.Formats.Asn1.AsnWriter;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("secrets.json");
builder.Services.Configure<JsonOptions>(options =>
    {
        options.SerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        options.SerializerOptions.PropertyNamingPolicy = null;
    });
builder.Services.AddScoped<WorkQueueService>();
builder.Services.AddScoped<MongoDBService>();

WebApplication app = builder.Build();


app.MapPost("/api/v1/work-queue", async (WorkQueueRequest request, WorkQueueService workQueueService) => {
    return await workQueueService.GetWorkQueueResponse(request);
});

app.Run();

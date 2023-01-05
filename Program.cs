using Microsoft.AspNetCore.Http.Json;
using MongoDB.Driver;
using WorkQueueAPI.Converters;
using WorkQueueAPI.Model;
using WorkQueueAPI.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("secrets.json");
builder.Services.Configure<JsonOptions>(options =>
    {
        options.SerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        options.SerializerOptions.PropertyNamingPolicy = null;
    });

WebApplication app = builder.Build();


String connectionString = builder.Configuration["ConnectionString"];
String databaseName = builder.Configuration["DatabaseName"];

MongoClient client = new MongoClient(
    connectionString
);

IMongoDatabase db = client.GetDatabase(databaseName);


app.MapPost("/api/v1/work-queue", (WorkQueueRequest request) => {
    return (new WorkQueueService()).GetWorkQueueResponse(db, request);
});

app.Run();

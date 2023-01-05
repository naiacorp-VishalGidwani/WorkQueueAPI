using Microsoft.AspNetCore.Http.Json;
using MongoDB.Driver;
using WorkQueueAPI.Converters;
using WorkQueueAPI.Model;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("secrets.json");
builder.Services.Configure<JsonOptions>(options =>
    {
        options.SerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        options.SerializerOptions.PropertyNamingPolicy = null;
    });

var app = builder.Build();


String connectionString = builder.Configuration["ConnectionString"];
String databaseName = builder.Configuration["DatabaseName"];

var client = new MongoClient(
    connectionString
);

var db = client.GetDatabase(databaseName);


app.MapPost("/api/v1/work-queue", (WorkQueueRequest request) => {
    return new WorkQueueResponse()
    {
    };
});

app.Run();

using MongoDB.Driver;
using WorkQueueAPI.Model;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("secrets.json");

String connectionString = builder.Configuration["ConnectionString"];
String databaseName = builder.Configuration["DatabaseName"];

var app = builder.Build();

var client = new MongoClient(
    connectionString
);

var db = client.GetDatabase(databaseName);

app.MapGet("/", () => "Hello World!");

app.MapGet("/work-queue", () => {
    return new WorkQueueResponse()
    {
        Id = 1,
        ConnectionString = connectionString,
        Database = databaseName
    };
});

app.Run();

using MongoDB.Driver;
using WorkQueueAPI.Converters;
using WorkQueueAPI.Model;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("secrets.json");
builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    });

String connectionString = builder.Configuration["ConnectionString"];
String databaseName = builder.Configuration["DatabaseName"];

var app = builder.Build();

var client = new MongoClient(
    connectionString
);

var db = client.GetDatabase(databaseName);


app.MapPost("/api/v1/work-queue", (WorkQueueRequest request) => {
    return request;
/*    return new WorkQueueResponse()
    {
    };*/
});

app.Run();

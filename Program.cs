using WorkQueueAPI.Model;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/work-queue", () => {
    return new WorkQueueResponse()
    {
        Id = 1,
    };
});

app.Run();

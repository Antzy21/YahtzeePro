var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Yahtzee Pro Game Api");

app.Run();

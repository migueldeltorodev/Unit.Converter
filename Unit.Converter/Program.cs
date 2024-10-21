var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles();

//Routes:
app.MapGet("/", () => Results.Content(Templates.GetHtmlContent("home"), "text/html"));

app.Run();

static async Task<IResult> ConvertLength(float length, string unitFrom, string unitTo)
{
}
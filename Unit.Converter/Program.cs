using Unit.Converter.Models;
using Unit.Converter.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConverterService, LengthConverter>();
builder.Services.AddSingleton<IConverterService, TemperatureConverter>();
builder.Services.AddSingleton<IConverterService, WeightConverter>();

var app = builder.Build();

app.UseStaticFiles();

//Routes:
app.MapGet("/", () => Results.Content(Templates.GetHtmlContent("home"), "text/html"));

foreach (var page in new[] { "length", "weight", "temperature" })
{
    var converter = app.Services.GetServices<IConverterService>()
        .First(s => s.GetType().Name.ToLower().StartsWith(page));

    app.MapGet($"/{page}", () =>
        Results.Content(Templates.GetHtmlContent(page, converter), "text/html"));

    app.MapPost($"/{page}", async (HttpContext context) =>
    {
        try
        {
            var form = await context.Request.ReadFormAsync();

            if (!double.TryParse(form["value"], out double value))
            {
                return Results.BadRequest("Invalid value");
            }

            var fromUnit = form["fromUnit"].ToString();
            var toUnit = form["toUnit"].ToString();

            if (string.IsNullOrEmpty(fromUnit) || string.IsNullOrEmpty(toUnit))
            {
                return Results.BadRequest("Invalid units");
            }

            var conversion = new UnitConversion(value, fromUnit, toUnit);
            var result = converter.Convert(conversion);

            // Solo devolver el HTML del resultado
            var resultHtml = $@"
            <div class='result show'>
                {result.OriginalValue} {result.FromUnit} = {result.ConvertedValue:F4} {result.ToUnit}
            </div>";

            return Results.Content(resultHtml, "text/html");
        }
        catch (Exception ex)
        {
            return Results.BadRequest($"Error: {ex.Message}");
        }
    });
}

app.Run();
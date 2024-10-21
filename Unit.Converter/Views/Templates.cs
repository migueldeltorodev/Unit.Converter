public static class Templates
{
    public static string GetHtmlContent(string page, IConverterService? converter = null, ConversionResult? result = null)
    {
        var commonHtml = @"
            <!DOCTYPE html>
            <html>
            <head>
                <title>Unit Converter</title>
                <style>
                    body { font-family: Arial, sans-serif; max-width: 800px; margin: 0 auto; padding: 20px; }
                    nav { margin-bottom: 20px; }
                    nav a { margin-right: 10px; }
                    form { margin-top: 20px; }
                    .result { margin-top: 20px; padding: 10px; background-color: #f0f0f0; }
                    select, input { margin: 5px; padding: 5px; }
                    button { padding: 5px 10px; }
                </style>
            </head>
            <body>
                <nav>
                    <a href='/'>Home</a>
                    <a href='/length'>Length</a>
                    <a href='/weight'>Weight</a>
                    <a href='/temperature'>Temperature</a>
                </nav>";

        var resultHtml = result == null ? "" : $@"
            <div class='result'>
                {result.OriginalValue} {result.FromUnit} = {result.ConvertedValue:F4} {result.ToUnit}
            </div>";

        var pageContent = page switch
        {
            "home" => @"
                <h1>Welcome to Unit Converter</h1>
                <p>Select a conversion type from the navigation above to get started.</p>",

            _ when converter != null => $@"
                <h1>{char.ToUpper(page[0]) + page[1..]} Converter</h1>
                <form method='post'>
                    <input type='number' step='any' name='value' placeholder='Enter value' required>
                    <select name='fromUnit'>
                        {GetOptions(converter.GetAvailableUnits())}
                    </select>
                    <span>to</span>
                    <select name='toUnit'>
                        {GetOptions(converter.GetAvailableUnits())}
                    </select>
                    <button type='submit'>Convert</button>
                </form>{resultHtml}",

            _ => "<h1>Page not found</h1>"
        };

        return commonHtml + pageContent + "</body></html>";
    }

    private static string GetOptions(IEnumerable<string> units) =>
        string.Join("", units.Select(unit =>
            $"<option value='{unit}'>{char.ToUpper(unit[0]) + unit[1..]}</option>"));
}
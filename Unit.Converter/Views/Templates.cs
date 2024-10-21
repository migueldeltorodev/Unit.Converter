using Unit.Converter.Models;
using Unit.Converter.Services;

public static class Templates
{
    public static string GetHtmlContent(string page, IConverterService? converter = null, ConversionResult? result = null)
    {
        var scriptContent = @"
            <script>
                document.addEventListener('DOMContentLoaded', function() {
                    const form = document.querySelector('form');
                    const resultDiv = document.querySelector('.result');
                    const errorDiv = document.querySelector('.error');

                    if (form) {
                        form.addEventListener('submit', async function(e) {
                            e.preventDefault();
                            console.log('Form submitted');

                            const value = parseFloat(form.querySelector('input[name=""value""]').value);
                            if (isNaN(value)) {
                                showError('Please enter a valid number');
                                return;
                            }

                            hideError();

                            try {
                                const formData = new FormData(form);
                                const searchParams = new URLSearchParams(formData);

                                console.log('Sending data:', Object.fromEntries(searchParams));

                                const response = await fetch(form.action, {
                                    method: 'POST',
                                    headers: {
                                        'Content-Type': 'application/x-www-form-urlencoded',
                                    },
                                    body: searchParams.toString()
                                });

                                console.log('Response status:', response.status);

                                if (!response.ok) {
                                    throw new Error(`HTTP error! status: ${response.status}`);
                                }

                                const html = await response.text();
                                console.log('Response HTML:', html);

                                // Actualizar solo el div de resultado
                                if (resultDiv) {
                                    const tempDiv = document.createElement('div');
                                    tempDiv.innerHTML = html;
                                    const newResult = tempDiv.querySelector('.result');

                                    if (newResult) {
                                        console.log('Found new result:', newResult.innerHTML);
                                        resultDiv.innerHTML = newResult.innerHTML;
                                        resultDiv.classList.add('show');
                                        resultDiv.style.display = 'block';
                                    } else {
                                        console.log('No result element found in response');
                                        showError('Unable to display result');
                                    }
                                } else {
                                    console.log('No result div found in current page');
                                }
                            } catch (error) {
                                console.error('Error:', error);
                                showError('An error occurred. Please try again.');
                            }
                        });
                    }

                    function showError(message) {
                        if (errorDiv) {
                            errorDiv.textContent = message;
                            errorDiv.style.display = 'block';
                        }
                        if (resultDiv) {
                            resultDiv.style.display = 'none';
                        }
                        console.log('Error shown:', message);
                    }

                    function hideError() {
                        if (errorDiv) {
                            errorDiv.style.display = 'none';
                        }
                    }
                });
            </script>";
        var commonHtml = @"
            <!DOCTYPE html>
            <html>
            <head>
                <title>Unit Converter</title>
                <style>
                    body {
                        font-family: Arial, sans-serif;
                        max-width: 800px;
                        margin: 0 auto;
                        padding: 20px;
                    }
                    nav {
                        margin-bottom: 20px;
                        background-color: #f8f9fa;
                        padding: 10px;
                        border-radius: 5px;
                    }
                    nav a {
                        margin-right: 10px;
                        text-decoration: none;
                        color: #333;
                        padding: 5px 10px;
                        border-radius: 3px;
                    }
                    nav a:hover {
                        background-color: #e9ecef;
                    }
                    form {
                        margin-top: 20px;
                        background-color: #f8f9fa;
                        padding: 20px;
                        border-radius: 5px;
                    }
                    .result {
                        margin-top: 20px;
                        padding: 15px;
                        background-color: #d4edda;
                        border: 1px solid #c3e6cb;
                        border-radius: 5px;
                        color: #155724;
                        display: none;
                    }
                    .result.show {
                        display: block;
                    }
                    select, input {
                        margin: 5px;
                        padding: 8px;
                        border: 1px solid #ced4da;
                        border-radius: 4px;
                    }
                    button {
                        padding: 8px 16px;
                        background-color: #007bff;
                        color: white;
                        border: none;
                        border-radius: 4px;
                        cursor: pointer;
                    }
                    button:hover {
                        background-color: #0056b3;
                    }
                    .converter-container {
                        display: flex;
                        flex-wrap: wrap;
                        gap: 10px;
                        align-items: center;
                    }
                    .error {
                        color: #721c24;
                        background-color: #f8d7da;
                        border: 1px solid #f5c6cb;
                        padding: 10px;
                        margin-top: 10px;
                        border-radius: 4px;
                        display: none;
                    }
                </style>" + scriptContent + @"
            </head>
            <body>
                <nav>
                    <a href='/'>Home</a>
                    <a href='/length'>Length</a>
                    <a href='/weight'>Weight</a>
                    <a href='/temperature'>Temperature</a>
                </nav>";

        var resultHtml = result == null ? "" : $@"
            <div class='result show'>
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
                <div class='converter-container'>
                    <input type='number' step='any' name='value' placeholder='Enter value' required>
                    <select name='fromUnit'>
                        {GetOptions(converter.GetAvailableUnits())}
                    </select>
                    <span>to</span>
                    <select name='toUnit'>
                        {GetOptions(converter.GetAvailableUnits())}
                    </select>
                    <button type='submit'>Convert</button>
                </div>
            </form>
            <div class='error'></div>
            <div class='result' style='display: none;'></div>",

            _ => "<h1>Page not found</h1>"
        };

        return commonHtml + pageContent + "</body></html>";
    }

    private static string GetOptions(IEnumerable<string> units) =>
        string.Join("", units.Select(unit =>
            $"<option value='{unit}'>{char.ToUpper(unit[0]) + unit[1..]}</option>"));
}
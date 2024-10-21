namespace Unit.Converter.Constants
{
    public static class ConversionFactors
    {
        public static readonly Dictionary<string, double> LengthToMeters = new()
        {
            ["millimeter"] = 0.001,
            ["centimeter"] = 0.01,
            ["meter"] = 1,
            ["kilometer"] = 1000,
            ["inch"] = 0.0254,
            ["foot"] = 0.3048,
            ["yard"] = 0.9144,
            ["mile"] = 1609.344
        };

        public static readonly Dictionary<string, double> WeightToGrams = new()
        {
            ["milligram"] = 0.001,
            ["gram"] = 1,
            ["kilogram"] = 1000,
            ["ounce"] = 28.3495,
            ["pound"] = 453.592
        };

        public static readonly string[] TemperatureUnits = { "celsius", "fahrenheit", "kelvin" };
    }
}
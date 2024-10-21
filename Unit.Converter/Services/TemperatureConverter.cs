using Unit.Converter.Constants;
using Unit.Converter.Models;

namespace Unit.Converter.Services
{
    public class TemperatureConverter : IConverterService
    {
        public ConversionResult Convert(UnitConversion conversion)
        {
            double result = conversion switch
            {
                { FromUnit: "celsius", ToUnit: "fahrenheit" } => (conversion.Value * 9 / 5) + 32,
                { FromUnit: "celsius", ToUnit: "kelvin" } => conversion.Value + 273.15,
                { FromUnit: "fahrenheit", ToUnit: "celsius" } => (conversion.Value - 32) * 5 / 9,
                { FromUnit: "fahrenheit", ToUnit: "kelvin" } => (conversion.Value - 32) * 5 / 9 + 273.15,
                { FromUnit: "kelvin", ToUnit: "celsius" } => conversion.Value - 273.15,
                { FromUnit: "kelvin", ToUnit: "fahrenheit" } => (conversion.Value - 273.15) * 9 / 5 + 32,
                _ => conversion.Value
            };

            return new ConversionResult(conversion.Value, conversion.FromUnit, result, conversion.ToUnit);
        }

        public IEnumerable<string> GetAvailableUnits() => ConversionFactors.TemperatureUnits;
    }
}
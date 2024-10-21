using Unit.Converter.Constants;
using Unit.Converter.Models;

namespace Unit.Converter.Services
{
    public class LengthConverter : IConverterService
    {
        public ConversionResult Convert(UnitConversion conversion)
        {
            var fromFactor = ConversionFactors.LengthToMeters[conversion.FromUnit];
            var toFactor = ConversionFactors.LengthToMeters[conversion.ToUnit];
            var meters = conversion.Value * fromFactor;
            var result = meters / toFactor;

            return new ConversionResult(conversion.Value, conversion.FromUnit, result, conversion.ToUnit);
        }

        public IEnumerable<string> GetAvailableUnits() => ConversionFactors.LengthToMeters.Keys;
    }
}
using Unit.Converter.Constants;
using Unit.Converter.Models;

namespace Unit.Converter.Services
{
    public class WeightConverter : IConverterService
    {
        public ConversionResult Convert(UnitConversion conversion)
        {
            var fromFactor = ConversionFactors.WeightToGrams[conversion.FromUnit];
            var toFactor = ConversionFactors.WeightToGrams[conversion.ToUnit];
            var grams = conversion.Value * fromFactor;
            var result = grams / toFactor;

            return new ConversionResult(conversion.Value, conversion.FromUnit, result, conversion.ToUnit);
        }

        public IEnumerable<string> GetAvailableUnits() => ConversionFactors.WeightToGrams.Keys;
    }
}
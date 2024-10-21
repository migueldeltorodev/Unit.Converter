namespace Unit.Converter.Models
{
    public record ConversionResult(double OriginalValue, string FromUnit, double ConvertedValue, string ToUnit);
}
using Unit.Converter.Models;

namespace Unit.Converter.Services
{
    public interface IConverterService
    {
        ConversionResult Convert(UnitConversion conversion);

        IEnumerable<string> GetAvailableUnits();
    }
}
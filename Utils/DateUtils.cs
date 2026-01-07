using System.Globalization;

namespace APIArenaAuto.Utils
{
    public static class DateUtils
    {
        public static DateTime? Converter(string? data)
        {
            if (string.IsNullOrWhiteSpace(data))
                return null;

            // formatos aceitos
            var formatos = new[]
            {
                "dd/MM/yyyy",
                "yyyy-MM-dd",
                "dd-MM-yyyy",
                "yyyy/MM/dd"
            };

            if (DateTime.TryParseExact(
                data,
                formatos,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var dataConvertida))
            {
                return dataConvertida;
            }

            throw new Exception("Data inválida. Use dd/MM/yyyy ou yyyy-MM-dd");
        }
    }
}

using System.Text.RegularExpressions;

namespace APIArenaAuto.Utils
{
    public static class StringUtils
    {
        public static string? SomenteNumeros(string? valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return null;

            return Regex.Replace(valor, @"\D", "");
        }
    }
}

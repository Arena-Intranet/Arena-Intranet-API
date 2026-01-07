using System.Text.RegularExpressions;

namespace APIArenaAuto.Utils
{
    public static class CpfUtils
    {
        public static string? LimparCpf(string? cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return null;

            return Regex.Replace(cpf, @"\D", "");
        }

        public static bool CpfValido(string? cpf)
        {
            cpf = LimparCpf(cpf);

            if (string.IsNullOrEmpty(cpf) || cpf.Length != 11)
                return false;

            // Evita CPFs com todos números iguais
            if (new string(cpf[0], 11) == cpf)
                return false;

            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf[..9];
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += (tempCpf[i] - '0') * multiplicador1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            string digito = resto.ToString();
            tempCpf += digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += (tempCpf[i] - '0') * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            digito += resto.ToString();

            return cpf.EndsWith(digito);
        }
    }
}

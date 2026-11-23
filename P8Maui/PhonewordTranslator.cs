using System.Text;

namespace P8Maui;

// Module 1 du parcours MAUI (application "Phoneword") :
// convertit un numéro contenant des lettres (ex. "1-555-NETMAUI") en chiffres,
// selon les lettres imprimées sur un clavier de téléphone.
public static class PhonewordTranslator
{
    private static readonly string[] Digits = { "ABC", "DEF", "GHI", "JKL", "MNO", "PQRS", "TUV", "WXYZ" };

    // Renvoie null si le texte est vide ou contient un caractère non traduisible.
    public static string? ToNumber(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return null;

        raw = raw.ToUpperInvariant();
        var newNumber = new StringBuilder();

        foreach (var c in raw)
        {
            if (" -0123456789".Contains(c))
            {
                newNumber.Append(c);
            }
            else
            {
                var result = TranslateToNumber(c);
                if (result is null)
                    return null;
                newNumber.Append(result);
            }
        }

        return newNumber.ToString();
    }

    // "ABC" -> 2, "DEF" -> 3, ... "WXYZ" -> 9
    private static int? TranslateToNumber(char c)
    {
        for (var i = 0; i < Digits.Length; i++)
        {
            if (Digits[i].Contains(c))
                return 2 + i;
        }
        return null;
    }
}

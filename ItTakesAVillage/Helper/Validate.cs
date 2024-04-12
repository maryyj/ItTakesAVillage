using ItTakesAVillage.Contracts;
using ItTakesAVillage.Models;

namespace ItTakesAVillage.Helper
{
    public static class Validate
    {
        public static string NormalizeName(string name)
        {
            string lowerCaseName = name.ToLower();
            string trimmedName = lowerCaseName.Trim();

            string normalizedName = new string(trimmedName
                .Where(c => Char.IsLetter(c) || Char.IsWhiteSpace(c))
                .ToArray());

            return normalizedName;
        }
    }
}

namespace ItTakesAVillage.Helper;

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
    public static void ValidateReturnDate(List<ToolLoan> loans)
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);
        foreach (var loan in loans)
        {
            if (loan.ToDate < today && loan.IsReturned == false)
            {
                loan.ToolPool.IsBorrowed = false;
                loan.IsReturned = true;
            }
        }
    }
}

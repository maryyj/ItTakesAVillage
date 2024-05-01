

namespace ItTakesAVillage.Models;

public class ItTakesAVillageUser : IdentityUser
{
    [PersonalData]
    public string FirstName { get; set; } = string.Empty;

    [PersonalData]
    public string LastName { get; set; } = string.Empty;

    public ICollection<UserGroup>? UserGroups { get; set; }

}


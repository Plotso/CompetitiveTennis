namespace CompetitiveTennis.Identity.Models;

public record FullUserOutputModel(string Token, string Email, string Username, string FirstName, string LastName, bool HasAdministrativeRights);
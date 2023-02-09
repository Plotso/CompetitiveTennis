namespace CompetitiveTennis.Services.Interfaces;

public interface ICurrentTokenService
{
    string Get();

    void Set(string token);
}
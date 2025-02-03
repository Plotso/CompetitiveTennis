namespace CompetitiveTennis.Tournaments.Services.Interfaces.BL;

using Contracts.Account;

public interface IAccountStatsProvider
{
    ValueTask<AccountStats> GetAccountStats(string username);
}
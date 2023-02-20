namespace CompetitiveTennis.Data.Models;

using Interfaces;

public class BaseTestEntity<TKey> : BaseDeletableEntity<TKey>, ITestEntity
{
    public bool IsTest { get; set; }
}
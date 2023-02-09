namespace CompetitiveTennis.Data.Models;

using Interfaces;

public class BaseTestEntity : ITestEntity
{
    public bool IsTest { get; set; }
}
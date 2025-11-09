/// <summary>
///     Dependency injection to provide economy functions.
/// </summary>
public interface IEconomyValidator
{
    bool CanAfford(int amount);
}
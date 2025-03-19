namespace BattleshipsCLI;

public class ShipPlacementException: Exception
{
    public ShipPlacementException()
    {
    }

    public ShipPlacementException(string message)
        : base(message)
    {
    }

    public ShipPlacementException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
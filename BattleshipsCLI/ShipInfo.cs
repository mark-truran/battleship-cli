namespace BattleshipsCLI;

public class ShipInfo
{
    public required string Name { get; init; }
    public int Size { get; init; } 
    public int Count { get; init; }
    public char Symbol => !string.IsNullOrWhiteSpace(Name) && char.IsLetter(Name[0]) ? Name[0] : '?';
}
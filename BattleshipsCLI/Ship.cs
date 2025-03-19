using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BattleshipsCLITests")]

namespace BattleshipsCLI;

internal class Ship(string name, List<(int, int)> positions)
{
    public string Name { get; } = name;
    public List<(int row, int col)> Positions { get; } = positions;

    internal readonly HashSet<(int row, int col)> Hits = [];

    public void RegisterHit(int row, int col)
    {
        if (Positions.Contains((row, col)))
            Hits.Add((row, col));
    }

    public bool IsSunk() => Hits.Count == Positions.Count;
}
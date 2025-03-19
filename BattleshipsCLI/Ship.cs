namespace BattleshipsCLI;

public class Ship(string name, List<(int, int)> positions)
{
    public string Name { get; } = name;
    public List<(int row, int col)> Positions { get; } = positions;
    
    private readonly HashSet<(int row, int col)> _hits = [];

    public void RegisterHit(int row, int col)
    {
        if (Positions.Contains((row, col)))
            _hits.Add((row, col));
    }

    public bool IsSunk() => _hits.Count == Positions.Count;
    
    public HashSet<(int row, int col)> GetHits() => _hits;
}
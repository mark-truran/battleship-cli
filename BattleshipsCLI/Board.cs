using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BattleshipCLITests")]

namespace BattleshipsCLI;

internal class Board(int rows, int columns, ShipInfo[] fleetInfo, int shipPlacementRetryLimit = 10)
{
    internal readonly char[,] Grid = new char[rows, columns];

    internal List<Ship> Ships { get; } = [];
    
    public void Initialise()
    {
        for (var i = 0; i < rows; i++)
            for (var j = 0; j < columns; j++)
                Grid[i, j] = '~'; 
    }
    
    public void PlaceShips()
    {
        var random = new Random();

        foreach (var shipInfo in fleetInfo)
        {
            for (var j = 0; j < shipInfo.Count; j++)
            {
                var placed = false;
                var placementAttempt = 0; 
                
                while (!placed)
                {
                    var row = random.Next(rows);
                    var col = random.Next(columns);
                    var horizontal = random.Next(2) == 0;
                
                    var positions = new List<(int, int)>();

                    for (var i = 0; i < shipInfo.Size; i++)
                    {
                        var r = row + (horizontal ? 0 : i);
                        var c = col + (horizontal ? i : 0);

                        if (r >= rows || c >= columns || Grid[r, c] != '~')
                            break;

                        positions.Add((r, c));
                    }

                    if (positions.Count == shipInfo.Size)
                    {
                        Ships.Add(new Ship(shipInfo.Name, positions));
                        foreach (var (r, c) in positions)
                        {
                            Grid[r, c] = shipInfo.Symbol; 
                        }
                        placed = true;
                    }
                    else
                    {
                        placementAttempt++;
                        if (placementAttempt > shipPlacementRetryLimit) 
                            throw new ShipPlacementException($"Unable to place {shipInfo.Name}. Increase grid size or reduce number of ships.");
                    }
                }
            }
        }
    }
    
    public void PrintBoard(bool showShips = false)
    {
        Console.Write("   "); 
        for (var i = 1; i <= columns; i++)
        {
            var spacer = i >= 10 ? " " : "  ";
            Console.Write(i + spacer); 
        }
        Console.WriteLine();

        for (var i = 0; i < rows; i++)
        {
            Console.Write((char)('A' + i) + "  "); 
            for (var j = 0; j < columns; j++)
            {
                var displayChar = (fleetInfo.Select(c => c.Symbol).Contains(Grid[i, j]) && !showShips) ? '~' : Grid[i, j];
                Console.Write($"{displayChar}  ");
            }
            Console.WriteLine();
        }
    }
    
    public bool FireShot(string? target)
    {
        if (string.IsNullOrEmpty(target) || target.Length < 2)
            return false;

        var rowChar = char.ToUpper(target[0]);
        var rowIndex = rowChar - 'A';
        if (!int.TryParse(target.AsSpan(1), out var colIndex) || colIndex < 1 || colIndex > columns) 
            return false;

        colIndex -= 1; 
        if (rowIndex < 0 || rowIndex >= rows || colIndex < 0 || colIndex >= columns)
            return false;

        if (Grid[rowIndex, colIndex] == 'X' || Grid[rowIndex, colIndex] == 'O')
            return false; 

        foreach (var ship in Ships.Where(ship => ship.Positions.Contains((rowIndex, colIndex))))
        {
            ship.RegisterHit(rowIndex, colIndex);
            Grid[rowIndex, colIndex] = 'X';
            Console.WriteLine("Hit!");

            if (ship.IsSunk())
                Console.WriteLine($"You sank the {ship.Name}!");

            return true;
        }

        Grid[rowIndex, colIndex] = 'O'; 
        Console.WriteLine("Miss!");
        return true;
    }
    
    public bool AllShipsSunk() => Ships.All(s => s.IsSunk());
    
    public IEnumerable<char> GetFlattenedGrid() => Grid.Cast<char>();
}
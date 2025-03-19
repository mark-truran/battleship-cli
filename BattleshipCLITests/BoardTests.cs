using BattleshipsCLI;

namespace BattleshipCLTest;

public class BoardTests
{
    [Fact]
    public void Initialise_ShouldPopulate2DAreaWithTildeCharacter()
    {
        // Arrange
        const int rows = 5;
        const int columns = 5;

        var board = new Board(rows, columns, []);

        // Act
        board.Initialise();
        var result = board.GetFlattenedGrid();

        // Assert
        Assert.Equal((rows * columns), result.Count(c => c == '~'));
    }
    
    [Fact]
    public void PlaceShips_ShouldPlaceOneBattleship()
    {
        // Arrange
        const int rows = 10;
        const int columns = 10;
        
        var shipInfo = new ShipInfo()
        {
            Name = "Battleship",
            Size = 4,
            Count = 1
        };
        
        var board = new Board(rows, columns, [shipInfo]);

        // Act
        board.Initialise();
        board.PlaceShips();
        var characterCounts = board.GetFlattenedGrid()
            .GroupBy(c => c)
            .Select(g => new { Character = g.Key, Count = g.Count() }).ToList();

        // Assert
        Assert.Equal((rows * columns) - (shipInfo.Size),
            characterCounts.Where(c => c.Character == '~').Sum(c => c.Count));
        Assert.Equal(shipInfo.Size, characterCounts.Where(c => c.Character == 'B').Sum(c => c.Count));
    }
    
    [Fact]
    public void PlaceShips_ShouldPlaceOneBattleshipAndTwoDestroyers()
    {
        // Arrange
        const int rows = 10;
        const int columns = 10;
        
        var battleship = new ShipInfo()
        {
            Name = "Battleship",
            Size = 4,
            Count = 1
        };
        
        var destroyers = new ShipInfo()
        {
            Name = "Destroyer",
            Size = 3,
            Count = 2
        };
        
        var grid = new Board(rows, columns, [battleship, destroyers]);

        // Act
        grid.Initialise();
        grid.PlaceShips();
        var characterCounts = grid.GetFlattenedGrid()
            .GroupBy(c => c)
            .Select(g => new { Character = g.Key, Count = g.Count() }).ToList();

        // Assert
        Assert.Equal((rows * columns) - (battleship.Size + destroyers.Size + destroyers.Size),
            characterCounts.Where(c => c.Character == '~').Sum(c => c.Count));
        Assert.Equal(destroyers.Size + destroyers.Size, characterCounts.Where(c => c.Character == 'D').Sum(c => c.Count));
        Assert.Equal(battleship.Size, characterCounts.Where(c => c.Character == 'B').Sum(c => c.Count));
    }
    
    [Theory]
    [InlineData("A1", true)]   // Valid lower boundary
    [InlineData("B5", true)]   // Valid middle value
    [InlineData("J10", true)]  // Valid upper boundary in a 10x10 grid
    [InlineData("K1", false)]  // Invalid row (out of bounds)
    [InlineData("A0", false)]  // Invalid column (too low)
    [InlineData("A11", false)] // Invalid column (too high for a 10x10 grid)
    [InlineData("1A", false)]  // Invalid format (number before letter)
    [InlineData("AA1", false)] // Invalid format (double letters)
    [InlineData("", false)]    // Empty string
    [InlineData(null, false)]  // Null input
    [InlineData("Z5", false)]  // Row out of bounds for 10x10
    [InlineData("a7", true)]   // Lowercase row (should be valid)
    public void FiresShot_ValidatesCoorindatesProperly(string? target, bool expected)
    {
        // Arrange
        var playerGrid = new Board(10, 10, [], 10);
        playerGrid.Initialise();

        // Act
        var result = playerGrid.FireShot(target);

        // Assert
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public void PlaceShips_ImpossibleShipPlacementShouldThrowException()
    {
        // Arrange
        const int rows = 5;
        const int columns = 5;
        const int shipPlacementRetryLimit = 1;
        
        var battleships = new ShipInfo()
        {
            Name = "Battleship",
            Size = 4,
            Count = 10
        };
        
        var board = new Board(rows, columns, [battleships], shipPlacementRetryLimit);

        // Act + Assert
        board.Initialise();
        var exception = Assert.Throws<ShipPlacementException>(() => board.PlaceShips());
        Assert.Equal($"Unable to place {battleships.Name}. Increase grid size or reduce number of ships.", exception.Message);
    }
    
    [Fact]
    public void FireShot_ShouldWriteAnXToCorrectBoardPositionWhenAShipIsHit()
    {
        // Arrange
        const int rows = 5;
        const int columns = 5;
        
        const int shipX = 0;
        const int shipY = 0;
        
        var board = new Board(rows, columns, []);
        board.Initialise();
        
        var ship = new Ship("Dingy", [(shipX, shipY)]);
        board.Ships.Add(ship);
        
        // Act
        board.FireShot("A1"); 

        // Assert
        Assert.Equal('X',board.GetGrid()[shipX,shipY]);
    }
    
    [Fact]
    public void FireShot_ShouldWriteAnOToCorrectBoardPositionWhenAShipIsHit()
    {
        // Arrange
        const int rows = 5;
        const int columns = 5;
        
        const int shipX = 0;
        const int shipY = 0;
        
        const int shotX = 0;
        const int shotY = 1;
        
        var board = new Board(rows, columns, []);
        board.Initialise();
        
        var ship = new Ship("Dingy", [(shipX, shipY)]);
        board.Ships.Add(ship);
        
        // Act
        board.FireShot("A2"); 

        // Assert
        Assert.Equal('O',board.GetGrid()[shotX,shotY]);
    }
    
    [Fact]
    public void AllShipsSunk_ShouldReturnFalse_WhenAtLeastOneShipIsNotSunk()
    {
        // Arrange
        var ship1 = new Ship("Destroyer", [(1, 1), (1, 2)]);
        var ship2 = new Ship("Submarine", [(2, 2), (2, 3)]);

        // Ship 1 is fully hit
        ship1.RegisterHit(1, 1);
        ship1.RegisterHit(1, 2);

        // Ship 2 is partially hit (not sunk)
        ship2.RegisterHit(2, 2);

        var board = new Board(5, 5, []);
        board.Ships.Add(ship1);
        board.Ships.Add(ship2);
        
        // Act
        var result = board.AllShipsSunk();

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void AllShipsSunk_ShouldReturnTrue_WhenAllShipsAreSunk()
    {
        // Arrange
        var ship1 = new Ship("Destroyer", [(1, 1), (1, 2)]);
        var ship2 = new Ship("Submarine", [(2, 2), (2, 3)]);

        // Ship 1 is fully hit
        ship1.RegisterHit(1, 1);
        ship1.RegisterHit(1, 2);

        // Ship 2 is partially hit (not sunk)
        ship2.RegisterHit(2, 2);
        ship2.RegisterHit(2, 3);

        var board = new Board(5, 5, []);
        board.Ships.Add(ship1);
        board.Ships.Add(ship2);
        
        // Act
        var result = board.AllShipsSunk();

        // Assert
        Assert.True(result);
    }
}
using BattleshipsCLI;

namespace BattleshipCLTest;

public class ShipTests
{
    [Fact]
    public void RegisterHit_ShouldRecordHit_WhenHitIsOnShip()
    {
        // Arrange
        var ship = new Ship("Battleship", [(1, 1), (1, 2), (1, 3)]);

        // Act
        ship.RegisterHit(1, 2);

        // Assert
        Assert.Contains((1, 2), ship.Hits);
    }

    [Fact]
    public void RegisterHit_ShouldNotRecordHit_WhenHitIsNotOnShip()
    {
        // Arrange
        var ship = new Ship("Battleship", [(1, 1), (1, 2), (1, 3)]);

        // Act
        ship.RegisterHit(3, 3);

        // Assert
        Assert.Empty(ship.Hits); // No valid hit was registered
    }
    
    [Fact]
    public void IsSunk_ShouldReturnFalse_WhenNotAllPositionsAreHit()
    {
        // Arrange
        var ship = new Ship("Battleship", [(1, 1), (1, 2), (1, 3)]);

        // Act
        ship.RegisterHit(1, 2);

        // Assert
        Assert.False(ship.IsSunk());
    }

    [Fact]
    public void IsSunk_ShouldReturnTrue_WhenAllPositionsAreHit()
    {
        // Arrange
        var ship = new Ship("Battleship", [(1, 1), (1, 2), (1, 3)]);

        // Act
        ship.RegisterHit(1, 1);
        ship.RegisterHit(1, 2);
        ship.RegisterHit(1, 3);

        // Assert
        Assert.True(ship.IsSunk());
    }
}
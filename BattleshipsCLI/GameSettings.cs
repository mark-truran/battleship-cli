using BattleshipsCLI;

namespace BattleshipsCLI;

public class GameSettings
{
    public int Rows { get; set; }
    public int Columns { get; set; }
    public bool Debug { get; set; }
    public int ShipPlacementRetryLimit { get; set; }
    public required ShipInfo[] FleetInfo { get; set; }
    public required ShipInfo[] ClassicFleetInfo { get; set; }
}
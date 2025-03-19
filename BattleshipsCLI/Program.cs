using BattleshipsCLI;
using Cocona;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var builder = CoconaApp.CreateBuilder();
builder.Configuration.AddJsonFile("appsettings.json", optional: false);
builder.Services.Configure<GameSettings>(builder.Configuration.GetSection("GameSettings"));
var app = builder.Build();

app.AddCommand(([Option('d', Description = "Reveal hidden ships")] bool? debug, [Option('c', Description = "Number of columns in the grid")] int? columns, 
    [Option('r', Description = "Number of rows in the grid")] int? rows, [Option('m', Description = "Switch on classic Hasbro mode")] bool? hasbroMode, IOptions<GameSettings> options) => {   
        
    var settings = options.Value;
    
    if (debug.HasValue)
        settings.Debug = debug.Value;
        
    if (columns.HasValue)
        settings.Columns = columns.Value;
        
    if (rows.HasValue)
        settings.Columns = rows.Value;
    
    var board = new Board(settings.Rows, settings.Columns, hasbroMode.HasValue ? settings.ClassicFleetInfo : settings.FleetInfo, settings.ShipPlacementRetryLimit);
    board.Initialise();

    try
    {
        Console.Clear();
        board.PlaceShips();
        Console.WriteLine("Welcome to Battleship!");
        Console.WriteLine("Enter coordinates (e.g., A6, B4). Type 'q' to exit.");
        
        while (!board.AllShipsSunk())
        {
            board.PrintBoard(settings.Debug);
        
            Console.Write("\nCoordinates: ");
            var input = Console.ReadLine()?.Trim().ToUpper();

            if (input == "Q") break;

            if (!board.FireShot(input))
            {
                Console.WriteLine("Invalid shot. Try again.");
            }
        }
        Console.WriteLine("Congratulations! You sank all the ships!");
        board.PrintBoard(true); 
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        Environment.Exit(1);
    }
});

app.Run();
# Battleships CLI Game

## Description
Battleships is a command-line implementation of the classic strategy game written in C#. The player tries to guess the location of enemy ships on a 2D grid. The objective is to sink all enemy ships as fast as possible.

## Features
- Single-player mode against a computer opponent
- Customizable grid size
- Randomized ship placements
- Simple UI for gameplay feedback

## Installation
To run the game, follow these steps:

1. Clone this repository:
   ```sh
   git clone https://github.com/mark-truran/battleship-cli.git
   ```
2. Navigate to the solution directory:
   ```sh
   cd battleship-cli
   ```
3. Ensure you have .NET SDK installed (version 9.0 or later).
4. Build the project:
   ```sh
   dotnet build
   ```
5. Run the tests
    ```sh
    dotnet test
    ```
5. Run the game:
   ```sh
   cd BattleshipsCLI
   dotnet run
   ```
6. For additional help:
    ```sh
   dotnet run -- -h
   ```

## How to Play
1. The computer will generate a 2D grid with ships randomly placed.
2. The player guesses the coordinates of the hidden ships (e.g., `A5`).
3. The computer will respond with "Hit" or "Miss".
4. The objective is to sink the computers' ships as fast as possible.
5. The game ends when all the computers' ships are destroyed.

## Input
- Enter coordinates using a letter (row) and number (column), e.g., `B3`.
- Type `q` to quit the game.

## Configuration
You can configure the application in two different ways:

You can change the following settings via appsettings:
- The number of rows in the 2D grid
- The number of columns in the 2D grid
- The type and number of ships the computer will place in the 2D grid
- Debug mode (allows you to see the hidden ships, for testing)
- The number of times the computer will try to place a ship (can be an issue if the grid is very small and the number of ships is large)

You can change the following settings via command line arguments:
- The number of rows in the 2D grid
- The number of columns in the 2D grid
- Debug mode (allows you to see the hidden ships, for testing)
- Turn classic mode on (uses classic 1990 Milton Bradley set up)

## Game Mode
Ships could be hidden in any square marked '~'. An 'X' means your shot was a hit. An 'O' means your shot was a miss. 

```console
Welcome to Battleship!
Enter coordinates (e.g., A6, B4). Type 'q' to exit.
   1  2  3  4  5  6  7  8  9  10
A  ~  ~  ~  ~  ~  ~  ~  ~  ~  ~
B  ~  ~  ~  ~  ~  ~  ~  ~  ~  ~
C  ~  ~  ~  ~  ~  ~  ~  ~  ~  ~
D  ~  ~  ~  ~  O  ~  ~  ~  ~  ~
E  ~  ~  ~  ~  ~  ~  ~  ~  ~  ~
F  ~  ~  ~  ~  ~  ~  ~  ~  ~  ~
G  ~  O  ~  ~  ~  ~  ~  ~  ~  ~
H  ~  ~  ~  ~  ~  ~  ~  ~  ~  ~
I  ~  ~  ~  ~  ~  ~  ~  ~  ~  ~
J  ~  ~  ~  ~  ~  ~  X  ~  ~  ~
```

## Debug Mode
In debug mode, all ships start the game revealed. Useful for debugging.
```console
Welcome to Battleship!
Enter coordinates (e.g., A6, B4). Type 'q' to exit.
   1  2  3  4  5  6  7  8  9  10
A  ~  ~  ~  ~  ~  ~  ~  ~  ~  ~
B  ~  ~  ~  ~  ~  ~  ~  ~  ~  ~
C  ~  D  ~  ~  ~  ~  ~  ~  ~  ~
D  ~  D  ~  ~  ~  ~  ~  ~  ~  ~
E  ~  D  ~  ~  ~  ~  ~  ~  ~  ~
F  ~  ~  ~  ~  ~  ~  ~  ~  ~  ~
G  ~  ~  ~  ~  ~  ~  ~  ~  ~  ~
H  ~  ~  ~  ~  ~  ~  D  ~  ~  ~
I  ~  ~  ~  ~  ~  ~  D  ~  ~  ~
J  B  B  B  B  ~  ~  D  ~  ~  ~
```
## Command Line Arguments
```console
Usage: BattleshipsCL [--debug] [--columns <Int32>] [--rows <Int32>] [--hasbro-mode] [--help] [--version]

BattleshipsCL

Options:
  -d, --debug              Reveal hidden ships
  -c, --columns <Int32>    Number of columns in the grid
  -r, --rows <Int32>       Number of rows in the grid
  -m, --hasbro-mode        Switch on classic Hasbro mode
  -h, --help               Show help message
  --version                Show version
```
Make sure you provide a double dash when calling these commands e.g.

```console
dotnet run -- --help
```
This tells 'dotnet run' that any arguments following it should be passed directly to the application instead of being interpreted by 'dotnet run' itself.

## Notes
This app uses Cocona,a micro-framework for .NET Core console application. Cocona makes it easy and fast to build console
applications on .NET. Cocona helps with dependency injection, command line argument parsing and more. See further https://github.com/mayuki/Cocona

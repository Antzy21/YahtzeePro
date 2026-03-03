using Microsoft.Extensions.Logging;
using YahtzeePro.Core;
using YahtzeePro.Core.Models;

namespace YahtzeePro.Play;

public class GameManagerService(ILogger<IGameManagerService> logger) : IGameManagerService
{
    private readonly Random _random = new();
    private readonly ILogger<IGameManagerService> _logger = logger;

    public void MakeMove(Game game, MoveChoice move)
    {
        _logger.LogInformation("Making {move} move, on {game}", move, game.GameState);
        if (move == MoveChoice.Safe)
        {
            if (game.GameState.IsStartOfTurn)
            {
                // Roll at start of turn
                var rolledDice = DiceCombinationGenerator.Generate(game.GameState.GameConfiguration.TotalDice, _random);
                game.ResolveRolledDice(rolledDice);
            }
            else
            {
                game.TurnMoves.Add(new TurnMove(game.Turns, MoveChoice.Safe, null));
                game.Turns++;
                game.GameState = game.GameState.Bank();
            }
        }
        else if (move == MoveChoice.Risky)
        {
            var rolledDice = DiceCombinationGenerator.Generate(game.GameState.DiceToRoll, _random);
            game.TurnMoves.Add(new TurnMove(game.Turns, MoveChoice.Risky, rolledDice));
            game.ResolveRolledDice(rolledDice);
        }
        _logger.LogInformation("Last dice roll: {lastDiceRoll}", game.TurnMoves?.ToString() ?? "Banked");
        _logger.LogInformation("Game state after making {move} move: {game}", move, game.GameState);
    }
}

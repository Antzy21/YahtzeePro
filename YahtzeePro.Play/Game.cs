using System.Diagnostics.CodeAnalysis;
using YahtzeePro.Core;
using YahtzeePro.Core.Models;
using YahtzeePro.Play.Players;

namespace YahtzeePro.Play;

public class Game
{
    private readonly IPlayer _player1;
    private readonly IPlayer _player2;

    private readonly Random _random = new();

    private int _currentPlayerId = 0;

    public GameState GameState;

    public Game(GameConfiguration gameConfiguration, IPlayer player1, IPlayer player2)
    {
        _player1 = player1;
        _player2 = player2;

        GameState = new(
            PlayerScore: 0,
            OpponentScore: 0,
            CachedScore: 0,
            DiceToRoll: gameConfiguration.TotalDice,
            IsStartOfTurn: true,
            GameConfiguration: gameConfiguration);
    }

    public IPlayer GetCurrentPlayer()
    {
        return _currentPlayerId == 0 ? _player1 : _player2;
    }

    public bool GameIsOver([NotNullWhen(true)] out GameResult? gameResult)
    {
        gameResult = null;
        if (GameState.OpponentScore >= GameState.GameConfiguration.WinningValue)
        {
            gameResult = new(GameState.OpponentScore, GameState.PlayerScore, GetOpponent().Name);
            return true;
        }
        return false;
    }

    public void MakeMove(MoveChoice move)
    {
        switch (move)
        {
            case MoveChoice.Safe:
                GameState = MakeSafeMove(GameState);
                break;
            case MoveChoice.Risky:
                GameState = MakeRiskyMove(GameState);
                break;
        }
    }

    private void SwitchPlayer()
    {
        _currentPlayerId ++;
        if (_currentPlayerId >= 2)
        {
            _currentPlayerId = 0;
        }
    }

    private GameState MakeRiskyMove(GameState gs)
    {
        var rolledDice = DiceCombination.Generate(gs.DiceToRoll, _random);

        return ResolveRolledDice(rolledDice, gs);
    }

    private GameState MakeSafeMove(GameState gs)
    {
        if (gs.IsStartOfTurn)
        {
            // Roll at start of turn
            var rolledDice = DiceCombination.Generate(GameState.GameConfiguration.TotalDice, _random);
            return ResolveRolledDice(rolledDice, gs);
        }
        else
        {
            SwitchPlayer();
            return gs.Bank();
        }
        throw new NotImplementedException();
    }

    private GameState ResolveRolledDice(DiceCombination rolledDice, GameState gs)
    {
        if (rolledDice.Score == 0)
        {
            // Fail
            SwitchPlayer();
            return gs.Fail();
        }
        else if (rolledDice.AllDiceAreScoring)
        {
            return gs.RollOver(rolledDice.Score);
        }
        else
        {
            return gs.AddRolledScore(rolledDice.Score, rolledDice.NumberOfScoringDice);
        }
    }

    private IPlayer GetOpponent() => _currentPlayerId == 0 ? _player2 : _player1;
}
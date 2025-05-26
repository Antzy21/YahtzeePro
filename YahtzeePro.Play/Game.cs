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

    private IPlayer _currentPlayer;

    private GameState _gameState;

    public Game(GameConfiguration gameConfiguration, IPlayer player1, IPlayer player2)
    {
        _player1 = player1;
        _player2 = player2;

        _currentPlayer = _player1;

        _gameState = new(
            PlayerScore: 0,
            OpponentScore: 0,
            CachedScore: 0,
            DiceToRoll: gameConfiguration.TotalDice,
            IsStartOfTurn: true,
            GameConfiguration: gameConfiguration);
    }

    public bool GameIsOver([NotNullWhen(true)] out GameResult? gameResult)
    {
        gameResult = null;
        if (_gameState.OpponentScore >= _gameState.GameConfiguration.WinningValue)
        {
            gameResult = new(_gameState.OpponentScore, _gameState.PlayerScore, GetOpponent().Name);
            return true;
        }
        return false;
    }

    public void GetAndMakeMove()
    {
        var move = _currentPlayer.GetMove(_gameState, _gameState.GameConfiguration);
        MakeMove(move);
    }

    public void MakeMove(MoveChoice move)
    {
        switch (move)
        {
            case MoveChoice.Safe:
                _gameState = MakeSafeMove(_gameState);
                break;
            case MoveChoice.Risky:
                _gameState = MakeRiskyMove(_gameState);
                break;
        }
    }

    private void SwitchPlayer()
    {
        if (_currentPlayer == _player1)
        {
            _currentPlayer = _player2;
        }
        else
        {
            _currentPlayer = _player1;
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
            var rolledDice = DiceCombination.Generate(_gameState.GameConfiguration.TotalDice, _random);
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

    private IPlayer GetOpponent() => _currentPlayer == _player1 ? _player2 : _player1;
}
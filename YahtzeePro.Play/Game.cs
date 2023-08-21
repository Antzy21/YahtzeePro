using System;
using System.Security.Cryptography.X509Certificates;
using YahtzeePro;
using YahtzeePro.Play;

internal class Game
{
    private readonly int _winningValue;
    private readonly int _totalDice;
    private readonly IPlayer _player1;
    private readonly IPlayer _player2;

    private IPlayer _currentPlayer;

    private GameState _gameState;

    public Game(int winningValue, int totalDice, IPlayer player1, IPlayer player2)
    {
        _winningValue = winningValue;
        _totalDice = totalDice;
        _player1 = player1;
        _player2 = player2;

        _currentPlayer = _player1;

        _gameState = new(
            PlayerScore: 0,
            OpponentScore: 0,
            CachedScore: 0,
            DiceToRoll: totalDice,
            IsStartOfTurn: true);
    }

    public void Play()
    {
        while (_gameState.PlayerScore < _winningValue) {
            
            var move = _currentPlayer.GetMove(_gameState);

            switch (move)
            {
                case PlayChoice.Safe:
                    _gameState = MakeSafeMove(_gameState);
                    break;
                case PlayChoice.Risky:
                    _gameState = MakeRiskyMove(_gameState);
                    break;
            }

            SwitchPlayer();
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
        if (gs.IsStartOfTurn)
        {
            // Roll
        }
        else
        {
            // Roll
        }
        throw new NotImplementedException();
    }

    private GameState MakeSafeMove(GameState gs)
    {
        if (gs.IsStartOfTurn)
        {
            // Roll
        }
        else
        {
            // Roll
        }
        throw new NotImplementedException();
    }
}
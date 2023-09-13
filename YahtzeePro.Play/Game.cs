using System;
using System.Collections.Generic;
using YahtzeePro;
using YahtzeePro.Play;

internal class Game
{
    private readonly int _winningValue;
    private readonly int _totalDice;
    private readonly IPlayer _player1;
    private readonly IPlayer _player2;

    private readonly Dictionary<int, RollPossibilities> _rollPosibilitiesDictionary = new();

    private readonly Random _random = new();

    private IPlayer _currentPlayer;

    public bool firstPlayerWon = false;

    private GameState _gameState;

    public Game(int winningValue, int totalDice, IPlayer player1, IPlayer player2)
    {
        _winningValue = winningValue;
        _player1 = player1;
        _player2 = player2;
        _totalDice = totalDice;

        for (int i = 1; i <= _totalDice; i++)
        {
            _rollPosibilitiesDictionary.Add(i, new RollPossibilities(i));
        }

        _currentPlayer = _player1;

        _gameState = new(
            PlayerScore: 0,
            OpponentScore: 0,
            CachedScore: 0,
            DiceToRoll: totalDice,
            IsStartOfTurn: true,
            TotalDice: _totalDice);
    }

    public void Play()
    {
        while (_gameState.PlayerScore < _winningValue)
        {

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
        }

        if (_currentPlayer == _player1)
            firstPlayerWon = true;
        else
            firstPlayerWon = false;
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
            var rolledDice = DiceCombination.Generate(_totalDice, _random);
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
}
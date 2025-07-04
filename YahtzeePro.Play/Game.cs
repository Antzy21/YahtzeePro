﻿using System.Diagnostics.CodeAnalysis;
using YahtzeePro.Core;
using YahtzeePro.Core.Models;
using YahtzeePro.Play.Players;

namespace YahtzeePro.Play;

public class Game(GameConfiguration gameConfiguration, IPlayer player1, IPlayer player2)
{
    public GameState GameState = new(
            PlayerScore: 0,
            OpponentScore: 0,
            CachedScore: 0,
            DiceToRoll: gameConfiguration.TotalDice,
            IsStartOfTurn: true,
            GameConfiguration: gameConfiguration);
    public DiceCombination? LastDiceRoll = null;

    private readonly IPlayer _player1 = player1;
    private readonly IPlayer _player2 = player2;
    private readonly Random _random = new();
    private int _currentPlayerId = 0;

    public IPlayer GetCurrentPlayer() => _currentPlayerId == 0 ? _player1 : _player2;

    public void MakeMove(MoveChoice move)
    {
        if (move == MoveChoice.Safe)
        {
            if (GameState.IsStartOfTurn)
            {
                // Roll at start of turn
                var rolledDice = DiceCombinationGenerator.Generate(GameState.GameConfiguration.TotalDice, _random);
                LastDiceRoll = rolledDice;
                GameState = ResolveRolledDice(rolledDice, GameState);
            }
            else
            {
                SwitchPlayer();
                LastDiceRoll = null;
                GameState = GameState.Bank();
            }
        }
        else if (move == MoveChoice.Risky)
        {
            var rolledDice = DiceCombinationGenerator.Generate(GameState.DiceToRoll, _random);
            LastDiceRoll = rolledDice;
            GameState = ResolveRolledDice(rolledDice, GameState);
        }
    }

    private void SwitchPlayer()
    {
        _currentPlayerId++;
        if (_currentPlayerId >= 2)
        {
            _currentPlayerId = 0;
        }
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

    public IPlayer GetOpponent() => _currentPlayerId == 0 ? _player2 : _player1;
}
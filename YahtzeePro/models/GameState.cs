﻿namespace YahtzeePro.models;

[Serializable]
public readonly record struct GameState(
    int PlayerScore, 
    int OpponentScore, 
    int CachedScore, 
    int DiceToRoll, 
    bool IsStartOfTurn, 
    GameConfiguration GameConfiguration
);

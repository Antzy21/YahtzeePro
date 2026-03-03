namespace YahtzeePro.Core.Models;

[Serializable]
public record TurnMove(
    int Turn,
    MoveChoice MoveChoice,
    DiceCombination? RolledDice
);

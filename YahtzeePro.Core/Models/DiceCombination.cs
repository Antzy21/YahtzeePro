namespace YahtzeePro.Core.Models;

[Serializable]
public record DiceCombination(
    Dictionary<int, int> DiceCount,
    int Score,
    int NumberOfScoringDice,
    bool AllDiceAreScoring
);
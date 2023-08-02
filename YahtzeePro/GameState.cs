namespace YahtzeePro
{
    public record GameState(
        int PlayerScore,
        int OpponentScore,
        int CachedScore,
        int DiceToRoll
    );
}

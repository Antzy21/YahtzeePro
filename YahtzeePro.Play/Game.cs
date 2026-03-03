using YahtzeePro.Core.Models;
using YahtzeePro.Play.Players;

namespace YahtzeePro.Play;

[Serializable]
public class Game(GameConfiguration gameConfiguration, IPlayer Player1, IPlayer Player2)
{
    public GameState GameState { get; set; } = new(
        PlayerScore: 0,
        OpponentScore: 0,
        CachedScore: 0,
        DiceToRoll: gameConfiguration.TotalDice,
        IsStartOfTurn: true,
        GameConfiguration: gameConfiguration);

    public IPlayer Player1 { get; set; } = Player1;
    public IPlayer Player2 { get; set; } = Player2;
    public List<TurnMove> TurnMoves { get; set; } = [];
    public int Turns { get; set; } = 0;
}
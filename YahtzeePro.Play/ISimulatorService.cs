using YahtzeePro.Core.Models;
using YahtzeePro.Play.Players;

namespace YahtzeePro.Play;

public interface ISimulatorService
{
    public GameResult SimulateGame(
        IPlayer player1,
        IPlayer player2,
        GameConfiguration gameConfiguration);

    public GameSetResult SimulateGames(
        IPlayer player1,
        IPlayer player2,
        int totalGames,
        GameConfiguration gameConfiguration);

    public IEnumerable<GameSetResult> SimulateSetsOfGames(
        IPlayer player1,
        IPlayer player2,
        int totalGames,
        int totalSets,
        GameConfiguration gameConfiguration);
}

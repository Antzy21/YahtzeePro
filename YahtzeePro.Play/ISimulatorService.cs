using YahtzeePro.Core.Models;
using YahtzeePro.Play.Players.AutoPlayers;

namespace YahtzeePro.Play;

public interface ISimulatorService
{
    public GameResult SimulateGame(
        IAutoPlayer player1,
        IAutoPlayer player2,
        GameConfiguration gameConfiguration);

    public GameSetResult SimulateGames(
        IAutoPlayer player1,
        IAutoPlayer player2,
        int totalGames,
        GameConfiguration gameConfiguration);

    public IEnumerable<GameSetResult> SimulateSetsOfGames(
        IAutoPlayer player1,
        IAutoPlayer player2,
        int totalGames,
        int totalSets,
        GameConfiguration gameConfiguration);
}

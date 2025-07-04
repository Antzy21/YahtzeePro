using YahtzeePro.Core.Models;

namespace YahtzeePro.Cli.Services;

public interface ICommandService {
    public void Status();

    public void CalculateOptimum(int winningValue, int totalDice);
    public void GetOptimum(int winningValue, int totalDice);
    public void ListOptimums();
    
    public void ListGames();
    public void Move(MoveChoice moveChoice, Guid gameId);
    public void NewGame(string opponent, int winningValue, int totalDice);

    public void Simulate(string strategy1, string strategy2, int numberOfGames, int numberOfSets, int winningValue, int totalDice);
}
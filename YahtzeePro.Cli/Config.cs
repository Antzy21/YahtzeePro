namespace YahtzeePro.Cli;

public class Config
{
    public Guid? GAMEID { set; get; }
    public int? GAMECONFIG_WINNINGVALUE { get; set; }
    public int? GAMECONFIG_TOTALDICE { get; set; }
    public bool AUTO_UPDATE_CONFIG { get; set; } = true;
}

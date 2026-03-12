using Dalamud.Game.Text;
using DalamudBasics.Configuration;
using MinigameCollection.Dice;
using System;

namespace MinigameCollection;

[Serializable]
public class Configuration : IConfiguration
{
    public int Version { get; set; } = 0;
    public XivChatType DefaultOutputChatType { get; set; } = XivChatType.Party;
    public bool LogOutgoingChatOutput { get; set; } = true;
    public bool LogClientOnlyChatOutput { get; set; } = true;
    public int LimitedChatChannelsMessageDelayInMs { get; set; } = 1500;

    public int SelectedGame { get; set; } = 0;

    public AcceptedRollType AcceptedRollType { get; set; } = AcceptedRollType.Any;

    public bool AutoBuyIn { get; set; } = true;
    public bool AutoCashOut { get; set; } = true;

    public bool GarleanRouletteRestartIfGunEmpties { get; set; } = true;
}

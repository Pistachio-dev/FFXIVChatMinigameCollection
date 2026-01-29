using Dalamud.Game.Text;
using DalamudBasics.Chat.Interpretation;
using DalamudBasics.Configuration;
using MinigameCollection.Dice;
using Model.Base;
using System;

namespace MinigameCollection;

[Serializable]
public class Configuration : IConfiguration
{
    public int Version { get; set; } = 0;
    public XivChatType DefaultOutputChatType { get; set; } = XivChatType.Echo;
    public bool LogOutgoingChatOutput { get; set; } = true;
    public bool LogClientOnlyChatOutput { get; set; } = true;
    public int LimitedChatChannelsMessageDelayInMs { get; set; } = 1000;

    public bool UsingRealGil { get; set; } = true;

    public int SelectedGame { get; set; } = 0;

    public AcceptedRollType AcceptedRollType { get; set; } = AcceptedRollType.Any;
}

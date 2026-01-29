using Dalamud.Plugin.Services;
using DalamudBasics.Chat.Output;
using DalamudBasics.Configuration;
using DalamudBasics.DiceRolling;
using DalamudBasics.Extensions;
using MinigameCollection.Dice;
using MinigameCollection.Games;
using MinigameCollection.Games.GarleanRouletteGame;
using MinigameCollection.Games.MicroGameGame;
using MinigameCollection.Games.NoGameGame;
using Model.Base;
using System;
using System.Linq;

namespace MinigameCollection
{
    public class GameHost : IDisposable
    {
        public (GameId id, Func<IGame> builder)[] AvailableGames()
        {
            return [
                (NoGame.Id, () => new NoGame()),
                (Microgame.Id, () => new Microgame()),
                (GarleanRoulette.Id, () => new GarleanRoulette(rollTracker, config))
            ];
        }

        public DiceRollManager DiceManager;
        public readonly IChatOutput ChatOutput;
        public readonly IChatGui ChatGui;
        public readonly IObjectTable ObjectTable;
        private readonly RollTracker rollTracker;
        private readonly Configuration config;
        private IGame? activeGame;

        private PlayerSet players;
        public readonly IFramework Framework;

        public PlayerSet Players => players;

        public GameHost(PlayerSet players, IFramework framework, DiceRollManager diceManager, IChatOutput chatOutput,
            IChatGui chatGui, IObjectTable objectTable, RollTracker rollTracker, IConfigurationService<Configuration> config)
        {
            this.players = players;
            this.Framework = framework;
            this.DiceManager = diceManager;
            this.ChatOutput = chatOutput;
            this.ChatGui = chatGui;
            this.ObjectTable = objectTable;
            this.rollTracker = rollTracker;
            this.config = config.GetConfiguration();
        }



        public bool HasGame()
        {
            return activeGame != null;
        }

        public void StartGame(GameId gameId)
        {
            (GameId id, Func<IGame> constructor) = AvailableGames().FirstOrDefault(p => p.id.Equals(gameId));
            activeGame = constructor();
            activeGame.SafeInitialize(this);
        }

        public void Update()
        {
            activeGame?.SafeUpdate();
        }

        public void DrawUI()
        {
            activeGame?.SafeDrawUI();
        }

        public string GetHostPlayerFullName()
        {
            return ObjectTable.LocalPlayer?.GetFullName() ?? "Player name not found";
        }

        public void Dispose()
        {
            UnloadGameIfNecessary();
        }

        public void UnloadGameIfNecessary()
        {
            if (HasGame())
            {
                activeGame!.Dispose();
            }
        }
    }
}

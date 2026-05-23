using Dalamud.Game;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;
using System;
using System.Linq;

// Lifted from PatMe
namespace MinigameCollection.Emotes
{
    public class EmoteReaderHooks : IDisposable
    {
        public Action<IPlayerCharacter, ushort>? OnEmote;

        public delegate void OnEmoteFuncDelegate(ulong unk, ulong instigatorAddr, ushort emoteId, ulong targetId, ulong unk2);
        private readonly Hook<OnEmoteFuncDelegate>? hookEmote;
        private readonly IGameInteropProvider sigScanner;
        private readonly IObjectTable objectTable;
        public bool IsValid = false;

        public EmoteReaderHooks(IGameInteropProvider sigScanner, IObjectTable objectTable)
        {

            this.sigScanner = sigScanner;
            this.objectTable = objectTable;
            try
            {
                hookEmote = sigScanner.HookFromSignature<OnEmoteFuncDelegate>("E8 ?? ?? ?? ?? 48 8D 8B ?? ?? ?? ?? 4C 89 74 24", OnEmoteDetour);
                hookEmote.Enable();

                IsValid = true;
            }
            catch (Exception ex)
            {
                Plugin.Log.Error(ex, "failed to hook emotes!");
            }

        }

        public void Dispose()
        {
            hookEmote?.Dispose();
            IsValid = false;
        }

        void OnEmoteDetour(ulong unk, ulong instigatorAddr, ushort emoteId, ulong targetId, ulong unk2)
        {
            // unk - some field of event framework singleton? doesn't matter here anyway
            // Service.logger.Info($"Emote >> unk:{unk:X}, instigatorAddr:{instigatorAddr:X}, emoteId:{emoteId}, targetId:{targetId:X}, unk2:{unk2:X}");

            Plugin.Log.Info($"Emote triggered >> instigatorAddr:{instigatorAddr:X}, emoteId:{emoteId}, targetId:{targetId:X}");
            if (objectTable.LocalPlayer != null)
            {
                if (targetId == objectTable.LocalPlayer.GameObjectId)
                {
                    var instigatorOb = objectTable.FirstOrDefault(x => (ulong)x.Address == instigatorAddr) as IPlayerCharacter;
                    if (instigatorOb != null)
                    {
                        OnEmote?.Invoke(instigatorOb, emoteId);                        
                    }
                }
            }

            hookEmote?.Original(unk, instigatorAddr, emoteId, targetId, unk2);
        }
    }
}

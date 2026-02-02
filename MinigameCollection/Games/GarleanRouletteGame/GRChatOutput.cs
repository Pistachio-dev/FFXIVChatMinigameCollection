using DalamudBasics.Chat.Output;
using DalamudBasics.Extensions;
using Model.Base;
using System;
using System.Collections.Generic;

namespace MinigameCollection.Games.GarleanRouletteGame
{
    public class GRChatOutput
    {
        private readonly IChatOutput chatOutput;
        private readonly string[] ShootQuips = [
                "D O D G E  T H I S",
                "B I T E  T H E  B U L L E T",
                "N O T H I N G  P E R S O N A L",
                "Y O U  R O L L E D  P O O R L Y",
                "E A T  L E A D",
                "T O U G H  L U C K",
                "Y O U  L O S E",
            ];

        private readonly string[] PlayerDeadQuips = [
            "<T> 's mind gets opened a bit too strongly",
            "<T> dies unceremoniously",
            "<T> has their brains redistributed",
            "<T> unsubscribes from life",
            "<T> is in a better place now",
            "<T> receives a dose of lead applied via gun",
            "<T> gets zeroed",
            "<T> is gone, but not forgotten",
            "<T> regrets their decision"

        ];

        
        public GRChatOutput(IChatOutput chatOutput)
        {
            this.chatOutput = chatOutput;
        }

        public void RollDiceAsHouse(string expectedRollerName, int outOf, bool isAlliance)
        {
            chatOutput.WriteChat($"{expectedRollerName.GetFirstName()} is taking too long. The house takes the shot...");
            chatOutput.WriteDiceCommand(outOf, isAlliance);
        }

        public void WriteWinner(MGPlayer player)
        {
            chatOutput.WriteChat("<se.15>", minSpacingBeforeInMs: 2000);
            chatOutput.WriteChat($"{player.FullName} wins.");
        }

        public void WritePlayerOrder(List<string> names)
        {
            chatOutput.WriteChat($"Order: {names.GetWordsSeparatedByArrows()}");
        }

        public void WritePlayerSurvives(MGPlayer? player)
        {
            chatOutput.WriteChat("...", minSpacingBeforeInMs: 1000);
            chatOutput.WriteChat("The gun clicks<se.12>", minSpacingBeforeInMs: 1000);
        }
        public void WritePlayerShot(MGPlayer? player)
        {
            chatOutput.WriteChat("...", minSpacingBeforeInMs: 1000);
            chatOutput.WriteChat("...", minSpacingBeforeInMs: 1000);

            chatOutput.WriteChat(@"　 ∧､<se.4>", minSpacingBeforeInMs: 1000);
            chatOutput.WriteChat(@"／⌒ヽ＼　　   ∧＿∧");
            chatOutput.WriteChat(@"|( ● )|　i＼（　´_ゝ`）");
            chatOutput.WriteChat(@"＼＿ノ　^i |ハ 　 　 ＼");
            chatOutput.WriteChat(@"　|＿|,-''iつl/　　　ｖ");
            chatOutput.WriteChat(@"　　[__|_|／〉 　　　 ｜");
            chatOutput.WriteChat(@"　　　[ニニ〉");
            chatOutput.WriteChat(@"　　　└―'");
            var randomQuip = ShootQuips[new Random().Next(0, ShootQuips.Length)];
            chatOutput.WriteChat(randomQuip);
            var randomDeathQuip = PlayerDeadQuips[new Random().Next(0, PlayerDeadQuips.Length)];
            chatOutput.WriteChat(randomDeathQuip.Replace("<T>", player?.FullName.GetNameOnly() ?? "nobody"));
        }
    }
}

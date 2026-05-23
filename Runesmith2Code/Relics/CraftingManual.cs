#region

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Runesmith2.Runesmith2Code.Utils;

#endregion

namespace Runesmith2.Runesmith2Code.Relics;

public class CraftingManual : Runesmith2Relic
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;

    public override async Task AfterPlayerTurnStartEarly(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner || player.Creature.CombatState!.RoundNumber > 1) return;

        Flash();
        var prefs = new CardSelectorPrefs(L10NLookup("RUNESMITH2-CRAFTING_MANUAL.selectionScreenPrompt"), 0, 1)
        {
            Cancelable = true
        };

        var card = (await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(Owner), Owner, prefs,
            c => c.Tags.Contains(RunesmithEnum.Recipe))).FirstOrDefault();
        if (card == null)
            return;
        _ = await CardPileCmd.Add(card, PileType.Hand);
        card.SetToFreeThisTurn();
    }
}
#region

using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Runesmith2.Runesmith2Code.Extensions;
using Runesmith2.Runesmith2Code.HoverTips;

#endregion

namespace Runesmith2.Runesmith2Code.Cards.Uncommon;

public class Electromagnet : Runesmith2Card
{
    public Electromagnet() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithBlock(2, 4);
        WithTip(RunesmithHoverTip.Improved);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        var prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
        var cardModel = (await CardSelectCmd.FromCombatPile(choiceContext,
            PileType.Discard.GetPile(Owner), Owner, prefs, c => c.IsImproved())).FirstOrDefault();
        if (cardModel != null) await CardPileCmd.Add(cardModel, PileType.Hand);
    }
}
#region

using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Runesmith2.Runesmith2Code.HoverTips;
using Runesmith2.Runesmith2Code.Powers;

#endregion

namespace Runesmith2.Runesmith2Code.Cards.Uncommon;

public class DuctTape : Runesmith2Card
{
    public DuctTape() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithTip(RunesmithHoverTip.Stasis);
        WithTip(RunesmithHoverTip.Enhance);
        WithKeyword(CardKeyword.Innate, UpgradeType.Add);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.ApplySelf<DuctTapePower>(choiceContext, this, 1);
    }
}
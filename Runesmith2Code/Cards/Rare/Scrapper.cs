#region

using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Runesmith2.Runesmith2Code.Cards.Token;
using Runesmith2.Runesmith2Code.Commands;
using Runesmith2.Runesmith2Code.HoverTips;
using Runesmith2.Runesmith2Code.Powers;

#endregion

namespace Runesmith2.Runesmith2Code.Cards.Rare;

public class Scrapper : Runesmith2Card
{
    public Scrapper() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithVar("Amount", 1);
        WithCostUpgradeBy(-1);
        WithTip(RunesmithHoverTip.Break);
        WithTip(RunesmithHoverTip.Charge);
        WithTip(typeof(Scrap));
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await CommonActions.ApplySelf<ScrapperPower>(choiceContext, this, DynamicVars["Amount"].IntValue);
        await RunesmithCardCmd.GiveCard<Scrap>(Owner, PileType.Hand, skipAnimation: true);
    }
}
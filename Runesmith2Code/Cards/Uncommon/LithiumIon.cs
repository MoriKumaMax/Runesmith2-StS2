#region

using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Runesmith2.Runesmith2Code.Commands;
using Runesmith2.Runesmith2Code.DynamicVars;
using Runesmith2.Runesmith2Code.HoverTips;
using Runesmith2.Runesmith2Code.Powers;
using Runesmith2.Runesmith2Code.Structs;

#endregion

namespace Runesmith2.Runesmith2Code.Cards.Uncommon;

public class LithiumIon : Runesmith2Card
{
    public LithiumIon() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithVar("Amount", 3, 1);
        WithVar(new AquaVar(1));
        WithTip(RunesmithHoverTip.Elements);
        WithTip(typeof(AmpPower));
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await RunesmithPlayerCmd.GainElements(new Elements(this), Owner, play);
        await CommonActions.ApplySelf<LithiumIonPower>(choiceContext, this, DynamicVars["Amount"].IntValue);
    }
}
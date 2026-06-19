#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Runesmith2.Runesmith2Code.Commands;
using Runesmith2.Runesmith2Code.Extensions;
using Runesmith2.Runesmith2Code.HoverTips;

#endregion

namespace Runesmith2.Runesmith2Code.Cards.Rare;

public class GammaRayBurst : Runesmith2Card
{
    public GammaRayBurst() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithVar("Amount", 0, 1);
        WithTip(RunesmithHoverTip.Charge);
    }

    protected override bool HasEnergyCostX => true;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var chargeAmount = ResolveEnergyXValue() + DynamicVars["Amount"].IntValue;
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        RuneCmd.ChargeAll(choiceContext, Owner, chargeAmount);
        
        var runeQueue = Owner.PlayerCombatState?.GetRuneQueue();
        if (runeQueue != null && runeQueue.HasAny())
            foreach (var rune in runeQueue.Runes)
            {
                await Cmd.CustomScaledWait(0.1f, 0.2f);
                await RuneCmd.Passive(choiceContext, rune);
            }
    }
}
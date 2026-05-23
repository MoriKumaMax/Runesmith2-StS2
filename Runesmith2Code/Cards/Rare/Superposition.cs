#region

using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Runesmith2.Runesmith2Code.Extensions;

#endregion

namespace Runesmith2.Runesmith2Code.Cards.Rare;

public class Superposition : Runesmith2Card
{
    public Superposition() : base(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithDamage(15, 6);
        WithBlock(11, 5);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);

        var runeQueue = Owner.PlayerCombatState?.RuneQueue();
        var isOdd = false;
        if (runeQueue != null)
        {
            var count = runeQueue.Runes.Count;
            if (count % 2 != 0) isOdd = true;
        }

        if (isOdd)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this)
                .WithHitCount(2)
                .Targeting(play.Target)
                .SpawningHitVfxOnEachCreature()
                .WithHitFx("vfx/vfx_attack_blunt")
                .Execute(choiceContext);
        }
        else
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this)
                .Targeting(play.Target)
                .SpawningHitVfxOnEachCreature()
                .WithHitFx("vfx/vfx_attack_blunt")
                .Execute(choiceContext);

            await CommonActions.CardBlock(this, play);
        }
    }
}
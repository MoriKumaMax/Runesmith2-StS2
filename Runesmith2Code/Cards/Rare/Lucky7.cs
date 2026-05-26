#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Runesmith2.Runesmith2Code.Commands;
using Runesmith2.Runesmith2Code.HoverTips;
using Runesmith2.Runesmith2Code.Structs;

using Runesmith2.Runesmith2Code.Utils;

#endregion

namespace Runesmith2.Runesmith2Code.Cards.Rare;

public class Lucky7 : Runesmith2Card
{
    public Lucky7() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
    {
        WithDamage(35, 14);
        WithTags(RunesmithEnum.Recipe);
        WithTip(RunesmithHoverTip.Recipe);
    }

    protected override bool ShouldGlowGoldInternal => HasElements();

    public override TargetType TargetType => HasElements() ? TargetType.AllEnemies : TargetType.Self;

    public override Elements CanonicalElementsCost => new(4, 4, 4);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (IsPlayedWithoutElements)
        {
            await RunesmithPlayerCmd.GainElements(GetElementsCostWithModifiers().ClampZero(), Owner, cardPlay);
            return;
        }
        
        await RecipeOnPlayWrapper(choiceContext, cardPlay);
    }

    private async Task RecipeOnPlayWrapper(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (CombatState == null) return;
        
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this)
            .TargetingAllOpponents(CombatState)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }
}
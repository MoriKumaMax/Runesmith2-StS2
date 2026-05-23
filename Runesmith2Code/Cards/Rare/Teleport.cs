#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Runesmith2.Runesmith2Code.Commands;
using Runesmith2.Runesmith2Code.Extensions;
using Runesmith2.Runesmith2Code.HoverTips;
using Runesmith2.Runesmith2Code.Models;

#endregion

namespace Runesmith2.Runesmith2Code.Cards.Rare;

public class Teleport : Runesmith2Card
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    public override RuneBreakType RuneBreakType => RuneBreakType.Newest;

    protected override bool ShouldGlowGoldInternal => HasRune();

    public Teleport() : base(0, CardType.Skill, CardRarity.Rare, TargetType.AnyAlly)
    {
        WithTip(RunesmithHoverTip.Break);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target?.Player);
        if (HasRune())
        {
            var runes = Owner.PlayerCombatState?.RuneQueue()?.Runes;
            var runeOrig = runes?[^1];
            if (runeOrig == null) return;
            var runeCopy = (RuneModel)runeOrig.MutableClone();

            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

            await RuneCmd.Break(choiceContext, Owner, runeOrig);

            await RuneCmd.AddRune(choiceContext, runeCopy, play.Target.Player, play);
        }
    }
}
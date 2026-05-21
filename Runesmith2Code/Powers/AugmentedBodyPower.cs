#region

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.HoverTips;
using Runesmith2.Runesmith2Code.Extensions;
using Runesmith2.Runesmith2Code.HoverTips;

#endregion

namespace Runesmith2.Runesmith2Code.Powers;

public class AugmentedBodyPower : Runesmith2Power
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        RunesmithHoverTipFactory.Static(RunesmithHoverTip.Improved)
    ];

    public override Task BeforeFlushLate(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player || player.Creature.CombatState == null ||
            !Hook.ShouldFlush(player.Creature.CombatState, player)) return Task.CompletedTask;

        var cards = PileType.Hand.GetPile(player).Cards
            .Where(c => c.IsImproved()).ToList();

        if (cards.Count <= 0) return Task.CompletedTask;

        foreach (var card in cards) card.GiveSingleTurnRetain();

        Flash();

        return Task.CompletedTask;
    }
}
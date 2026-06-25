#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using Runesmith2.Runesmith2Code.Cards;
using Runesmith2.Runesmith2Code.Cards.Basic;

#endregion

namespace Runesmith2.Runesmith2Code.Models.Runes;

// Deal damage
public class FlammaRune : RuneModel
{
    public override decimal PassiveVal { get; set; } = 4;
    public override int ChargeVal { get; set; } = 2;

    public override bool IsUsingPotency => true;

    public override ChargeDepletionType ChargeDepletion => ChargeDepletionType.EndTurn;

    public override Runesmith2RecipeCard RecipeCard => ModelDb.Get<Flamma>();

    public override async Task<bool> BeforeTurnEndRuneTrigger(PlayerChoiceContext choiceContext)
    {
        await Passive(choiceContext);
        return true;
    }

    public override async Task Passive(PlayerChoiceContext choiceContext)
    {
        Trigger();
        PlayPassiveSfx();
        await ApplyFireDamage(choiceContext, PassiveVal);
        UseCharge();
    }

    public override async Task Break(PlayerChoiceContext choiceContext)
    {
        PlayPassiveSfx();
        await ApplyFireDamage(choiceContext, BreakVal);
    }

    private async Task ApplyFireDamage(PlayerChoiceContext choiceContext, decimal amount)
    {
        var list = GetHittableCreatures();
        if (list.Count == 0) return;

        var target = Owner.RunState.Rng.CombatTargets.NextItem(list);
        if (target != null)
        {
            NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NGroundFireVfx.Create(target));
            await CreatureCmd.Damage(choiceContext, target, amount, ValueProp.Unpowered, Owner.Creature);
        }
    }
}
#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using Runesmith2.Runesmith2Code.Cards;
using Runesmith2.Runesmith2Code.Cards.Uncommon;

#endregion

namespace Runesmith2.Runesmith2Code.Models.Runes;

public class IncendiumRune : RuneModel
{
    public override decimal PassiveVal { get; set; } = 5;
    public override int ChargeVal { get; set; } = 3;

    public override bool IsUsingPotency => true;

    public override ChargeDepletionType ChargeDepletion => ChargeDepletionType.EndTurn;

    public override Runesmith2RecipeCard RecipeCard => ModelDb.Get<Incendium>();

    public override async Task<bool> BeforeTurnEndRuneTrigger(PlayerChoiceContext choiceContext)
    {
        await Passive(choiceContext);
        return true;
    }

    public override async Task Passive(PlayerChoiceContext choiceContext)
    {
        Trigger();
        PlayPassiveSfx();
        await ApplyAoeFireDamage(choiceContext, PassiveVal);
        UseCharge();
    }

    public override async Task Break(PlayerChoiceContext choiceContext)
    {
        await ApplyAoeFireDamage(choiceContext, BreakVal);
    }

    private async Task ApplyAoeFireDamage(PlayerChoiceContext choiceContext, decimal amount)
    {
        var targets = GetHittableCreatures();
        if (targets.Count == 0) return;

        foreach (var target in targets)
            NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NGroundFireVfx.Create(target));
        await CreatureCmd.Damage(choiceContext, targets, amount, ValueProp.Unpowered, Owner.Creature);
    }
}
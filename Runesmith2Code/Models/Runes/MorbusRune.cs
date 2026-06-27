#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Runesmith2.Runesmith2Code.Cards;
using Runesmith2.Runesmith2Code.Cards.Uncommon;

#endregion

namespace Runesmith2.Runesmith2Code.Models.Runes;

// Apply Weak and Vulnerable
public class MorbusRune : RuneModel
{
    public override decimal PassiveVal { get; set; } = 0;
    public override int ChargeVal { get; set; } = 2;

    public override (bool, bool) ShowBottomLabel => (false, true);

    public override (decimal, decimal) BottomValue => (1, 2);

    public override ChargeDepletionType ChargeDepletion => ChargeDepletionType.StartTurn;

    public override Runesmith2RecipeCard RecipeCard => ModelDb.Get<Morbus>();

    public override async Task<bool> SetupTurnStartRuneTrigger(PlayerChoiceContext choiceContext)
    {
        await Passive(choiceContext);
        return true;
    }

    public override async Task Passive(PlayerChoiceContext choiceContext)
    {
        Trigger();
        PlayPassiveSfx();
        await ApplyDebuff(choiceContext, 1);
        UseCharge();
    }

    public override async Task Break(PlayerChoiceContext choiceContext)
    {
        await ApplyDebuff(choiceContext, 2);
    }

    private async Task ApplyDebuff(PlayerChoiceContext choiceContext, decimal amount)
    {
        var targets = GetHittableCreatures();
        if (targets.Count == 0) return;

        await PowerCmd.Apply<WeakPower>(choiceContext, targets, amount, Owner.Creature, null);
        await PowerCmd.Apply<VulnerablePower>(choiceContext, targets, amount, Owner.Creature, null);
    }
}
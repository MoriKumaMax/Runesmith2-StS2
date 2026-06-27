#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Runesmith2.Runesmith2Code.Cards;
using Runesmith2.Runesmith2Code.Cards.Uncommon;
using Runesmith2.Runesmith2Code.Commands;
using Runesmith2.Runesmith2Code.Extensions;

#endregion

namespace Runesmith2.Runesmith2Code.Models.Runes;

// Give Charge
public class AlbusRune : RuneModel
{
    public override decimal PassiveVal { get; set; } = 0;
    public override int ChargeVal { get; set; } = 2;

    public override (bool, bool) ShowBottomLabel => (false, true);

    public override (decimal, decimal) BottomValue => (1, 2);

    public override ChargeDepletionType ChargeDepletion => ChargeDepletionType.EndTurn;

    public override Runesmith2RecipeCard RecipeCard => ModelDb.Get<Albus>();

    public override bool CanPassive => HasAnyValidRune() && base.CanPassive;

    public override async Task<bool> BeforeTurnEndEarlyRuneTrigger(PlayerChoiceContext choiceContext)
    {
        await Passive(choiceContext);
        return true;
    }

    public override async Task Passive(PlayerChoiceContext choiceContext)
    {
        Trigger();
        PlayPassiveSfx();
        await ChargeAndAddPotency(choiceContext, 1);
        UseCharge();
    }

    public override async Task Break(PlayerChoiceContext choiceContext)
    {
        await ChargeAndAddPotency(choiceContext, 2, true);
    }

    private async Task ChargeAndAddPotency(PlayerChoiceContext choiceContext, decimal amount, bool chargeAll = false)
    {
        var runeQueue = Owner.PlayerCombatState?.GetRuneQueue();
        if (runeQueue == null) return;
        
        RuneCmd.ChargeRunes(choiceContext,
            chargeAll ? runeQueue.Runes.Where(r => r != this) : runeQueue.Runes.Where(r => r is not AlbusRune),
            (int)amount);
        await RuneCmd.AddPotency(choiceContext, runeQueue.Runes, Owner, null, amount, ValueProp.Unpowered);
        await Cmd.CustomScaledWait(0.2f, 0.3f);
    }

    private bool HasAnyValidRune()
    {
        var runeQueue = Owner.PlayerCombatState?.GetRuneQueue();
        return runeQueue != null && runeQueue.Runes.Any(r => r is not AlbusRune);
    }
}
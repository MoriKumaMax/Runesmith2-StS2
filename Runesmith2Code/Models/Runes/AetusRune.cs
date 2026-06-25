#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Runesmith2.Runesmith2Code.Cards;
using Runesmith2.Runesmith2Code.Cards.Rare;

#endregion

namespace Runesmith2.Runesmith2Code.Models.Runes;

// Gain Energy
public class AetusRune : RuneModel
{
    public override decimal PassiveVal { get; set; } = 0;
    public override int ChargeVal { get; set; } = 4;

    public override (bool, bool) ShowBottomLabel => (false, true);

    public override (decimal, decimal) BottomValue => (1, 2);

    public override ChargeDepletionType ChargeDepletion => ChargeDepletionType.StartTurn;

    public override Runesmith2RecipeCard RecipeCard => ModelDb.Get<Aetus>();

    public override async Task<bool> SetupTurnStartRuneTrigger(PlayerChoiceContext choiceContext)
    {
        await Passive(choiceContext);
        return true;
    }

    public override async Task Passive(PlayerChoiceContext choiceContext)
    {
        Trigger();
        PlayPassiveSfx();
        await GainEnergy(choiceContext, 1);
        UseCharge();
    }

    public override async Task Break(PlayerChoiceContext choiceContext)
    {
        await GainEnergy(choiceContext, 2);
    }

    private async Task GainEnergy(PlayerChoiceContext choiceContext, decimal amount)
    {
        await PlayerCmd.GainEnergy(amount, Owner);
    }
}
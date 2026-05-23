#region

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Runesmith2.Runesmith2Code.Commands;
using Runesmith2.Runesmith2Code.DynamicVars;
using Runesmith2.Runesmith2Code.HoverTips;
using Runesmith2.Runesmith2Code.Structs;

#endregion

namespace Runesmith2.Runesmith2Code.Relics;

public class CrystallizedRainbow : Runesmith2Relic
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new ElementsVar(1)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        RunesmithHoverTipFactory.CreateElementsHoverTip()
    ];

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner || !CombatManager.Instance.IsInProgress)
            return;

        var amount = DynamicVars[ElementsVar.defaultName].IntValue;
        // TODO: Using orb rng, consider custom rng or use another type. 
        var elementGain = Owner.RunState.Rng.CombatOrbGeneration.NextItem([
            Elements.WithIgnis(amount), Elements.WithTerra(amount), Elements.WithAqua(amount)
        ]);

        Flash();
        await RunesmithPlayerCmd.GainElements(elementGain, Owner);
    }
}
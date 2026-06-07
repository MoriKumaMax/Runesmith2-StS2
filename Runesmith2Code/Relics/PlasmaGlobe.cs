#region

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Runesmith2.Runesmith2Code.Hooks;
using Runesmith2.Runesmith2Code.HoverTips;

#endregion

namespace Runesmith2.Runesmith2Code.Relics;

public class PlasmaGlobe : Runesmith2Relic, IModifyPotencyAdditive, IModifyCharge, IAfterModifyingCharge
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    private const string PotencyAmountKey = "PotencyAmount";

    private const string ChargeAmountKey = "ChargeAmount";

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new(PotencyAmountKey, 1),
        new(ChargeAmountKey, 1)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        RunesmithHoverTipFactory.Static(RunesmithHoverTip.Potency),
        RunesmithHoverTipFactory.Static(RunesmithHoverTip.Charge)
    ];

    public decimal ModifyPotencyAdditive(Player player, decimal block, ValueProp props, CardModel? cardSource,
        CardPlay? cardPlay)
    {
        if (cardSource != null)
        {
            if (cardSource.Owner != Owner) return 0m;
        }
        else if (Owner != player)
        {
            return 0m;
        }

        if (!props.IsPoweredCardOrMonsterMoveBlock()) return 0m;

        return DynamicVars[PotencyAmountKey].IntValue;
    }

    public decimal ModifyCharge(Player player, decimal charge, ValueProp props, CardModel? cardSource)
    {
        if (player == Owner && charge > 0) return charge + DynamicVars[ChargeAmountKey].IntValue;

        return charge;
    }

    public Task AfterModifyingCharge()
    {
        Flash();
        return Task.CompletedTask;
    }
}
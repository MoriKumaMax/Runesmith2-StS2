#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using Runesmith2.Runesmith2Code.Enchantments;

#endregion

namespace Runesmith2.Runesmith2Code.Relics;

public class Nanobots : Runesmith2Relic
{
    private const string ForgedAmountKey = "ForgedAmount";

    public override RelicRarity Rarity => RelicRarity.Uncommon;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new(ForgedAmountKey, 1)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        HoverTipFactory.FromEnchantment<Forged>(DynamicVars[ForgedAmountKey].IntValue);

    public override bool TryModifyCardRewardOptionsLate(
        Player player,
        List<CardCreationResult> cardRewards,
        CardCreationOptions options)
    {
        if (player != Owner)
            return false;
        var canonicalForged = ModelDb.Enchantment<Forged>();
        var list = cardRewards.Where(r => canonicalForged.CanEnchant(r.Card)).ToList();
        if (list.Count == 0)
            return false;
        var cardCreationResult = Owner.RunState.Rng.Niche.NextItem(list);
        if (cardCreationResult == null)
            return false;
        var card = Owner.RunState.CloneCard(cardCreationResult.Card);
        CardCmd.Enchant<Forged>(card, DynamicVars[ForgedAmountKey].BaseValue);
        cardCreationResult.ModifyCard(card, this);
        return true;
    }
}
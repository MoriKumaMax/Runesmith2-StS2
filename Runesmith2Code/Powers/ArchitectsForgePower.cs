#region

using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using Runesmith2.Runesmith2Code.Enchantments;
using Runesmith2.Runesmith2Code.Rewards;

#endregion

namespace Runesmith2.Runesmith2Code.Powers;

public class ArchitectsForgePower : Runesmith2Power
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override Task AfterCombatEnd(CombatRoom room)
    {
        ArgumentNullException.ThrowIfNull(Owner.Player);
        room.AddExtraReward(Owner.Player,
            new CardEnchantReward(ModelDb.GetId<Forged>(), Amount, CardEnchantReward.EnchantRewardFilter.CanEnhance,
                Owner.Player));
        return Task.CompletedTask;
    }
}
#region

using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Saves.Runs;
using Runesmith2.Runesmith2Code.Extensions;

#endregion

namespace Runesmith2.Runesmith2Code.Rewards;

public class CardEnchantReward(
    ModelId enchantId,
    int enchantAmount,
    CardEnchantReward.EnchantRewardFilter filter,
    Player player) : CustomReward(player)
{
    [CustomEnum] public static RewardType EnchantCardType;

    [Flags]
    public enum EnchantRewardFilter
    {
        Attack = 0x1,
        Skill = 0x2,
        Power = 0x4,
        CanEnhance = 0x8
    }

    public ModelId EnchantId { get; private set; } = ModelId.none;

    public int EnchantAmount { get; private set; } = -1;

    private static string RewardIcon => "res://Runesmith2/images/rewards/reward_icon_card_enchant.png";
    protected override string IconPath => RewardIcon;

    protected override RewardType RewardType => EnchantCardType;

    public override CreateRewardFromSave<CustomReward> DeserializeMethod => CreateFromSerializable;

    public override bool IsPopulated => EnchantAmount >= 0 && EnchantId != ModelId.none;

    public override LocString Description
    {
        get
        {
            var desc = new LocString("gameplay_ui", "RUNESMITH2-COMBAT_REWARD_ENCHANT");
            desc.Add("Enchant", ModelDb.GetById<EnchantmentModel>(EnchantId).Title.GetFormattedText());
            desc.Add("HasAmount", EnchantAmount > 0);
            desc.Add("Amount", EnchantAmount);
            return desc;
        }
    }

    public override void Populate()
    {
        EnchantId = enchantId;
        EnchantAmount = enchantAmount;
    }

    protected override async Task<bool> OnSelect()
    {
        var enchantment = ModelDb.GetById<EnchantmentModel>(EnchantId);
        // check if player has cards eligible for enchant
        if (!Player.Deck.Cards.Any(c => Filter(c) && enchantment.CanEnchant(c)))
            // make the reward selection does nothing
            return false;
        MainFile.Logger.Info($"Player {Player.NetId} obtained card enchantment from reward");

        var prefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1)
        {
            Cancelable = true,
            RequireManualConfirmation = true
        };

        var card = (await CardSelectCmd.FromDeckForEnchantment(Player, enchantment, EnchantAmount, Filter, prefs))
            .FirstOrDefault();
        if (card == null)
            return false;

        await ApplyEnchantment(card, enchantment, EnchantAmount);

        return true;
    }

    private bool Filter(CardModel? card)
    {
        if (card == null) return false;
        if (filter.HasFlag(EnchantRewardFilter.CanEnhance) && card.CanEnhance()) return true;
        if (filter.HasFlag(EnchantRewardFilter.Attack) && card.Type == CardType.Attack) return true;
        if (filter.HasFlag(EnchantRewardFilter.Skill) && card.Type == CardType.Skill) return true;
        if (filter.HasFlag(EnchantRewardFilter.Power) && card.Type == CardType.Power) return true;
        return false;
    }

    private static Task ApplyEnchantment(CardModel card, EnchantmentModel enchantment, int amount)
    {
        CardCmd.Enchant((EnchantmentModel)enchantment.MutableClone(), card, amount);
        var child = NCardEnchantVfx.Create(card);
        if (child == null) return Task.CompletedTask;
        var instance = NRun.Instance;
        instance?.GlobalUi.CardPreviewContainer.AddChildSafely(child);
        return Task.CompletedTask;
    }

    public override void MarkContentAsSeen()
    {
    }

    public static CardEnchantReward CreateFromSerializable(SerializableReward save, Player player)
    {
        return new CardEnchantReward(save.PredeterminedModelId, save.GoldAmount, (EnchantRewardFilter)save.OptionCount,
            player);
    }

    public override SerializableReward ToSerializable()
    {
        return new SerializableReward
        {
            RewardType = EnchantCardType,
            GoldAmount = enchantAmount,
            PredeterminedModelId = enchantId,
            OptionCount = (int)filter
        };
    }
}
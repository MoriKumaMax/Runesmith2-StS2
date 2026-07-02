#region

using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Runesmith2.Runesmith2Code.Cards;
using Runesmith2.Runesmith2Code.DynamicVars;
using Runesmith2.Runesmith2Code.Field;

#endregion

namespace Runesmith2.Runesmith2Code.Extensions;

public static class CardModelExtension
{
    public class RunesmithCardModelModifier(CardModel cardModel)
    {
        private bool _justEnhanced;

        public bool JustEnhanced
        {
            get
            {
                var ret = _justEnhanced;
                _justEnhanced = false;
                return ret;
            }
            set => _justEnhanced = value;
        }

        private bool _justStasis;

        public bool JustStasis
        {
            get
            {
                var ret = _justStasis;
                _justStasis = false;
                return ret;
            }
            private set => _justStasis = value;
        }

        private int _enhanced;

        public int Enhanced
        {
            get => _enhanced;
            set
            {
                CardModel.AssertMutable();
                _enhanced = Math.Clamp(value, 0, 999999);
                if (value <= 0) return;
                JustEnhanced = true;
                EnhanceChanged?.Invoke();
            }
        }

        public CardModel CardModel { get; set; } = cardModel;

        public bool Stasis
        {
            get;
            set
            {
                CardModel.AssertMutable();
                field = value;
                if (field) JustStasis = true;
                StasisChanged?.Invoke();
            }
        }

        public RunesmithCardModelModifier Clone(CardModel cardModel)
        {
            var ret = (RunesmithCardModelModifier)MemberwiseClone();
            ret.CardModel = cardModel;
            return ret;
        }

        public void ClearFlags()
        {
            _justEnhanced = false;
            _justStasis = false;
        }

        public event Action? EnhanceChanged;
        public event Action? StasisChanged;
    }

    extension(CardModel cardModel)
    {
        public RunesmithCardModelModifier GetCardModelModifier()
        {
            return RunesmithField.Modifier[cardModel]!;
        }

        public void AddEnhance(int amount, bool skipVisuals = false)
        {
            if (!cardModel.IsMutable) return;
            var modifier = cardModel.GetCardModelModifier();
            modifier.Enhanced += amount;
            if (skipVisuals)
                modifier.JustEnhanced = false;
        }

        public bool IsImproved()
        {
            return cardModel.IsUpgraded || cardModel.Enchantment != null || cardModel.IsEnhanced() ||
                   cardModel.IsStasis();
        }

        public bool IsEnhanced()
        {
            return cardModel.GetCardModelModifier().Enhanced > 0;
        }

        public int GetEnhance()
        {
            return cardModel.GetCardModelModifier().Enhanced;
        }

        public decimal GetEnhanceMultiplier()
        {
            if (cardModel is ICardEnhanceMult cardEnhanceMult)
                return 0.5m * cardModel.GetCardModelModifier().Enhanced * cardEnhanceMult.EnhanceMult;

            return 0.5m * cardModel.GetCardModelModifier().Enhanced;
        }

        public void ClearEnhance()
        {
            if (!cardModel.IsMutable) return;
            cardModel.GetCardModelModifier().Enhanced = 0;
        }

        public void SetStasis(bool stasis)
        {
            if (!cardModel.IsMutable) return;
            if (stasis && cardModel is Runesmith2Card { BlockStasis: true }) return;
            cardModel.GetCardModelModifier().Stasis = stasis;
        }

        public bool IsStasis()
        {
            return cardModel.GetCardModelModifier().Stasis;
        }

        public bool HasPotency()
        {
            return cardModel.DynamicVars.ContainsKey(PotencyVar.defaultName) &&
                   cardModel.DynamicVars[PotencyVar.defaultName].BaseValue > 0;
        }

        public bool CanEnhance()
        {
            if (cardModel.Type == CardType.Attack) return true;

            if (cardModel.GainsBlock) return true;
            
            if (cardModel.HasPotency())
                return true;

            // Probably not fool-proof but should help cover cases where Block is added as enchantment or card modifier 
            var cardVarNames = new HashSet<string>([]);
            if(cardModel.Enchantment != null)
            {
                cardVarNames.UnionWith(cardModel.Enchantment.DynamicVars.Values.Where(v => v.BaseValue > 0).Select(v => v.Name));
            }
            foreach (var cardModifier in cardModel.GetModifiers())
            {
                cardVarNames.UnionWith(cardModifier.DynamicVars.Values.Where(v => v.BaseValue > 0).Select(v => v.Name));
            }
            // Consider checking for damage var too?
            HashSet<string> dynamicVarFilter = [BlockVar.defaultName, CalculatedBlockVar.defaultName];
            if (cardVarNames.Any(name => dynamicVarFilter.Contains(name))) return true;

            return false;
        }

        public bool CanStasis()
        {
            return cardModel.CanEnhance() && !cardModel.IsStasis();
        }
    }
}
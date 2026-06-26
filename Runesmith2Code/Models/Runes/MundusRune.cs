#region

using Godot;
using MegaCrit.Sts2.Core.Models;
using Runesmith2.Runesmith2Code.Cards;
using Runesmith2.Runesmith2Code.Cards.Uncommon;
using Runesmith2.Runesmith2Code.Nodes.Runes;

#endregion

namespace Runesmith2.Runesmith2Code.Models.Runes;

// Does nothing
public class MundusRune : RuneModel
{
    public override decimal PassiveVal { get; set; } = 4;
    public override int ChargeVal { get; set; } = 3;

    public override bool UsePotency => true;

    public override bool CanPassive => false;

    public override ChargeDepletionType ChargeDepletion => ChargeDepletionType.None;
    public override (decimal, decimal) BottomValue => (PassiveVal, PassiveVal);
    public override (Color, Color, Color) BottomBreakColor => NRune.DefaultFontColor;

    public override Runesmith2RecipeCard RecipeCard => ModelDb.Get<Mundus>();
}
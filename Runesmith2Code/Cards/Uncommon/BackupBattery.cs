#region

using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Runesmith2.Runesmith2Code.Commands;
using Runesmith2.Runesmith2Code.DynamicVars;
using Runesmith2.Runesmith2Code.HoverTips;
using Runesmith2.Runesmith2Code.Structs;

#endregion

namespace Runesmith2.Runesmith2Code.Cards.Uncommon;

public class BackupBattery : Runesmith2Card
{
    public BackupBattery() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithVar(new EnergyVar(1));
        WithVar(new ElementsVar(2).WithUpgrade(1));
        WithEnergyTip();
        WithTip(RunesmithHoverTip.Elements);
        WithKeyword(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
        await RunesmithPlayerCmd.GainElements(new Elements(this), Owner, play);
    }
}
#region

using System.Globalization;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Runesmith2.Runesmith2Code.Commands;
using Runesmith2.Runesmith2Code.DynamicVars;
using Runesmith2.Runesmith2Code.Extensions;
using Runesmith2.Runesmith2Code.Structs;

#endregion

namespace Runesmith2.Runesmith2Code.Powers;

public class LithiumIonPower : Runesmith2Power
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner)) return;
        if (Owner.Player == null) return;
        
        var elements = Owner.Player.PlayerCombatState?.GetElements() ?? new Elements();
        if (elements.Aqua > 0)
        {
            Flash();
            await RunesmithPlayerCmd.LoseElements(Elements.WithAqua(1), Owner.Player);
            await PowerCmd.Apply<AmpPower>(new ThrowingPlayerChoiceContext(), Owner, Amount, Owner, null);
        }
    }
}
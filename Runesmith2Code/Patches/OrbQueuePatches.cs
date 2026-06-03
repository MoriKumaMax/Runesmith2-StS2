#region

using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Orbs;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Runesmith2.Runesmith2Code.Field;

#endregion

namespace Runesmith2.Runesmith2Code.Patches;

// Ties the RuneQueue end turn triggers to the OrbQueue's triggers
// Start of turn trigger is done at Hook.BeforeHandDraw using SingletonModel
[HarmonyPatch(typeof(OrbQueue), nameof(OrbQueue.BeforeTurnEnd))]
internal class OrbQueueBeforeTurnEndPatch
{
    [HarmonyPostfix]
    private static async Task Postfix(Task results, PlayerChoiceContext choiceContext, OrbQueue __instance)
    {
        await results;
        var playerCombatState = __instance._owner.PlayerCombatState;
        var runeQueue = playerCombatState != null ? RunesmithField.RunesmithCombatState[playerCombatState]?.RuneQueue : null;
        if (runeQueue == null) return;

        await runeQueue.BeforeTurnEnd(choiceContext);
    }
}
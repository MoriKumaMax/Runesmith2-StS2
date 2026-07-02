#region

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

#endregion

namespace Runesmith2.Runesmith2Code.Hooks;

public interface IAfterCardEnhanced
{
    public Task AfterCardEnhanced(PlayerChoiceContext choiceContext, CardModel card, CardPlay? cardPlay, int enhanceAmount);
}
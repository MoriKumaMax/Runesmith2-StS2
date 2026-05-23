#region

using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Runs;

#endregion

namespace Runesmith2.Runesmith2Code.Rewards;

public class CardEnchantRewardMessage : ICustomMessage
{
    public bool WasSkipped { get; set; }
    public ModelId EnchantId { get; set; } = ModelId.none;
    public int EnchantAmount { get; set; }

    public void Serialize(PacketWriter writer)
    {
        writer.WriteBool(WasSkipped);
        writer.WriteFullModelId(EnchantId);
        writer.WriteInt(EnchantAmount);
    }

    public void Deserialize(PacketReader reader)
    {
        WasSkipped = reader.ReadBool();
        EnchantId = reader.ReadFullModelId();
        EnchantAmount = reader.ReadInt();
    }

    public void HandleMessage(ulong senderId)
    {
        if (WasSkipped) return;
        var player = RunManager.Instance.DebugOnlyGetState()?.GetPlayer(senderId);
        if (player == null) return;
    }

    public bool ShouldBroadcast => false;
    public NetTransferMode Mode => NetTransferMode.Reliable;
    public LogLevel LogLevel => LogLevel.Debug;
}
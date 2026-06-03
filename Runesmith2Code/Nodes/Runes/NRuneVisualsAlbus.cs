using Runesmith2.Runesmith2Code.Extensions;

namespace Runesmith2.Runesmith2Code.Nodes.Runes;

public partial class NRuneVisualsAlbus : NRuneVisuals
{
    protected override Action CustomTrigger => () =>
    {
        var track = SpineAnimation.SetAnimation("trigger", false, 1);
        track?.SetMixBlend(MixBlend.MixBlend_Replace);
    };

    protected override void OnTriggerCompleted()
    {
        // Restore idle animation
        var track1Data = TrackDict[1];
        var track = SpineAnimation.SetAnimation("idle/loop_2", true, 1);
        if (track == null) return;
        track.SetTrackTime(0);
        track.SetTimeScale(track1Data.TimeScale);
        track1Data.Track = track;
    }
}
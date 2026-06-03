using Godot;

namespace Runesmith2.Runesmith2Code.Nodes.Runes;

public partial class NRuneVisualsAccel : NRuneVisuals
{
    private float _currTime;
    
    protected override Action CustomTrigger => () =>
    {
        var track0TimeScale = TrackDict[0].TimeScale;
        
        var tween = CreateTween();
        tween.TweenCallback(Callable.From(OnTriggerEnd)).SetDelay(0.1);
        tween.TweenProperty(this, "CurrTimeScale", 6 * track0TimeScale, 0.2).SetEase(Tween.EaseType.InOut)
            .SetTrans(Tween.TransitionType.Quad);
        tween.TweenInterval(0.2);
        tween.TweenProperty(this, "CurrTimeScale", track0TimeScale, 0.5).SetEase(Tween.EaseType.InOut)
            .SetTrans(Tween.TransitionType.Quad);
        
        SetAnimTimeScaleTween(tween);
    };
}
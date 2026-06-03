#region

using Godot;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Random;
using Runesmith2.Runesmith2Code.Extensions;

#endregion

namespace Runesmith2.Runesmith2Code.Nodes.Runes;

[GlobalClass]
public partial class NRuneVisuals : Node2D
{
    private const string TimeScaleKey = "time_scale";
    private const string TriggerKey = "trigger";

    private readonly List<string> _currAnimationList = [];

    private NRuneParticlesContainer? _backParticles;

    private NRuneParticlesContainer? _frontParticles;

    private CpuParticles2D? _breakParticles;

    private bool _hasTrigger;

    private Color _targetModulate = Colors.White;

    private float _targetTimeScale = 1f;

    protected readonly Dictionary<int, TrackData> TrackDict = [];

    private int _triggerTrack = 1;

    private Tween? _tween;
    private Tween? _tween2;
    private Node2D? _visual;
    private MegaSprite? _spineVisuals;

    protected virtual Action? CustomTrigger => null;

    protected virtual MixBlend TriggerMixBlend => MixBlend.MixBlend_Add;

    private bool IsSpineNode
    {
        get
        {
            if (IsInstanceValid(_visual)) return _visual.GetClass() == "SpineSprite";
            return false;
        }
    }

    protected SpineAnimationAccess SpineAnimation => new(_spineVisuals);

    private float CurrTimeScale
    {
        set
        {
            field = value;
            SetAllTimeScale(field);
        }
        get;
    }

    public override void _Ready()
    {
        _visual = GetNode<Node2D>("%Visuals");
        _backParticles = GetNode<NRuneParticlesContainer>("%BackParticles");
        _frontParticles = GetNode<NRuneParticlesContainer>("%FrontParticles");
        _breakParticles = GetNode<CpuParticles2D>("%BreakParticles");

        if (IsSpineNode)
        {
            _spineVisuals = new MegaSprite(_visual);
            if (_spineVisuals.GetSkeleton()?.GetData() == null)
            {
                GD.PushWarning($"Spine skeleton data failed to load for {Name}, disabling spine animation.");
                _spineVisuals = null;
            }

            _spineVisuals?.ConnectAnimationEvent(
                Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(OnAnimationEvent));
            _spineVisuals?.ConnectAnimationCompleted(
                Callable.From<GodotObject, GodotObject, GodotObject>(OnAnimationCompleted));
        }

        OnStart();
    }

    private void OnAnimationCompleted(GodotObject _, GodotObject __, GodotObject trackEntry)
    {
        var track = new MegaTrackEntry(trackEntry);
        if (track.GetAnimation().GetName() == "trigger")
        {
            OnTriggerCompleted();
            AnimationFinished(TriggerKey);
        }
    }

    protected virtual void OnTriggerCompleted()
    {
        
    }

    private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
    {
        if (new MegaEvent(spineEvent).GetData().GetEventName() == "trigger_end") OnTriggerEnd();
    }

    protected void OnStart()
    {
        var skelData = _spineVisuals?.GetSkeleton()?.GetData();
        if (skelData == null) return;

        _hasTrigger = skelData.FindAnimation("trigger") != null;

        // default option, don't call spine functions too much
        if (skelData.FindAnimation("idle_loop") != null)
        {
            var track = SpineAnimation.SetAnimation("idle_loop", true);
            var timeScale = Rng.Chaotic.NextFloat(0.9f, 1.1f);
            if (track == null) return;
            track.SetTimeScale(timeScale);
            track.SetTrackTime(Rng.Chaotic.NextFloat() * track.GetAnimationEnd());
            TrackDict.Add(0, new TrackData(track, timeScale));
            return;
        }

        // Use layered animation if there's idle/loop_N
        var index = 0;
        while (true)
        {
            var name = $"idle/loop_{index + 1}";
            if (skelData.FindAnimation(name) != null)
            {
                var track = SpineAnimation.SetAnimation(name, true, index);
                var timeScale = Rng.Chaotic.NextFloat(0.9f, 1.1f);
                if (track != null)
                {
                    track.SetTimeScale(timeScale);
                    track.SetTrackTime(Rng.Chaotic.NextFloat() * track.GetAnimationEnd());
                    TrackDict.Add(index, new TrackData(track, timeScale));
                }
            }
            else
            {
                _triggerTrack = index + 1;
                return;
            }

            index++;
        }
    }

    // Called when the Rune is triggered from any source
    public virtual void OnTrigger()
    {
        _backParticles?.Restart();
        if (!_hasTrigger && CustomTrigger == null) return;

        _currAnimationList.Add(TriggerKey);
        if (CustomTrigger == null)
        {
            var track = SpineAnimation.SetAnimation("trigger", false, _triggerTrack);
            track?.SetMixBlend(TriggerMixBlend);
            SpineAnimation.GetAnimationState()?.AddEmptyAnimation(_triggerTrack);
        }
        else
        {
            CustomTrigger();
        }
    }

    // Called when trigger_end event has been emitted from animation. Used for triggering particles.
    // Can also be called manually if no event was included
    protected virtual void OnTriggerEnd()
    {
        _frontParticles?.PlayOneShot();
    }

    public void OnBreak()
    {
        _breakParticles?.Restart();
        _frontParticles?.PlayOneShot();
        _tween?.Kill();
        _tween2?.Kill();
        _visual?.Modulate = Colors.Transparent;
    }

    protected void SetAnimTimeScaleTween(Tween tween)
    {
        if (_tween != null && _tween.IsRunning()) _tween.Kill();
        _tween = tween;

        _currAnimationList.Add(TimeScaleKey);
        _tween.TweenCallback(Callable.From(() => AnimationFinished(TimeScaleKey)));
        MainFile.Logger.Info("Setting anim time scale tween");
    }

    private void SetAllTimeScale(float scale)
    {
        foreach (var data in TrackDict.Values) data.Track.SetTimeScale(scale * data.TimeScale);
    }

    private void AnimationFinished(string source)
    {
        _currAnimationList.Remove(source);

        UpdateAfterAnimationFinished();
    }

    private void UpdateAfterAnimationFinished()
    {
        if (_currAnimationList.Count != 0) return;

        if (_targetModulate == _visual?.Modulate && Math.Abs(CurrTimeScale - _targetTimeScale) < 0.01) return;
        if (_tween2 != null && _tween2.IsRunning()) _tween2.Kill();
        _tween2 = CreateTween();
        _tween2.SetParallel();
        _tween2.TweenProperty(_visual, "modulate", _targetModulate, 0.1);
        _tween2.TweenProperty(this, "CurrTimeScale", _targetTimeScale, 0.1);
    }

    // Change visual based on charge status. If trigger anim and/or anim scale tween is playing, waits until all are finished playing.
    public void SetChargeStatus(bool isDepleted, Color darkenedColor)
    {
        _targetModulate = isDepleted ? darkenedColor : Colors.White;
        _targetTimeScale = isDepleted ? 0 : 1;
        
        UpdateAfterAnimationFinished();
    }

    protected class TrackData(MegaTrackEntry track, float timeScale)
    {
        internal MegaTrackEntry Track { set; get; } = track;
        internal float TimeScale => timeScale;
    }
}
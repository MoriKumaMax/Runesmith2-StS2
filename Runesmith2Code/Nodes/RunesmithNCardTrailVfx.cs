using Godot;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace Runesmith2.Runesmith2Code.Nodes;

public partial class RunesmithNCardTrailVfx : NCardTrailVfx
{
    public override void _Ready()
    {
        var outerTrail = GetNode<RunesmithNCardTrail>("%OuterTrail");
        outerTrail.Texture = ResourceLoader.Load<CompressedTexture2D>("res://images/packed/vfx/trail.png");
        var innerTrail = GetNode<RunesmithNCardTrail>("%InnerTrail");
        innerTrail.Texture = ResourceLoader.Load<CompressedTexture2D>("res://images/packed/vfx/trail2.png");
        var bigSparks = GetNode<CpuParticles2D>("%BigSparks");
        bigSparks.Texture = ResourceLoader.Load<CompressedTexture2D>("res://images/vfx/brush_particle_2.png");
        var littleSparks = GetNode<CpuParticles2D>("%LittleSparks");
        littleSparks.Texture = ResourceLoader.Load<CompressedTexture2D>("res://images/vfx/vfx_ghostly_power_up/sparkle.png");
        var sprite2 = GetNode<Sprite2D>("%Sprite2D2");
        sprite2.Texture = ResourceLoader.Load<CompressedTexture2D>("res://images/packed/vfx/small_card_silhouette.png");
        var sprite3 = GetNode<Sprite2D>("%Sprite2D3");
        sprite3.Texture = ResourceLoader.Load<CompressedTexture2D>("res://images/packed/vfx/small_card_silhouette.png");
        base._Ready();
    }
}
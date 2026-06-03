using Godot;
using Godot.Collections;

namespace Runesmith2.Runesmith2Code.Nodes;

public partial class NRuneParticlesContainer : Node2D
{
    private readonly Array<GpuParticles2D> _particles = [];

    private readonly Array<CpuParticles2D> _cpuParticles = [];
    
    public override void _Ready()
    {
        var children = GetChildren();
        _particles.AddRange(children.OfType<GpuParticles2D>());
        _cpuParticles.AddRange(children.OfType<CpuParticles2D>());
    }
    
    public void Restart()
    {
        foreach (var particle in _particles)
        {
            particle.Restart();
        }

        foreach (var particle in _cpuParticles)
        {
            particle.Restart();
        }
    }
    
    public void PlayOneShot()
    {
        foreach (var particle in _particles)
        {
            if (particle.Emitting)
            {
                var newEmitter = (GpuParticles2D) particle.Duplicate();
                newEmitter.Emitting = true;
                AddChild(newEmitter);
                newEmitter.Restart();
                newEmitter.Connect("finished", Callable.From(() => newEmitter.QueueFree()));
            }
            else
            {
                particle.Restart();
            }
        }
        foreach (var particle in _cpuParticles)
        {
            if (particle.Emitting)
            {
                var newEmitter = (CpuParticles2D) particle.Duplicate();
                newEmitter.Emitting = true;
                AddChild(newEmitter);
                newEmitter.Restart();
                newEmitter.Connect("finished", Callable.From(() => newEmitter.QueueFree()));
            }
            else
            {
                particle.Restart();
            }
        }
    }
}
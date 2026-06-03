using Godot;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;

namespace Runesmith2.Runesmith2Code.Extensions;

public static class MegaSpineExtensions
{
    public static void SetMixBlend(this MegaTrackEntry trackEntry, MixBlend mixBlend)
    {
        trackEntry.BoundObject.Call("set_mix_blend", (int) mixBlend);
    }
}

public enum MixBlend
{
    MixBlend_Setup = 0,
    MixBlend_First = 1,
    MixBlend_Replace = 2,
    MixBlend_Add = 3
}
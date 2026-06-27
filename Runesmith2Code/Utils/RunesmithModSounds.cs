using BaseLib.Audio;
using MegaCrit.Sts2.Core.Models.Cards;

namespace Runesmith2.Runesmith2Code.Utils;

public static class RunesmithModSounds
{
    public static readonly ModSound GrindstoneSfx = new("res://Runesmith2/audio/grindstone_sharpen.ogg");
    public static readonly ModSound StasisSfx = new("res://Runesmith2/audio/stasis_another.ogg");
    public static readonly ModSound RuneCraftSfx = new("res://Runesmith2/audio/rune_craft.ogg");
    public static readonly ModSound RuneChargeSfx = new("res://Runesmith2/audio/rune_charge.ogg");
    public static readonly ModSound RunePassiveSfx = new("res://Runesmith2/audio/rune_passive.ogg");
    public static readonly ModSound RuneBreakSfx = new("res://Runesmith2/audio/rune_break.ogg");
    public static readonly ModSound LaserTurretSfx = new("res://Runesmith2/audio/laser_turret.ogg");
    public static readonly ModSound EnhanceSfx = new("res://Runesmith2/audio/enhance.ogg");
    public static readonly ModSound ElementsGainSfx = new("res://Runesmith2/audio/elements_gain.ogg");


    public static void PlayGrindStoneSfx()
    {
        GrindstoneSfx.Play(pitchVariation: 0.1f);
    }
    
    public static void PlayStasisSfx()
    {
        StasisSfx.Play(pitchVariation: 0.05f);
    }
    
    public static void PlayRuneBreakSfx()
    {
        RuneBreakSfx.Play(pitchVariation: 0.1f);
    }
    
    public static void PlayRuneCraftSfx()
    {
        RuneCraftSfx.Play(pitchVariation: 0.07f);
    }
    
    public static void PlayRunePassiveSfx()
    {
        RunePassiveSfx.Play(pitchVariation: 0.07f);
    }

    public static void PlayRuneChargeSfx()
    {
        RuneChargeSfx.Play(pitchVariation: 0.05f);
    }
    
    public static void PlayLaserTurretSfx()
    {
        LaserTurretSfx.Play(pitchVariation: 0.1f);
    }
    
    public static void PlayEnhanceSfx()
    {
        EnhanceSfx.Play(pitchVariation: 0.05f);
    }
    
    public static void PlayElementsGainSfx()
    {
        ElementsGainSfx.Play(pitchVariation: 0.05f);
    }
}
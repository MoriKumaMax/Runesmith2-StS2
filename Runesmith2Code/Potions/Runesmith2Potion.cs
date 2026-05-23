#region

using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using Runesmith2.Runesmith2Code.Character;
using Runesmith2.Runesmith2Code.Extensions;

#endregion

namespace Runesmith2.Runesmith2Code.Potions;

[Pool(typeof(Runesmith2PotionPool))]
public abstract class Runesmith2Potion : CustomPotionModel
{
    public override string? CustomPackedImagePath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionImagePath();
            return ResourceLoader.Exists(path) ? path : null;
        }
    }

    public override string? CustomPackedOutlinePath 
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".PotionImagePath();
            return ResourceLoader.Exists(path) ? path : null;
        }
    }
}
using BepInEx;
using HarmonyLib;

namespace MoreTrackSpeeds;

[BepInProcess("SpinRhythm.exe")]
[BepInPlugin(Guid, Name, Version)]
public class Plugin : BaseUnityPlugin
{
    public const string Guid = "xyz.bacur.plugins.moretrackspeeds";
    public const string Name = "MoreTrackSpeeds";
    public const string Version = "1.0.0";

    private static Harmony _harmony;

    protected void Awake()
    {
        _harmony = new Harmony(Guid);
        _harmony.PatchAll(typeof(Patches.TrackSpeedHandler));
    }

    protected void OnDestroy()
    {
        _harmony.UnpatchSelf();
    }
}

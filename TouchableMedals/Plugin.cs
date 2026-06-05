using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace TouchableMedals;

[BepInProcess("SpinRhythm.exe")]
[BepInPlugin(Guid, Name, Version)]
public class Plugin : BaseUnityPlugin
{
    public const string Guid = "xyz.bacur.plugins.touchablemedals";
    public const string Name = "TouchableMedals";
    public const string Version = "1.0.0";

    internal static new ManualLogSource Logger;
    internal static Harmony Patcher;

    protected void Awake()
    {
        Logger = base.Logger;
        Patcher = new Harmony(Guid);
        Patcher.PatchAll(typeof(Patches.MedalHandler));
    }

    protected void OnDestroy()
    {
        Patcher.UnpatchSelf();
    }
}

using HarmonyLib;
using System.Collections.Generic;

namespace MoreTrackSpeeds.Patches
{
    [HarmonyPatch]
    internal static class TrackSpeedHandler
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(XDMenuPlay.Customise.TrackSpeeds), nameof(XDMenuPlay.Customise.TrackSpeeds.GetOptionCallbacks))]
        private static void GetOptionCallbacks()
        {
            XDMenuPlay.Customise.TrackSpeeds._trackSpeeds = new List<int>();
            XDMenuPlay.Customise.TrackSpeeds._trackSpeeds.Add(0);
            foreach (int item in new IntRange(10, 121))
            {
                XDMenuPlay.Customise.TrackSpeeds._trackSpeeds.Add(item);
            }
        }
    }
}

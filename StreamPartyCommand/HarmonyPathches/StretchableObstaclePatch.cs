using HarmonyLib;
using System;
using UnityEngine;

namespace StreamPartyCommand.HarmonyPathches
{
    [HarmonyPatch(typeof(StretchableObstacle), nameof(StretchableObstacle.SetAllProperties),
    new Type[] { typeof(float), typeof(float), typeof(float), typeof(Color), typeof(float) })]
    public class StretchableObstaclePatch
    {
        public static Color WallColor { get; set; }

        public static void Prefix(ref float width, ref float height, ref float length, ref Color color, ref float manualUvOffset)
        {
            if (!Enable) {
                return;
            }
            color = WallColor;
        }

        internal static bool Enable { get; set; }
    }
}

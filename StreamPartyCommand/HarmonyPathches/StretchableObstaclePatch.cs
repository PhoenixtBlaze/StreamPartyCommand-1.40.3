﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace StreamPartyCommand.HarmonyPathches
{
    [HarmonyPatch(typeof(StretchableObstacle), nameof(StretchableObstacle.SetSizeAndColor), new Type[] { typeof(float), typeof(float), typeof(float), typeof(Color) })]
    public class StretchableObstaclePatch
    {
        public static Color WallColor { get; set; }

        public static void Prefix(ref float width, ref float height, ref float length, ref Color color)
        {
            if (!Enable) {
                return;
            }
            color = WallColor;
        }
        internal static bool Enable { get; set; }
    }
}

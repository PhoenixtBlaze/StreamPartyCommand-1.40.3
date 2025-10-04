using System.Diagnostics;
using HarmonyLib;
using Zenject;

namespace StreamPartyCommand.Diagnostics
{
    // Patch BSML's getter that emits the error message
    [HarmonyPatch(typeof(BeatSaberMarkupLanguage.BeatSaberUI), "DiContainer", MethodType.Getter)]
    internal static class BSMLDiContainerGetterPatch
    {
        private static void Postfix(DiContainer __result)
        {
            if (__result == null) {
                var st = new StackTrace(true);
                // Trim the stack to find the first non-BSML caller (the mod that touched it)
                foreach (var frame in st.GetFrames()) {
                    var t = frame.GetMethod()?.DeclaringType;
                    var asm = t?.Assembly?.GetName()?.Name;
                    if (!string.IsNullOrEmpty(asm) && asm != "BeatSaberMarkupLanguage") {
                        StreamPartyCommand.Plugin.Log?.Error(
                            $"BSML.DiContainer early access by: {asm}\n{st}");
                        break;
                    }
                }
            }
        }
    }
}
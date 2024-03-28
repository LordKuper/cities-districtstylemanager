using CitiesHarmony.API;
using HarmonyLib;

namespace DistrictStyleManager.Patches
{
    public static class Patcher
    {
        private const string HarmonyId = "LordKuper.DistrictStyleManager";
        internal static bool Patched;

        public static void PatchAll()
        {
            if (Patched) { return; }
            if (HarmonyHelper.IsHarmonyInstalled)
            {
                Logger.Info("applying Harmony patches");
                var harmony = new Harmony(HarmonyId);
                harmony.PatchAll();
                Patched = true;
            }
            else { Logger.Error("Harmony not found"); }
        }

        public static void UnpatchAll()
        {
            if (!Patched) { return; }
            if (HarmonyHelper.IsHarmonyInstalled)
            {
                Logger.Info("reverting Harmony patches");
                var harmonyInstance = new Harmony(HarmonyId);
                harmonyInstance.UnpatchAll(HarmonyId);
                Patched = false;
            }
            else { Logger.Error("Harmony not found"); }
        }
    }
}
using System.Diagnostics.CodeAnalysis;
using ColossalFramework;
using ColossalFramework.Math;
using HarmonyLib;
using JetBrains.Annotations;

namespace DistrictStyleManager.Patches
{
    [HarmonyPatch, UsedImplicitly, SuppressMessage("ReSharper", "InconsistentNaming"),
     SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "Harmony")]
    public static class PrivateBuildingAIPatch
    {
        [HarmonyPrefix, HarmonyPatch(typeof(PrivateBuildingAI), nameof(PrivateBuildingAI.GetUpgradeInfo)),
         UsedImplicitly]
        public static bool GetUpgradeInfoPrefix(PrivateBuildingAI __instance, ref BuildingInfo __result,
            ushort buildingID, ref Building data)
        {
            if (data.m_level == 4)
            {
                __result = null;
                return false;
            }
            if (__instance.m_upgradeTarget != null &&
                __instance.m_upgradeTarget.m_class.m_service == __instance.m_info.m_class.m_service &&
                __instance.m_upgradeTarget.m_class.m_subService == __instance.m_info.m_class.m_subService &&
                __instance.m_upgradeTarget.m_class.m_level == __instance.m_info.m_class.m_level + 1 &&
                __instance.m_upgradeTarget.GetWidth() == __instance.m_info.GetWidth() &&
                __instance.m_upgradeTarget.GetLength() == __instance.m_info.GetLength() &&
                __instance.m_upgradeTarget.m_zoningMode == __instance.m_info.m_zoningMode)
            {
                __result = __instance.m_upgradeTarget;
                return false;
            }
            var r = new Randomizer(buildingID);
            for (var index = 0; index <= data.m_level; ++index) { r.Int32(1000U); }
            var level = (ItemClass.Level) (data.m_level + 1);
            var instance = Singleton<DistrictManager>.instance;
            var district = instance.GetDistrict(data.m_position);
            var style = instance.m_districts.m_buffer[district].m_Style;
            __result = Singleton<BuildingManager>.instance.GetRandomBuildingInfo(ref r,
                __instance.m_info.m_class.m_service, __instance.m_info.m_class.m_subService, level, data.Width,
                data.Length, __instance.m_info.m_zoningMode, style);
            if (__result == null) { __result = __instance.m_info; }
            return false;
        }
    }
}
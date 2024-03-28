using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ColossalFramework;
using HarmonyLib;
using JetBrains.Annotations;

namespace DistrictStyleManager.Patches
{
    [HarmonyPatch, UsedImplicitly]
    public class BuildingManagerPatch
    {
        [HarmonyPostfix, HarmonyPatch(typeof(BuildingManager), nameof(BuildingManager.GetRandomBuildingInfo)),
         UsedImplicitly, SuppressMessage("ReSharper", "InconsistentNaming"),
         SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "Harmony"),
         SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Harmony")]
        public static void GetRandomBuildingPostfix(ref BuildingInfo __result, ItemClass.Service service,
            ItemClass.SubService subService, int width, int length, BuildingInfo.ZoningMode zoningMode, int style)
        {
            if (__result == null) { return; }
            var districtStyle = Singleton<DistrictManager>.instance.m_Styles[style];
            var affectedServices = (HashSet<int>) AccessTools.Field(districtStyle.GetType(), "m_AffectedServices")
                .GetValue(districtStyle);
            var indexRangeStart = DistrictStyle.GetServiceLevelIndex(service, subService, ItemClass.Level.Level1);
            var indexRangeEnd = DistrictStyle.GetServiceLevelIndex(service, subService, ItemClass.Level.Level5);
            var affectsZoneType = affectedServices.Any(index => index >= indexRangeStart && index <= indexRangeEnd);
            if (affectsZoneType && !districtStyle.Contains(__result)) { __result = null; }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using ColossalFramework;
using HarmonyLib;
using JetBrains.Annotations;

namespace DistrictStyleManager.Managers
{
    [UsedImplicitly]
    public class DistrictStyleManager : Singleton<DistrictStyleManager>
    {
        internal static void DeleteDistrictStyle(DistrictStyle style)
        {
            var districtStyleId = GetDistrictStyleId(style.FullName) + 1;
            var districts = Singleton<DistrictManager>.instance.m_districts.m_buffer;
            for (var i = 0; i < districts.Length; i++)
            {
                if (districts[i].m_Style == districtStyleId) { districts[i].m_Style = 0; }
                if (districts[i].m_Style > districtStyleId) { districts[i].m_Style--; }
            }
            Singleton<DistrictManager>.instance.m_Styles = Singleton<DistrictManager>.instance.m_Styles
                .Where(instanceStyle => !instanceStyle.FullName.Equals(style.FullName)).ToArray();
        }

        [CanBeNull]
        internal static DistrictStyle GetDistrictStyle(string name)
        {
            var styles = DistrictManager.instance.m_Styles;
            if (styles == null || styles.Length == 0) { return null; }
            try { return styles.First(ds => ds.FullName.Equals(name)); }
            catch (InvalidOperationException) { return null; }
        }

        private static int GetDistrictStyleId(string name)
        {
            var styles = Singleton<DistrictManager>.instance.m_Styles;
            if (styles == null) { return -1; }
            for (ushort i = 0; i < styles.Length; i++)
            {
                if (styles[i].FullName.Equals(name)) { return i; }
            }
            return -1;
        }

        internal static void RefreshDistrictStyleAffectedService(DistrictStyle style)
        {
            var field = AccessTools.Field(style.GetType(), "m_AffectedServices") ??
                throw new Exception($"District style {style.FullName} does not have field m_AffectedServices");
            var buildingInfos = style.GetBuildingInfos();
            if (buildingInfos == null || buildingInfos.Length <= 0) { field.SetValue(style, new HashSet<int>()); }
            else
            {
                var affectedServices = new HashSet<int>();
                var services = new Dictionary<string, int>();
                foreach (var buildingInfo in buildingInfos)
                {
                    var service = buildingInfo.GetService();
                    var subService = buildingInfo.GetSubService();
                    var level = buildingInfo.m_class != null ? buildingInfo.m_class.m_level : ItemClass.Level.Level1;
                    affectedServices.Add(DistrictStyle.GetServiceLevelIndex(service, subService, level));
                    var key = $"{service}.{subService}";
                    if (services.ContainsKey(key)) { services[key]++; }
                    else { services[key] = 1; }
                }
                field.SetValue(style, affectedServices);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ColossalFramework;
using HarmonyLib;
using JetBrains.Annotations;

namespace DistrictStyleManager.Managers
{
    [UsedImplicitly]
    internal class VirtualStyleManager : Singleton<VirtualStyleManager>
    {
        public const byte MaxDistrictCount = 128;
        public const string VanillaStyleName = "DSM-Vanilla";
        public const string VirtualStylePackage = "DSMVirtual";
        private const string VirtualStylePrefix = "DSMVirtual-";

        private static readonly Dictionary<byte, HashSet<string>> VirtualStyles =
            new Dictionary<byte, HashSet<string>>();

        private static DistrictStyle CreateVirtualStyle(string name)
        {
            var style = new DistrictStyle(name, false) {PackageName = VirtualStylePackage};
            Singleton<DistrictManager>.instance.m_Styles =
                Singleton<DistrictManager>.instance.m_Styles.AddItem(style).ToArray();
            return style;
        }

        internal static HashSet<string> GetDistrictStyles(byte districtId)
        {
            return VirtualStyles.ContainsKey(districtId)
                ? new HashSet<string>(VirtualStyles.GetValueSafe(districtId))
                : GetVanillaDistrictStyle(districtId);
        }

        internal static HashSet<string> GetStylesToSave(byte districtId)
        {
            return VirtualStyles.ContainsKey(districtId)
                ? new HashSet<string>(VirtualStyles.GetValueSafe(districtId))
                : null;
        }

        private static HashSet<string> GetVanillaDistrictStyle(byte districtId)
        {
            var styleId = DistrictManager.instance.m_districts.m_buffer[districtId].m_Style - 1;
            if (styleId < 0) { return new HashSet<string>(); }
            var style = DistrictManager.instance.m_Styles[styleId];
            return style != null ? new HashSet<string> {style.FullName} : new HashSet<string>();
        }

        private static void GetVirtualStyleNames(byte districtId, out string virtualStyleName,
            out string virtualStyleFullName)
        {
            virtualStyleName = $"{VirtualStylePrefix}{districtId}";
            virtualStyleFullName = $"{VirtualStylePackage}.{virtualStyleName}";
        }

        public static void InitializeVanillaStyle()
        {
            if (DistrictManager.instance.m_Styles.Any(s =>
                    s.PackageName.Equals(VirtualStylePackage) && s.Name.Equals(VanillaStyleName))) { return; }
            var vanillaBuildings = new HashSet<BuildingInfo>();
            for (uint i = 0; i < PrefabCollection<BuildingInfo>.LoadedCount(); i++)
            {
                var building = PrefabCollection<BuildingInfo>.GetLoaded(i);
                if (building == null) { continue; }
                var prefabAi = building.GetAI();
                if (!(prefabAi != null && !prefabAi.ToString().Contains("PloppableRICO.Ploppable") &&
                        (prefabAi is CommercialBuildingAI || prefabAi is ResidentialBuildingAI ||
                            prefabAi is IndustrialBuildingAI || prefabAi is IndustrialExtractorAI ||
                            prefabAi is OfficeBuildingAI))) { continue; }
                if (Regex.Replace(building.name, @"^{{.*?}}\.", "").Contains(".")) { continue; }
                if (building.m_requiredModderPack != 0) { continue; }
                vanillaBuildings.Add(building);
            }
            if (vanillaBuildings.Count > 0)
            {
                var style = CreateVirtualStyle(VanillaStyleName);
                UpdateBuildingInfosInVirtualStyle(vanillaBuildings, style);
            }
        }

        public static void LoadData()
        {
            InitializeVanillaStyle();
            var data = DistrictStyleSerializer.GetSavedData();
            Logger.Info($"data containers loaded = {data.Length}");
            for (var i = 0; i < data.Length; i++)
            {
                var districtId = (byte) i;
                if (DistrictManager.instance.m_districts.m_buffer[i].m_flags == District.Flags.None) { continue; }
                var style = data[i];
                if (style != null && style.StyleNames.Count > 0) { SetDistrictStyles(districtId, style.StyleNames); }
            }
        }

        private static void MergeStylesToVirtual(HashSet<string> districtStyles, DistrictStyle virtualStyle)
        {
            var buildingInfos = new HashSet<BuildingInfo>();
            if (districtStyles != null && districtStyles.Count > 0)
            {
                foreach (var styleName in districtStyles)
                {
                    var districtStyle = DistrictStyleManager.GetDistrictStyle(styleName);
                    if (districtStyle?.GetBuildingInfos() == null || districtStyle.GetBuildingInfos().Length == 0)
                    {
                        continue;
                    }
                    buildingInfos.UnionWith(districtStyle.GetBuildingInfos());
                }
            }
            UpdateBuildingInfosInVirtualStyle(buildingInfos, virtualStyle);
        }

        public static void SetDistrictStyles(byte districtId, HashSet<string> districtStyles)
        {
            var currentDistrictStyleSelection = GetDistrictStyles(districtId);
            if (currentDistrictStyleSelection.SetEquals(districtStyles)) { return; }
            VirtualStyles.Remove(districtId);
            GetVirtualStyleNames(districtId, out var virtualStyleName, out var virtualStyleFullName);
            var virtualStyle = DistrictStyleManager.GetDistrictStyle(virtualStyleFullName);
            if (districtStyles.Count == 0)
            {
                DistrictManager.instance.m_districts.m_buffer[districtId].m_Style = 0;
                if (virtualStyle != null) { DistrictStyleManager.DeleteDistrictStyle(virtualStyle); }
                return;
            }
            VirtualStyles.Add(districtId, districtStyles);
            if (virtualStyle == null) { virtualStyle = CreateVirtualStyle(virtualStyleName); }
            MergeStylesToVirtual(districtStyles, virtualStyle);
            var virtualStyleId = Array.IndexOf(DistrictManager.instance.m_Styles, virtualStyle);
            if (virtualStyleId >= 0)
            {
                DistrictManager.instance.m_districts.m_buffer[districtId].m_Style = (ushort) (virtualStyleId + 1);
            }
            else
            {
                Logger.Error($"virtual style for district {districtId} does not exist!");
                DistrictManager.instance.m_districts.m_buffer[districtId].m_Style = 0;
            }
        }

        private static void UpdateBuildingInfosInVirtualStyle(HashSet<BuildingInfo> buildingInfos, DistrictStyle style)
        {
            var buildingInfosFieldInfo = AccessTools.Field(style.GetType(), "m_Infos");
            if (buildingInfosFieldInfo == null)
            {
                Logger.Error($"district style {style.FullName} does not have field m_Infos");
                return;
            }
            buildingInfosFieldInfo.SetValue(style, buildingInfos);
            DistrictStyleManager.RefreshDistrictStyleAffectedService(style);
        }
    }
}
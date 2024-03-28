using System;
using ColossalFramework;
using ColossalFramework.Globalization;
using DistrictStyleManager.Managers;

namespace DistrictStyleManager
{
    internal static class DistrictStyleHelper
    {
        internal static FastList<DistrictStyle> StoredStyles
        {
            get
            {
                var districtStyles = Singleton<DistrictManager>.instance.m_Styles;
                var resultData = new FastList<DistrictStyle>();
                if (districtStyles.Length <= 0) { return resultData; }
                foreach (var districtStyle in districtStyles)
                {
                    if (districtStyle.PackageName.Equals(VirtualStyleManager.VirtualStylePackage) &&
                        !districtStyle.Name.Equals(VirtualStyleManager.VanillaStyleName,
                            StringComparison.OrdinalIgnoreCase)) { continue; }
                    if (districtStyle.BuiltIn)
                    {
                        var index = districtStyle.Name.IndexOf("BuiltinStyle", StringComparison.OrdinalIgnoreCase);
                        var builtinName = districtStyle.Name.Substring(0, index);
                        SteamHelper.DLC dlcId;
                        switch (builtinName)
                        {
                            case "European":
                                dlcId = SteamHelper.DLC.None;
                                break;
                            case "EuropeanSuburbia":
                                dlcId = SteamHelper.DLC.ModderPack3;
                                break;
                            default:
                                dlcId = (SteamHelper.DLC) Enum.Parse(typeof(SteamHelper.DLC), builtinName, true);
                                break;
                        }
                        if (!SteamHelper.IsDLCOwned(dlcId)) { continue; }
                        resultData.Add(districtStyle);
                    }
                    else
                    {
                        var index = districtStyle.Name.IndexOf("BuiltinStyle", StringComparison.Ordinal);
                        if (index > "ModderPack".Length) { continue; }
                        resultData.Add(districtStyle);
                    }
                }
                return resultData;
            }
        }

        internal static string GetDisplayName(DistrictStyle districtStyle)
        {
            if (!districtStyle.BuiltIn) { return districtStyle.Name; }
            var flag = false;
            if (Singleton<SimulationManager>.exists && Singleton<SimulationManager>.instance.m_metaData != null)
            {
                flag = Singleton<SimulationManager>.instance.m_metaData.m_environment == "Europe";
            }
            var name = districtStyle.Name;
            if (districtStyle.Name.Equals(DistrictStyle.kEuropeanStyleName))
            {
                name = Locale.Get(!flag ? "STYLES_EUROPEAN" : "STYLES_NORMAL");
            }
            else if (districtStyle.Name.Equals(DistrictStyle.kEuropeanSuburbiaStyleName))
            {
                name = Locale.Get("STYLES_EUROPEANSUBURBIA");
            }
            else if (districtStyle.Name.Equals(DistrictStyle.kModderPack5StyleName))
            {
                name = Locale.Get("STYLES_MODDERPACKFIVE");
            }
            else if (districtStyle.Name.Equals(DistrictStyle.kModderPack11StyleName))
            {
                name = Locale.Get("STYLES_MODDERPACKELEVEN");
            }
            else if (districtStyle.Name.Equals(DistrictStyle.kModderPack14StyleName))
            {
                name = Locale.Get("STYLES_MODDERPACKFOURTEEN");
            }
            else if (districtStyle.Name.Equals(DistrictStyle.kModderPack16StyleName))
            {
                name = Locale.Get("STYLES_MODDERPACKSIXTEEN");
            }
            else if (districtStyle.Name.Equals(DistrictStyle.kModderPack18StyleName))
            {
                name = Locale.Get("STYLES_MODDERPACKEIGHTEEN");
            }
            else if (districtStyle.Name.Equals(DistrictStyle.kModderPack20StyleName))
            {
                name = Locale.Get("STYLES_MODDERPACKTWENTY");
            }
            else if (districtStyle.Name.Equals(DistrictStyle.kModderPack21StyleName))
            {
                name = Locale.Get("STYLES_MODDERPACKTWENTYONE");
            }
            return name;
        }

        internal static string GetSubServiceLocalizedName(ItemClass.SubService subService)
        {
            string id;
            string key;
            switch (subService)
            {
                case ItemClass.SubService.None:
                    id = string.Empty;
                    key = string.Empty;
                    break;
                case ItemClass.SubService.ResidentialLow:
                    id = "ZONING_TITLE";
                    key = "ResidentialLow";
                    break;
                case ItemClass.SubService.ResidentialHigh:
                    id = "ZONING_TITLE";
                    key = "ResidentialHigh";
                    break;
                case ItemClass.SubService.CommercialLow:
                    id = "ZONING_TITLE";
                    key = "CommercialLow";
                    break;
                case ItemClass.SubService.CommercialHigh:
                    id = "ZONING_TITLE";
                    key = "CommercialHigh";
                    break;
                case ItemClass.SubService.IndustrialGeneric:
                    id = "ZONING_TITLE";
                    key = "Industrial";
                    break;
                case ItemClass.SubService.IndustrialForestry:
                    id = "DISTRICT_TITLE";
                    key = "SpecializationForest";
                    break;
                case ItemClass.SubService.IndustrialFarming:
                    id = "DISTRICT_TITLE";
                    key = "SpecializationFarming";
                    break;
                case ItemClass.SubService.IndustrialOil:
                    id = "DISTRICT_TITLE";
                    key = "SpecializationOil";
                    break;
                case ItemClass.SubService.IndustrialOre:
                    id = "DISTRICT_TITLE";
                    key = "SpecializationOre";
                    break;
                case ItemClass.SubService.CommercialLeisure:
                    id = "DISTRICT_TITLE";
                    key = "SpecializationLeisure";
                    break;
                case ItemClass.SubService.CommercialTourist:
                    id = "DISTRICT_TITLE";
                    key = "SpecializationTourist";
                    break;
                case ItemClass.SubService.OfficeGeneric:
                    id = "ZONING_TITLE";
                    key = "Office";
                    break;
                case ItemClass.SubService.OfficeHightech:
                    id = "DISTRICT_TITLE";
                    key = "SpecializationHightech";
                    break;
                case ItemClass.SubService.CommercialEco:
                    id = "DISTRICT_TITLE";
                    key = "SpecializationOrganic";
                    break;
                case ItemClass.SubService.ResidentialLowEco:
                    id = "DISTRICT_TITLE";
                    key = "ResidentialLowEco";
                    break;
                case ItemClass.SubService.ResidentialHighEco:
                    id = "DISTRICT_TITLE";
                    key = "ResidentialHighEco";
                    break;
                default:
                    switch (subService - 38)
                    {
                        case ItemClass.SubService.None:
                            id = "ZONING_TITLE";
                            key = "ResidentialWallToWall";
                            break;
                        case ItemClass.SubService.ResidentialLow:
                            id = "ZONING_TITLE";
                            key = "CommercialWallToWall";
                            break;
                        case ItemClass.SubService.ResidentialHigh:
                            id = "ZONING_TITLE";
                            key = "OfficeWallToWall";
                            break;
                        case ItemClass.SubService.CommercialHigh:
                            id = "ZONING_TITLE";
                            key = "OfficeFinancial";
                            break;
                        default:
                            id = string.Empty;
                            key = string.Empty;
                            break;
                    }
                    break;
            }
            return id.Length > 0 ? Locale.Get(id, key) : string.Empty;
        }
    }
}
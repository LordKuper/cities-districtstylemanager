using System;
using ColossalFramework.UI;
using DistrictStyleManager.GUI;

namespace DistrictStyleManager
{
    public class BuildingHelper
    {
        private static readonly string[] Atlases =
        {
            "Thumbnails", "Thumbnails", "Thumbnails", "Thumbnails", "Thumbnails", "Thumbnails", "Thumbnails",
            "Thumbnails", "Thumbnails", "Ingame", "Ingame", "Ingame", "Ingame", "Thumbnails", "Thumbnails",
            "Thumbnails", "Thumbnails", "Thumbnails", "Thumbnails"
        };

        private static readonly string[] SpriteNames =
        {
            "ZoningResidentialLow", "ZoningResidentialHigh", "DistrictSpecializationSelfsufficient",
            "ZoningCommercialLow", "ZoningCommercialHigh", "DistrictSpecializationLeisure",
            "DistrictSpecializationTourist", "DistrictSpecializationOrganic", "ZoningIndustrial",
            "IconPolicyFarming", "IconPolicyForest", "IconPolicyOil", "IconPolicyOre", "ZoningOffice",
            "DistrictSpecializationHightech", "DistrictSpecializationResidentialWallToWall",
            "DistrictSpecializationCommercialWallToWall", "DistrictSpecializationOfficeWallToWall",
            "DistrictSpecializationOfficeFinancial"
        };

        private static readonly string[] Tooltips =
        {
            "Low density residential", "High density residential", "Eco residential", "Low density commercial",
            "High density commercial", "Leisure commercial", "Tourism commercial", "Eco commercial",
            "Generic Industry", "Farming Industry", "Forest Industry", "Oil Industry", "Ore Industry", "Office",
            "Hightech office", "Residential wall2wall", "Commercial wall2wall", "Office wall2wall",
            "Office financial"
        };

        public static UITextureAtlas GetAtlas(BuildingCategory category)
        {
            return GuiHelper.GetAtlas(Atlases[(int) category]);
        }

        public static string GetSprite(BuildingCategory category)
        {
            return SpriteNames[(int) category];
        }

        public static ItemClass.SubService[] GetSubServices(BuildingCategory category)
        {
            switch (category)
            {
                case BuildingCategory.None:
                    return new ItemClass.SubService[] { };
                case BuildingCategory.ResidentialLow:
                    return new[] {ItemClass.SubService.ResidentialLow};
                case BuildingCategory.ResidentialHigh:
                    return new[] {ItemClass.SubService.ResidentialHigh};
                case BuildingCategory.ResidentialEco:
                    return new[] {ItemClass.SubService.ResidentialLowEco, ItemClass.SubService.ResidentialHighEco};
                case BuildingCategory.CommercialLow:
                    return new[] {ItemClass.SubService.CommercialLow};
                case BuildingCategory.CommercialHigh:
                    return new[] {ItemClass.SubService.CommercialHigh};
                case BuildingCategory.CommercialLeisure:
                    return new[] {ItemClass.SubService.CommercialLeisure};
                case BuildingCategory.CommercialTourism:
                    return new[] {ItemClass.SubService.CommercialTourist};
                case BuildingCategory.CommercialEco:
                    return new[] {ItemClass.SubService.CommercialEco};
                case BuildingCategory.Industrial:
                    return new[] {ItemClass.SubService.IndustrialGeneric};
                case BuildingCategory.Farming:
                    return new[] {ItemClass.SubService.IndustrialFarming};
                case BuildingCategory.Forestry:
                    return new[] {ItemClass.SubService.IndustrialForestry};
                case BuildingCategory.Oil:
                    return new[] {ItemClass.SubService.IndustrialOil};
                case BuildingCategory.Ore:
                    return new[] {ItemClass.SubService.IndustrialOre};
                case BuildingCategory.Office:
                    return new[] {ItemClass.SubService.OfficeGeneric};
                case BuildingCategory.OfficeHightech:
                    return new[] {ItemClass.SubService.OfficeHightech};
                case BuildingCategory.ResidentialWallToWall:
                    return new[] {ItemClass.SubService.ResidentialWallToWall};
                case BuildingCategory.CommercialWallToWall:
                    return new[] {ItemClass.SubService.CommercialWallToWall};
                case BuildingCategory.OfficeWallToWall:
                    return new[] {ItemClass.SubService.OfficeWallToWall};
                case BuildingCategory.OfficeFinancial:
                    return new[] {ItemClass.SubService.OfficeFinancial};
                default:
                    throw new ArgumentOutOfRangeException(nameof(category), category, null);
            }
        }

        public static string GetTooltip(BuildingCategory category)
        {
            return Tooltips[(int) category];
        }
    }
}
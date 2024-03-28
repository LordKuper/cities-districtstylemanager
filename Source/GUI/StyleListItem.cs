using System;
using System.Linq;
using ColossalFramework.PlatformServices;
using ColossalFramework.UI;
using JetBrains.Annotations;
using UnityEngine;

namespace DistrictStyleManager.GUI
{
    [UsedImplicitly]
    public class StyleListItem : UIPanel
    {
        private const float IconLabelWidth = 25f;
        private const float IconSize = 16f;
        private UIPanel _background;
        private UIPanel _buildingInfoBar;
        private DistrictStyle _districtStyle;
        private UICheckBox _nameCheckBox;

        private static bool IsSelectedStyle(string fullName)
        {
            return DistrictStylePickerPanel.Instance.GetSelectedStyles().Contains(fullName);
        }

        public void Setup(DistrictStyle style)
        {
            _districtStyle = style;
            SetupControls();
            if (_districtStyle == null) { return; }
            _nameCheckBox.text = DistrictStyleHelper.GetDisplayName(_districtStyle);
            _nameCheckBox.label.textColor = new Color32(255, 255, 255, 255);
            _nameCheckBox.label.isInteractive = false;
            _nameCheckBox.isChecked = IsSelectedStyle(_districtStyle.FullName);
        }

        private void SetupBackgroundPanel()
        {
            if (_background != null) { return; }
            _background = AddUIComponent<UIPanel>();
            _background.width = width;
            _background.height = StyleListPanel.RowHeight;
            _background.relativePosition = Vector2.zero;
            _background.zOrder = 0;
        }

        private void SetupBuildingInfoBar()
        {
            if (_buildingInfoBar != null) { return; }
            _buildingInfoBar = _background.AddUIComponent<UIPanel>();
            _buildingInfoBar.width = width - StyleListPanel.Spacing * 2;
            _buildingInfoBar.height = StyleListPanel.BuildingInfoBarHeight;
            _buildingInfoBar.relativePosition = new Vector3(StyleListPanel.Spacing,
                _nameCheckBox.relativePosition.y + CheckBox.Height + StyleListPanel.Spacing);
            var categories = new[]
            {
                BuildingCategory.ResidentialLow, BuildingCategory.ResidentialHigh,
                BuildingCategory.ResidentialWallToWall, BuildingCategory.ResidentialEco,
                BuildingCategory.CommercialLow, BuildingCategory.CommercialHigh,
                BuildingCategory.CommercialWallToWall, BuildingCategory.CommercialLeisure,
                BuildingCategory.CommercialTourism, BuildingCategory.CommercialEco, BuildingCategory.Office,
                BuildingCategory.OfficeWallToWall, BuildingCategory.OfficeHightech,
                BuildingCategory.OfficeFinancial, BuildingCategory.Industrial, BuildingCategory.Farming,
                BuildingCategory.Forestry, BuildingCategory.Oil, BuildingCategory.Ore
            };
            if (!PlatformService.IsDlcInstalled(SteamHelper.kAfterDLCAppID))
            {
                categories = categories.Where(c =>
                        !new[] {BuildingCategory.CommercialLeisure, BuildingCategory.CommercialTourism}.Contains(c))
                    .ToArray();
            }
            if (!PlatformService.IsDlcInstalled(SteamHelper.kGreenDLCAppID))
            {
                categories = categories.Where(c =>
                    !new[]
                    {
                        BuildingCategory.ResidentialEco, BuildingCategory.CommercialEco,
                        BuildingCategory.OfficeHightech
                    }.Contains(c)).ToArray();
            }
            if (!PlatformService.IsDlcInstalled(SteamHelper.kPlazasAndPromenadesDLCAppID))
            {
                categories = categories.Where(c =>
                    !new[]
                    {
                        BuildingCategory.ResidentialWallToWall, BuildingCategory.CommercialWallToWall,
                        BuildingCategory.OfficeWallToWall
                    }.Contains(c)).ToArray();
            }
            if (!PlatformService.IsDlcInstalled(SteamHelper.kFinancialDistrictsDLCAppID))
            {
                categories = categories.Where(c => !new[] {BuildingCategory.OfficeFinancial}.Contains(c)).ToArray();
            }
            var index = 0;
            foreach (var category in categories)
            {
                var buildingCount = _districtStyle.GetBuildingInfos().Count(bi =>
                    BuildingHelper.GetSubServices(category).Contains(bi.GetSubService()));
                var row = Math.DivRem(index, 10, out var column);
                var iconPanel = _buildingInfoBar.AddUIComponent<UIPanel>();
                iconPanel.backgroundSprite = "IconPolicyBaseRect";
                iconPanel.size = new Vector2(IconSize, IconSize);
                iconPanel.relativePosition =
                    new Vector3(
                        (column + 1) * StyleListPanel.Spacing * 2 +
                        column * (IconSize + StyleListPanel.Spacing + IconLabelWidth),
                        row * (StyleListPanel.Spacing + StyleListPanel.BuildingInfoRowHeight));
                iconPanel.tooltip = BuildingHelper.GetTooltip(category);
                var icon = iconPanel.AddUIComponent<UISprite>();
                icon.atlas = BuildingHelper.GetAtlas(category);
                icon.spriteName = BuildingHelper.GetSprite(category);
                icon.size = iconPanel.size;
                icon.relativePosition = Vector3.zero;
                var label = _buildingInfoBar.AddUIComponent<UILabel>();
                label.width = IconLabelWidth;
                label.relativePosition =
                    new Vector3(iconPanel.relativePosition.x + iconPanel.width + StyleListPanel.Spacing,
                        iconPanel.relativePosition.y + 3f);
                label.textScale = 0.75f;
                label.text = $"{buildingCount:D}";
                index++;
            }
        }

        private void SetupControls()
        {
            if (_nameCheckBox != null) { return; }
            isVisible = true;
            canFocus = true;
            isInteractive = true;
            width = parent.width;
            height = StyleListPanel.RowHeight;
            SetupBackgroundPanel();
            SetupNameCheckbox();
            SetupBuildingInfoBar();
        }

        private void SetupNameCheckbox()
        {
            _nameCheckBox = CheckBox.CreateCheckBox(_background);
            _nameCheckBox.width = width - StyleListPanel.Spacing * 2;
            _nameCheckBox.clipChildren = false;
            _nameCheckBox.relativePosition = new Vector3(StyleListPanel.Spacing, StyleListPanel.Spacing * 2);
            _nameCheckBox.eventCheckChanged += (component, state) =>
            {
                if (state) { DistrictStylePickerPanel.Instance.SelectStyle(_districtStyle); }
                else { DistrictStylePickerPanel.Instance.DeselectStyle(_districtStyle); }
            };
        }
    }
}
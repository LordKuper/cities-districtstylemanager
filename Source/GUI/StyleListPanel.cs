using System;
using System.Collections.Generic;
using ColossalFramework.PlatformServices;
using ColossalFramework.UI;
using JetBrains.Annotations;
using UnityEngine;

namespace DistrictStyleManager.GUI
{
    [UsedImplicitly]
    public class StyleListPanel : UIPanel
    {
        internal const float BuildingInfoRowHeight = 20f;
        internal const float Spacing = 5f;
        private readonly List<StyleListItem> _items = new List<StyleListItem>();
        private UIScrollablePanel _styleList;

        internal static float BuildingInfoBarHeight =>
            BuildingInfoRowHeight * BuildingInfoRowCount + Spacing * (BuildingInfoRowCount - 1);

        private static int BuildingInfoRowCount => (int) Math.Ceiling((double) BuildingTypesCount / 10);

        private static ushort BuildingTypesCount
        {
            get
            {
                ushort count = 10;
                if (PlatformService.IsDlcInstalled(SteamHelper.kAfterDLCAppID))
                {
                    count += 2; // CommercialLeisure, CommercialTourism
                }
                if (PlatformService.IsDlcInstalled(SteamHelper.kGreenDLCAppID))
                {
                    count += 3; // ResidentialEco, CommercialEco, OfficeHightech
                }
                if (PlatformService.IsDlcInstalled(SteamHelper.kPlazasAndPromenadesDLCAppID))
                {
                    count += 3; // ResidentialWallToWall, CommercialWallToWall, OfficeWallToWall
                }
                if (PlatformService.IsDlcInstalled(SteamHelper.kFinancialDistrictsDLCAppID))
                {
                    count += 1; // OfficeFinancial
                }
                return count;
            }
        }

        public static StyleListPanel Instance { get; private set; }
        internal static float RowHeight => Spacing * 2 + CheckBox.Height + Spacing + BuildingInfoBarHeight;

        public override void Start()
        {
            if (Instance != null) { return; }
            base.Start();
            Instance = this;
            _styleList = GuiHelper.CreateScrollablePanel(this);
            _styleList.backgroundSprite = "UnlockingPanel";
            _styleList.relativePosition = Vector3.zero;
            _styleList.clipChildren = true;
            UpdateList();
        }

        internal void UpdateList()
        {
            foreach (var item in _items) { Destroy(item); }
            _items.Clear();
            foreach (var style in DistrictStyleHelper.StoredStyles)
            {
                var item = _styleList.AddUIComponent<StyleListItem>();
                item.Setup(style);
                _items.Add(item);
            }
        }
    }
}
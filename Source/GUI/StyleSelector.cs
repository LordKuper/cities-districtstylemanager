using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;

namespace DistrictStyleManager.GUI
{
    internal static class StyleSelector
    {
        private const string CityStyleSelectorName = "CityStyleSelector";

        internal static void AddStylePickerToDistrictPanel()
        {
            var districtInfoPanel = GameObject.Find("(Library) DistrictWorldInfoPanel")
                .GetComponent<DistrictWorldInfoPanel>();
            var policiesButton = districtInfoPanel.Find("PoliciesButton").GetComponent<UIButton>();
            var parent = policiesButton.parent;
            var stylePickerButton = Button.CreateButton(parent, 106f, 27f);
            stylePickerButton.name = "districtStylesPicker";
            stylePickerButton.text = "STYLES";
            stylePickerButton.anchor = UIAnchorStyle.Left | UIAnchorStyle.CenterVertical;
            stylePickerButton.relativePosition = new Vector3(150, 2, 0);
            stylePickerButton.eventClick += (component, clickEvent) =>
            {
                if (clickEvent.used) { return; }
                var districtId = WorldInfoPanel.GetCurrentInstanceID().District;
                var districtName = Singleton<DistrictManager>.instance.GetDistrictName(districtId);
                DistrictStylePickerPanel.Instance.Toggle(districtId,
                    districtName.IsNullOrWhiteSpace() ? districtId.ToString() : districtName);
            };
            var uiDropDown = UIView.Find<UIDropDown>("StyleDropdown");
            uiDropDown.Hide();
        }

        internal static void AddStyleSelectorToCityPanel()
        {
            if (GameObject.Find(CityStyleSelectorName) != null) { return; }
            var cityInfoPanel = GameObject.Find("(Library) CityInfoPanel").GetComponent<CityInfoPanel>();
            var policiesButton = cityInfoPanel.Find("PoliciesButton").GetComponent<UIButton>();
            var parent = policiesButton.parent;
            var stylePickerButton = Button.CreateButton(parent, 96f, 27f);
            stylePickerButton.name = "cityStylesPicker";
            stylePickerButton.text = "STYLES";
            stylePickerButton.relativePosition = new Vector3(150, 4, 0);
            stylePickerButton.eventClick += (component, clickEvent) =>
            {
                if (!clickEvent.used) { DistrictStylePickerPanel.Instance.Toggle(0, cityInfoPanel.GetCityName()); }
            };
        }
    }
}
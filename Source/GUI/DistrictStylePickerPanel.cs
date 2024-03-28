using System;
using System.Collections.Generic;
using ColossalFramework.UI;
using DistrictStyleManager.Managers;
using JetBrains.Annotations;
using UnityEngine;

namespace DistrictStyleManager.GUI
{
    [UsedImplicitly]
    public class DistrictStylePickerPanel : UIPanel
    {
        private const float BarHeight = 40f;
        private const float PanelHeight = 500f;
        private const float PanelWidth = 600f;
        private const float Spacing = 5f;
        private TitleBar _pickerTitleBar;
        private UIButton _saveButton;
        private HashSet<string> _selectedStyles = new HashSet<string>();
        private StyleListPanel _styleListPanel;
        public static DistrictStylePickerPanel Instance { get; private set; }
        private byte SelectedDistrictId { get; set; }
        private string SelectedDistrictName { get; set; }

        internal void DeselectStyle(DistrictStyle districtStyle)
        {
            _selectedStyles.Remove(districtStyle.FullName);
        }

        private void Draw()
        {
            backgroundSprite = "UnlockingPanel2";
            isVisible = false;
            canFocus = true;
            isInteractive = true;
            width = PanelWidth + Spacing * 2;
            height = BarHeight * 2 + PanelHeight + Spacing * 3;
            relativePosition = new Vector3(Mathf.Floor((UIView.GetAView().fixedWidth - width) / 2),
                Mathf.Floor((UIView.GetAView().fixedHeight - height) / 2));
            DrawTitleBar();
            DrawStyleList();
            DrawSaveButton();
        }

        private void DrawSaveButton()
        {
            _saveButton = Button.CreateButton(this, 100f, BarHeight);
            _saveButton.text = "Save";
            _saveButton.relativePosition = new Vector3(Spacing, BarHeight + Spacing + PanelHeight + Spacing);
            _saveButton.eventClick += (component, param) =>
            {
                VirtualStyleManager.SetDistrictStyles(SelectedDistrictId, _selectedStyles);
                Instance.Hide();
            };
        }

        private void DrawStyleList()
        {
            _styleListPanel = AddUIComponent<StyleListPanel>();
            _styleListPanel.width = PanelWidth;
            _styleListPanel.height = PanelHeight;
            _styleListPanel.relativePosition = new Vector3(Spacing, BarHeight + Spacing);
        }

        private void DrawTitleBar()
        {
            _pickerTitleBar = AddUIComponent(typeof(TitleBar)) as TitleBar;
            if (_pickerTitleBar != null)
            {
                _pickerTitleBar.Title = "District Style Picker";
                _pickerTitleBar.IconSprite = "ToolbarIconZoomOutCity";
            }
            else { Logger.Error("could not create title bar for district style picker"); }
        }

        internal HashSet<string> GetSelectedStyles()
        {
            return _selectedStyles;
        }

        public static void Initialize()
        {
            if (Instance != null) { return; }
            try
            {
                var view = UIView.GetAView();
                var panel = view.AddUIComponent(typeof(DistrictStylePickerPanel)) as DistrictStylePickerPanel;
                if (panel != null)
                {
                    Instance = panel;
                    panel.name = "DSM_DistrictStylePickerPanel";
                    Instance.Draw();
                }
                else { Logger.Error("could not add DistrictStylePickerPanel to view"); }
            }
            catch (Exception e) { Logger.Exception(e, "Exception when initializing DistrictStylePickerPanel"); }
        }

        internal void SelectStyle(DistrictStyle districtStyle)
        {
            _selectedStyles.Add(districtStyle.FullName);
        }

        internal void Toggle(byte districtId, string districtName)
        {
            if (isVisible)
            {
                _pickerTitleBar.Title = "District Style Picker";
                Instance.Hide();
            }
            else
            {
                Instance.Show(true);
                SelectedDistrictId = districtId;
                SelectedDistrictName = districtName;
                _pickerTitleBar.Title = $"District Style Picker ({SelectedDistrictName})";
                _selectedStyles = VirtualStyleManager.GetDistrictStyles(SelectedDistrictId);
                if (StyleListPanel.Instance != null) { StyleListPanel.Instance.UpdateList(); }
            }
        }
    }
}
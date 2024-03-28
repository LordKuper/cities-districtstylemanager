using ColossalFramework.UI;
using UnityEngine;

namespace DistrictStyleManager.GUI
{
    internal static class Button
    {
        internal static UIButton CreateButton(UIComponent parent, string name, string text, string tooltip)
        {
            var button = parent.AddUIComponent<UIButton>();
            button.name = name;
            button.text = text;
            button.tooltip = tooltip;
            button.normalBgSprite = "OptionBase";
            button.hoveredBgSprite = "OptionBaseHovered";
            button.pressedBgSprite = "OptionBasePressed";
            button.disabledBgSprite = "OptionBaseDisabled";
            return button;
        }

        internal static UIButton CreateButton(UIComponent parent, float width, float height)
        {
            var button = parent.AddUIComponent<UIButton>();
            button.size = new Vector2(width, height);
            button.normalBgSprite = "ButtonMenu";
            button.hoveredBgSprite = "ButtonMenuHovered";
            button.pressedBgSprite = "ButtonMenuPressed";
            button.disabledBgSprite = "ButtonMenuDisabled";
            button.useDropShadow = true;
            button.dropShadowOffset = new Vector2(0, -1.33f);
            button.dropShadowColor = new Color32(0, 0, 0, 0);
            button.textColor = new Color32(254, 254, 254, 255);
            button.hoveredTextColor = new Color32(7, 132, 255, 255);
            button.pressedTextColor = new Color32(30, 30, 44, 255);
            button.disabledTextColor = new Color32(46, 46, 46, 255);
            button.textVerticalAlignment = UIVerticalAlignment.Middle;
            button.textHorizontalAlignment = UIHorizontalAlignment.Center;
            button.textPadding = new RectOffset(0, 0, 4, 0);
            button.canFocus = false;
            return button;
        }
    }
}
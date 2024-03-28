using ColossalFramework.UI;
using UnityEngine;

namespace DistrictStyleManager.GUI
{
    internal static class CheckBox
    {
        private const float CheckBoxSize = 16f;
        internal const float Height = 20f;

        public static UICheckBox CreateCheckBox(UIComponent parent)
        {
            var checkBox = parent.AddUIComponent<UICheckBox>();
            checkBox.width = parent.width;
            checkBox.height = Height;
            checkBox.clipChildren = true;
            var sprite = checkBox.AddUIComponent<UISprite>();
            sprite.spriteName = "ToggleBase";
            sprite.size = new Vector2(CheckBoxSize, CheckBoxSize);
            sprite.relativePosition = new Vector3(0f, (Height - CheckBoxSize) / 2);
            checkBox.checkedBoxObject = sprite.AddUIComponent<UISprite>();
            ((UISprite) checkBox.checkedBoxObject).spriteName = "ToggleBaseFocused";
            checkBox.checkedBoxObject.size = new Vector2(CheckBoxSize, CheckBoxSize);
            checkBox.checkedBoxObject.relativePosition = Vector3.zero;
            checkBox.label = checkBox.AddUIComponent<UILabel>();
            checkBox.label.text = " ";
            checkBox.label.textScale = 1f;
            checkBox.label.relativePosition = new Vector3(CheckBoxSize + 10f, (Height - CheckBoxSize) / 2);
            return checkBox;
        }
    }
}
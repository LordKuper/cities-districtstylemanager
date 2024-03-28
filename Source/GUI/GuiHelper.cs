using System;
using ColossalFramework.UI;
using UnityEngine;

namespace DistrictStyleManager.GUI
{
    internal static class GuiHelper
    {
        private const float ScrollbarWidth = 16f;
        private static UITextureAtlas[] _atlases;

        internal static UIScrollablePanel CreateScrollablePanel(UIPanel parent)
        {
            parent.autoLayout = true;
            parent.autoLayoutDirection = LayoutDirection.Horizontal;
            var scrollablePanel = parent.AddUIComponent<UIScrollablePanel>();
            scrollablePanel.autoLayout = true;
            scrollablePanel.autoLayoutPadding = new RectOffset(10, 10, 0, 16);
            scrollablePanel.autoLayoutStart = LayoutStart.TopLeft;
            scrollablePanel.wrapLayout = true;
            scrollablePanel.size = new Vector2(parent.size.x - ScrollbarWidth, parent.size.y);
            scrollablePanel.autoLayoutDirection = LayoutDirection.Horizontal;
            var verticalScrollbar = CreateVerticalScrollbar(parent, scrollablePanel);
            verticalScrollbar.Show();
            verticalScrollbar.Invalidate();
            scrollablePanel.Invalidate();
            return scrollablePanel;
        }

        private static UIScrollbar CreateVerticalScrollbar(UIComponent panel, UIScrollablePanel scrollablePanel)
        {
            var verticalScrollbar = panel.AddUIComponent<UIScrollbar>();
            verticalScrollbar.name = "VerticalScrollbar";
            verticalScrollbar.width = ScrollbarWidth;
            verticalScrollbar.height = scrollablePanel.height;
            verticalScrollbar.orientation = UIOrientation.Vertical;
            verticalScrollbar.pivot = UIPivotPoint.TopLeft;
            verticalScrollbar.AlignTo(panel, UIAlignAnchor.TopRight);
            verticalScrollbar.minValue = 0;
            verticalScrollbar.value = 0;
            verticalScrollbar.incrementAmount = 50;
            verticalScrollbar.autoHide = true;
            var trackSprite = verticalScrollbar.AddUIComponent<UISlicedSprite>();
            trackSprite.relativePosition = Vector2.zero;
            trackSprite.autoSize = true;
            trackSprite.size = trackSprite.parent.size;
            trackSprite.fillDirection = UIFillDirection.Vertical;
            trackSprite.spriteName = "ScrollbarTrack";
            verticalScrollbar.trackObject = trackSprite;
            var thumbSprite = trackSprite.AddUIComponent<UISlicedSprite>();
            thumbSprite.relativePosition = Vector2.zero;
            thumbSprite.fillDirection = UIFillDirection.Vertical;
            thumbSprite.autoSize = true;
            thumbSprite.width = thumbSprite.parent.width;
            thumbSprite.spriteName = "ScrollbarThumb";
            verticalScrollbar.thumbObject = thumbSprite;
            verticalScrollbar.eventValueChanged += (component, value) =>
            {
                scrollablePanel.scrollPosition = new Vector2(0, value);
            };
            panel.eventMouseWheel += (component, eventParam) =>
            {
                verticalScrollbar.value -= (int) eventParam.wheelDelta * verticalScrollbar.incrementAmount;
            };
            scrollablePanel.eventMouseWheel += (component, eventParam) =>
            {
                verticalScrollbar.value -= (int) eventParam.wheelDelta * verticalScrollbar.incrementAmount;
            };
            scrollablePanel.verticalScrollbar = verticalScrollbar;
            return verticalScrollbar;
        }

        public static UITextureAtlas GetAtlas(string name)
        {
            if (_atlases == null)
            {
                _atlases = Resources.FindObjectsOfTypeAll(typeof(UITextureAtlas)) as UITextureAtlas[];
            }
            if (_atlases == null) { return UIView.GetAView().defaultAtlas; }
            foreach (var atlas in _atlases)
            {
                if (string.Equals(atlas.name, name, StringComparison.OrdinalIgnoreCase)) { return atlas; }
            }
            return UIView.GetAView().defaultAtlas;
        }

        public static void ResizeIcon(UISprite icon, Vector2 maxSize)
        {
            icon.width = icon.spriteInfo.width;
            icon.height = icon.spriteInfo.height;
            if (icon.height == 0) { return; }
            var ratio = icon.width / icon.height;
            if (icon.width > maxSize.x)
            {
                icon.width = maxSize.x;
                icon.height = maxSize.x / ratio;
            }
            if (icon.height > maxSize.y)
            {
                icon.height = maxSize.y;
                icon.width = maxSize.y * ratio;
            }
        }
    }
}
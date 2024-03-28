using ColossalFramework.UI;
using UnityEngine;

namespace DistrictStyleManager.GUI
{
    public class TitleBar : UIPanel
    {
        private UIDragHandle _drag;
        private UISprite _icon;
        private UILabel _title;
        public bool IsModal = false;
        public UIButton CloseButton { get; private set; }

        public string IconSprite
        {
            get => _icon.spriteName;
            set
            {
                if (_icon == null) { SetupControls(); }
                else
                {
                    _icon.spriteName = value;
                    if (_icon.spriteInfo == null) { return; }
                    GuiHelper.ResizeIcon(_icon, new Vector2(30, 30));
                    _icon.relativePosition = new Vector3(10, 5);
                }
            }
        }

        public string Title
        {
            get => _title.text;
            set
            {
                if (_title == null) { SetupControls(); }
                else { _title.text = value; }
            }
        }

        private void SetupControls()
        {
            width = parent.width;
            height = 40;
            isVisible = true;
            canFocus = true;
            isInteractive = true;
            relativePosition = Vector3.zero;
            _drag = AddUIComponent<UIDragHandle>();
            _drag.width = width - 50;
            _drag.height = height;
            _drag.relativePosition = Vector3.zero;
            _drag.target = parent;
            _icon = AddUIComponent<UISprite>();
            _icon.spriteName = IconSprite;
            _icon.relativePosition = new Vector3(10, 5);
            _title = AddUIComponent<UILabel>();
            _title.relativePosition = new Vector3(50, 13);
            _title.text = Title;
            CloseButton = AddUIComponent<UIButton>();
            CloseButton.relativePosition = new Vector3(width - 35, 2);
            CloseButton.normalBgSprite = "buttonclose";
            CloseButton.hoveredBgSprite = "buttonclosehover";
            CloseButton.pressedBgSprite = "buttonclosepressed";
            CloseButton.eventClick += (component, param) =>
            {
                if (IsModal) { UIView.PopModal(); }
                parent.Hide();
            };
        }
    }
}
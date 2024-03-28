using System;
using DistrictStyleManager.GUI;
using DistrictStyleManager.Managers;
using DistrictStyleManager.Patches;
using ICities;
using JetBrains.Annotations;

namespace DistrictStyleManager
{
    [UsedImplicitly]
    public class Loading : LoadingExtensionBase
    {
        private static bool _isModEnabled;
        private static bool _isModLoaded;

        public override void OnCreated(ILoading loading)
        {
            base.OnCreated(loading);
            if (loading.currentMode != AppMode.Game)
            {
                _isModEnabled = false;
                Logger.Info("not in Game mode. Skipping mod activation");
                Patcher.UnpatchAll();
                return;
            }
            if (!Patcher.Patched)
            {
                _isModEnabled = false;
                Logger.Error("Harmony patches haven't been applied. Aborting mod activation");
                return;
            }
            if (_isModEnabled) { return; }
            _isModEnabled = true;
            Logger.Info("activating mod");
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode);
            if (!Patcher.Patched) { throw new Exception("Harmony did not load properly!"); }
            if (!_isModEnabled || _isModLoaded) { return; }
            StyleSelector.AddStyleSelectorToCityPanel();
            StyleSelector.AddStylePickerToDistrictPanel();
            DistrictStylePickerPanel.Initialize();
            VirtualStyleManager.LoadData();
            Logger.Info("mod activation complete");
            _isModLoaded = true;
        }
    }
}
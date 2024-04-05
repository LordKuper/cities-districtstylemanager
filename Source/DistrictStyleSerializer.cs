using System;
using System.IO;
using ColossalFramework.IO;
using DistrictStyleManager.Managers;
using ICities;
using JetBrains.Annotations;

namespace DistrictStyleManager
{
    [UsedImplicitly]
    public class DistrictStyleSerializer : SerializableDataExtensionBase
    {
        private const uint DataFormatVersion = 1;
        private const string DataId = "DistrictStyleManager";
        private static VirtualDistrictStyleContainer[] _virtualDistrictStyles;

        internal static VirtualDistrictStyleContainer[] GetSavedData()
        {
            return _virtualDistrictStyles;
        }

        public override void OnCreated(ISerializableData serializableData)
        {
            base.OnCreated(serializableData);
            _virtualDistrictStyles = new VirtualDistrictStyleContainer[VirtualStyleManager.MaxDistrictCount];
        }

        public override void OnLoadData()
        {
            base.OnLoadData();
            try
            {
                VirtualStyleManager.InitializeVanillaStyle();
                var byteData = serializableDataManager.LoadData(DataId);
                if (byteData != null && byteData.Length > 0)
                {
                    using (var stream = new MemoryStream(byteData))
                    {
                        _virtualDistrictStyles =
                            DataSerializer.DeserializeArray<VirtualDistrictStyleContainer>(stream,
                                DataSerializer.Mode.Memory);
                    }
                }
                else { Logger.Info("no DSM styles in saved game file"); }
            }
            catch (Exception e) { Logger.Exception(e, "failed to load DistrictStyleManager"); }
        }

        public override void OnSaveData()
        {
            base.OnSaveData();
            Logger.Info("saving DSM data");
            for (var i = 0; i < _virtualDistrictStyles.Length; i++)
            {
                var data = VirtualStyleManager.GetStylesToSave((byte) i);
                if (data == null) { continue; }
                var styleContainer = new VirtualDistrictStyleContainer {StyleNames = data};
                _virtualDistrictStyles[i] = styleContainer;
            }
            using (var stream = new MemoryStream())
            {
                DataSerializer.SerializeArray(stream, DataSerializer.Mode.Memory, DataFormatVersion,
                    _virtualDistrictStyles);
                serializableDataManager.SaveData(DataId, stream.ToArray());
                Logger.Info($"saved {stream.Length}\u00A0B");
            }
        }
    }
}
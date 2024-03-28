using System.Collections.Generic;
using System.Linq;
using ColossalFramework;
using ColossalFramework.IO;

namespace DistrictStyleManager
{
    public class VirtualDistrictStyleContainer : IDataContainer
    {
        public HashSet<string> StyleNames;

        public void AfterDeserialize(DataSerializer s)
        {
            Logger.Info("validating DistrictStyleManager data");
            if (!DistrictManager.exists)
            {
                Logger.Error("District Manager does not exist");
                return;
            }
            if (StyleNames.Count == 0) { return; }
            var names = Singleton<DistrictManager>.instance.m_Styles.Where(style => StyleNames.Contains(style.FullName))
                .Select(style => style.FullName).ToArray();
            StyleNames = new HashSet<string>(names);
        }

        public void Deserialize(DataSerializer s)
        {
            Logger.Info("loading DistrictStyleManager data");
            StyleNames = new HashSet<string>(s.ReadUniqueStringArray());
        }

        public void Serialize(DataSerializer s)
        {
            Logger.Info("writing DistrictStyleManager data");
            s.WriteUniqueStringArray(StyleNames.ToArray());
        }
    }
}
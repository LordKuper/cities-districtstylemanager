using System.Reflection;
using CitiesHarmony.API;
using DistrictStyleManager.Patches;
using ICities;
using JetBrains.Annotations;

namespace DistrictStyleManager
{
    [UsedImplicitly]
    public class DistrictStyleManagerMod : IUserMod
    {
        public string Description =>
            "Allows to select multiple styles for a district and modifies building style selector logic.";

        public string Name => $"District Style Manager v.{Assembly.GetExecutingAssembly().GetName().Version}";

        public void OnDisabled()
        {
            Patcher.UnpatchAll();
        }

        public void OnEnabled()
        {
            HarmonyHelper.DoOnHarmonyReady(Patcher.PatchAll);
        }
    }
}
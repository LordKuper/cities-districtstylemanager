﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using JetBrains.Annotations;

namespace DistrictStyleManager.Patches
{
    /// <summary>
    ///     This transpiler is fixing small bug in LoadingManager.LoadCustomContent.
    ///     If a district style contains an asset, which is missing, log warn message can cause an exception
    ///     "index out of bound" if style's number of assets is lower than number of loaded styles. There is a wrong index
    ///     used when getting the missing asset's name.
    /// </summary>
    [HarmonyPatch, UsedImplicitly]
    public static class LoadingManagerLoadCustomContentTranspiler
    {
        /// <summary>
        ///     Find a correct method which we want to modify.
        /// </summary>
        [UsedImplicitly]
        private static MethodInfo TargetMethod()
        {
            return AccessTools.Method(
                typeof(LoadingManager).GetNestedType("<LoadCustomContent>c__Iterator5",
                    BindingFlags.Instance | BindingFlags.NonPublic), "MoveNext");
        }

        [UsedImplicitly]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            Logger.Info("Transpiling LoadingManager.LoadCustomContent to fix Styles loading");
            var codes = new List<CodeInstruction>(instructions);
            const string warnMessage = "Warning: Missing asset (";
            var warnMessageFound = false;
            var finished = false;
            var i = 0;
            while (i < codes.Count && !finished)
            {
                if (warnMessageFound && codes[i].opcode == OpCodes.Ldarg_0)
                {
                    Logger.Info("Transpiling LoadingManager.LoadCustomContent: Fix bugged warning message");
                    codes[i + 1].opcode = OpCodes.Ldloc_S;
                    codes[i + 1].operand = 32;
                    codes.RemoveAt(i);
                    finished = true;
                }
                if (!warnMessageFound && codes[i].opcode == OpCodes.Ldstr && warnMessage.Equals(codes[i].operand))
                {
                    Logger.Info("Transpiling LoadingManager.LoadCustomContent: We found bugged warning message");
                    warnMessageFound = true;
                }
                i++;
            }
            return codes.AsEnumerable();
        }
    }
}
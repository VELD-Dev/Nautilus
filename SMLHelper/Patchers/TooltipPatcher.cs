﻿namespace SMLHelper.V2.Patchers
{
    using Harmony;
    using QModManager;
    using SMLHelper.V2.Handlers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Text;

    internal class TooltipPatcher
    {
        internal static bool DisableEnumIsDefinedPatch = false;

        internal static void Patch(HarmonyInstance harmony)
        {
            HarmonyInstance.DEBUG = true;

            Type TooltipFactoryType = typeof(TooltipFactory);
            Type thisType = typeof(TooltipPatcher);
            MethodInfo InventoryItemMethod = AccessTools.Method(typeof(TooltipFactory), "InventoryItem");
            MethodInfo InventoryItemViewMethod = AccessTools.Method(typeof(TooltipFactory), "InventoryItemView");
            MethodInfo BuildTechMethod = AccessTools.Method(typeof(TooltipFactory), "BuildTech");
            MethodInfo RecipeMethod = AccessTools.Method(typeof(TooltipFactory), "Recipe");

            HarmonyMethod TranspilerPatch = new HarmonyMethod(SymbolExtensions.GetMethodInfo(() => Transpiler(null, null)));

            harmony.Patch(InventoryItemMethod, transpiler: TranspilerPatch);
            harmony.Patch(InventoryItemViewMethod, transpiler: TranspilerPatch);
            harmony.Patch(BuildTechMethod, transpiler: TranspilerPatch);
            harmony.Patch(RecipeMethod, transpiler: TranspilerPatch);
        }

        internal static void CustomTooltip(StringBuilder sb, TechType techType)
        {
            WriteTechType(sb, techType);

            if (IsVanillaTechType(techType))
                WriteModName(sb, "Subnautica");
            else if (TechTypePatcher.cacheManager.ContainsKey(techType))
                GetAndWriteModName(sb, techType);
            else
                WriteModNameError(sb, "Unknown Mod", "Mod added item without using SMLHelper");
        }
        internal static void CustomTooltip(StringBuilder sb, InventoryItem item)
        {
            CustomTooltip(sb, item.item.GetTechType());
        }

        internal static void WriteTechType(StringBuilder sb, TechType techType)
        {
            sb.AppendFormat("\n\n<size=19><color=#808080FF>{0} ({1})</color></size>", techType.AsString(), (int)techType);
        }
        internal static void WriteModName(StringBuilder sb, string text)
        {
            sb.AppendFormat("\n<size=23><color=#00ffffff>{0}</color></size>", text);
        }
        internal static void WriteModNameError(StringBuilder sb, string text, string reason)
        {
            sb.AppendFormat("\n<size=23><color=#ff0000ff>{0}</color></size><size=17><color=#808080FF>({1})</color></size>", text, reason);
        }

        internal static void GetAndWriteModName(StringBuilder sb, TechType type)
        {
            // if (MissingTechTypes.Contains(type)) WriteModNameError(sb, "Mod Missing");
            // This is for something else I am going to do

            if (TechTypeHandler.TechTypesByAssemblies.TryGetValue(type, out Assembly assembly))
            {
                // SMLHelper can access QModManager internals because of this: 
                // https://github.com/QModManager/QModManager/blob/releases/2.0.1/QModManager/Properties/AssemblyInfo.cs#L21

                string modName = null;

                foreach (QMod mod in Patcher.loadedMods)
                {
                    if (mod == null || mod.LoadedAssembly == null) continue;
                    if (mod.LoadedAssembly == assembly)
                    {
                        modName = mod.DisplayName;
                        break;
                    }
                }

                if (string.IsNullOrEmpty(modName))
                {
                    WriteModNameError(sb, "Unknown Mod", "Mod could not be determined");
                }
                else
                {
                    WriteModName(sb, modName);
                }
            }
            else
            {
                WriteModNameError(sb, "Unknown Mod", "Assembly could not be determined");
            }
        }

        internal static bool IsVanillaTechType(TechType type)
        {
            DisableEnumIsDefinedPatch = true;
            bool result = Enum.IsDefined(typeof(TechType), type);
            DisableEnumIsDefinedPatch = false;
            return result;
        }

        #region Patches

        internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase method)
        {
            // Turn the IEnumerable into a List
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);

            // Get the target op code based on what method is being patched
            // Ldloc_0 for InventoryItem and InventoryItemView, Ldarg_2 for BuildTech and Recipe
            OpCode targetCode = method.Name.Contains("InventoryItem") ? OpCodes.Ldloc_0 : OpCodes.Ldarg_2;
            // Get the last occurance of the code
            int entryIndex = codes.FindLastIndex(c => c.opcode == targetCode);

            // Get the fnction that will be injected based on what method is currently being patched
            // CustomTooltip(StringBuilder, InventoryItem) for InventoryItem and InventoryItemView, CustomTooltip(StringBuilder, TechType) for BuildTech and Recipe
            Expression<Action> methodInfo;
            if (method.Name.Contains("InventoryItem")) methodInfo = () => CustomTooltip(null, null);
            else methodInfo = () => CustomTooltip(null, TechType.None);

            // Remove all labels at the target op code
            List<Label> labelsToMove = codes[entryIndex].labels.ToArray().ToList();
            codes[entryIndex].labels.Clear();
            
            // Insert custom IL codes
            CodeInstruction[] codesToInsert = new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Ldloc_0)
                {
                    // Move labels correctly
                    labels = labelsToMove
                },
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, SymbolExtensions.GetMethodInfo(methodInfo))
            };
            codes.InsertRange(entryIndex, codesToInsert);

            // Return modified IL
            return codes.AsEnumerable();
        }

        #endregion
    }
}

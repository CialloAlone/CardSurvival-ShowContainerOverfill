using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.UIElements.StyleVariableResolver;

namespace ShowContainerOverfill
{
    [HarmonyPatch(typeof(InGameCardBase), nameof(InGameCardBase.GetIndexForInventory))]
    public static class CardDropLimit_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {

            //Original code:

            //// if (_CardWeight > MaxWeightCapacity - InventoryWeight(_IgnoreBonus: true))
            //IL_00fa: ldarg.s _CardWeight
            //IL_00fc: ldarg.0
            //IL_00fd: call instance float32 InGameCardBase::get_MaxWeightCapacity()
            //IL_0102: ldarg.0
            //IL_0103: ldc.i4.1
            //IL_0104: call instance float32 InGameCardBase::InventoryWeight(bool)
            //IL_0109: sub
            //IL_010a: ble.un.s IL_011f

            //Goal:
            //  Reuse the computed weight on the stack (IL_00fd).
            //  load "this" on the stack, 
            //  call GetMaxWeightLimit, which returns the adjusted weight as a float.

            MethodInfo maxWeightCapacityProperty = AccessTools.DeclaredProperty(typeof(InGameCardBase), nameof(InGameCardBase.MaxWeightCapacity))
                .GetGetMethod();

            List<CodeInstruction> instructionList = instructions.ToList();

            //foreach (var instruction in instructionList)
            //{
            //    Plugin.LogInfo($"{instruction.ToString()} - {instruction.operand?.GetType().Name}");
            //}


            MethodInfo getMaxWeightLimitMethod = AccessTools.Method(typeof(CardDropLimit_Patch), nameof(CardDropLimit_Patch.GetMaxWeightLimit));

            List<CodeInstruction> newInstructions = new CodeMatcher(instructionList)
                .MatchForward(true,
                    new CodeMatch(OpCodes.Starg_S, (byte)4),
                    new CodeMatch(OpCodes.Ldarg_S, (byte)4),
                    new CodeMatch(OpCodes.Ldarg_0),
                    new CodeMatch(OpCodes.Call, maxWeightCapacityProperty)
                    )
                .ThrowIfNotMatch("Did not find max weight capacity call.")
                .Advance(1)
                .InsertAndAdvance(
                    new CodeInstruction(OpCodes.Ldarg_0), //The in game card
                    new CodeInstruction(OpCodes.Call, getMaxWeightLimitMethod) 
                )
                .InstructionEnumeration()
                .ToList();


            //Plugin.LogInfo("--------- after");
            //foreach (var instruction in newInstructions)
            //{
            //    Plugin.LogInfo($"{instruction.ToString()} - {instruction.operand?.GetType().Name}");
            //}

            return newInstructions;
        }

        public static float GetMaxWeightLimit(float computedMax, InGameCardBase card)
        {

            if (InGameCardBase_CanReceiveInInventoryInstance_Patch.PreventOverfill == false)
            {
                return computedMax;
            }

            


            float result = card.CardModel.ContentWeightReduction == 0 ? computedMax : card.CardModel.ContentWeightReduction * -1;

            return result;
        }
    }
}

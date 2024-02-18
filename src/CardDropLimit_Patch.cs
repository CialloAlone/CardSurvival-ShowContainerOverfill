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
        public static void Postfix(ref int __result, InGameCardBase __instance, CardData _ForCard, ref float _CardWeight)
        {
            if (InGameCardBase_CanReceiveInInventoryInstance_Patch.PreventOverfill == false || __result == -1)
            {
                return;
            }

            if (__instance.CardModel.CardType is CardTypes.Blueprint or CardTypes.EnvImprovement or CardTypes.EnvDamage)
            {
                return;
            }

            if (!__instance.IsLegacyInventory && (bool) (UnityEngine.Object) _ForCard)
            {
                // 应用自定义规则计算新的最大权重限制
                float weightReduction = __instance.CardModel.ContentWeightReduction * -1;
                float maxWeight = weightReduction == 0  ? __instance.MaxWeightCapacity : weightReduction;

                if (_CardWeight > maxWeight - __instance.InventoryWeight(true))
                {
                    Plugin.LogInfo($"修改前返回值: {__result}, 最大重量: {maxWeight}, 当前重量: {__instance.InventoryWeight(true)}, 卡片重量: {_CardWeight}");
                    __result = -1;
                }
            }
        }
    }
}
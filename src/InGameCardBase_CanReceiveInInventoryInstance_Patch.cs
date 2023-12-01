using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowContainerOverfill
{
    [HarmonyPatch(typeof(InGameCardBase), nameof(InGameCardBase.CanReceiveInInventoryInstance))]
    public static class InGameCardBase_CanReceiveInInventoryInstance_Patch
    {

        public static bool PreventOverfill { get; set; }

        public static void Postfix(InGameCardBase __instance, ref bool __result, InGameCardBase _Card)
        {
            if (PreventOverfill == false) return;

            if (__result == false) return;

            CardData cardModel = __instance.CardModel;

            if(__instance.IsLegacyInventory || cardModel.CardType == CardTypes.Blueprint || cardModel.CardType == CardTypes.EnvImprovement || cardModel.CardType == CardTypes.EnvDamage)
            {
                return;
            }

            float currentWeight = __instance.InventoryWeight(true);

            __result = !IsOverBonusCapacity(__instance, _Card, currentWeight);
        }

        public static bool IsOverBonusCapacity(InGameCardBase storageCard, InGameCardBase transferringCard, float currentInventoryWeight)
        {
            return ((storageCard.CardModel.ContentWeightReduction * -1) < currentInventoryWeight + transferringCard.CurrentWeight && transferringCard.CurrentContainer != storageCard);
        }

    }
}

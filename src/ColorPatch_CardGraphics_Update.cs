using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace ShowContainerOverfill
{
    [HarmonyPatch(typeof(CardGraphics), "Update")]
    public static class ColorPatch_CardGraphics_Update
    {
        private static Color DefaultBarColor = Color.black;

        public static void Postfix(CardGraphics __instance)
        {
            //The logic that reaches the container fill bar change.
            if (!(__instance.CardLogic != null && __instance.CardLogic.CardModel != null && __instance.CardLogic.CardModel.CardType != CardTypes.Explorable
                && __instance.CardLogic.CardsInInventory != null
                && __instance.InventoryBarIndicator != null))
            {
                return;
            }

            Image image = __instance.InventoryBarIndicator.GetComponent<UnityEngine.UI.Image>();

            //Cache color
            if (DefaultBarColor == Color.black)
            {
                DefaultBarColor = image.color;
            }

            if (__instance.CardLogic.InventoryWeight(false) > 0) image.color = Plugin.OverfillColor;
            else if (InGameCardBase_CanReceiveInInventoryInstance_Patch.PreventOverfill) image.color = Plugin.OverfillLockColor;
            else image.color = DefaultBarColor;

            return;
        }

    }
}

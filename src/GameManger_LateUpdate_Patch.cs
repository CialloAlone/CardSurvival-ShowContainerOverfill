using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ShowContainerOverfill
{
    [HarmonyPatch(typeof(GameManager), "Update")]
    public static class GameManger_LateUpdate_Patch
    {
        public static void Postfix()
        {
            if(Input.GetKeyUp(Plugin.OverfillLockKey))
            {
                InGameCardBase_CanReceiveInInventoryInstance_Patch.PreventOverfill =! 
                    InGameCardBase_CanReceiveInInventoryInstance_Patch.PreventOverfill;
            }
        }
    }
}

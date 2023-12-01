using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace ShowContainerOverfill
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource Log { get; set; }
        public static ConfigEntry<KeyboardShortcut> OverfillLockKey { get; set; }

        public static ConfigEntry<bool> StartOverfillLockEnabled { get; set; }

        public static ConfigEntry<Color> OverfillLockColor { get; set; }

        public static ConfigEntry<Color> OverfillColor { get; set; }

        public static ConfigEntry<Color> DefaultBarColor { get; set; }


        private void Awake()
        {

            Log = Logger;


            ColorUtility.TryParseHtmlString("#FFBB00", out Color defaultBarColor);

            DefaultBarColor = Config.Bind<Color>("General", nameof(DefaultBarColor), defaultBarColor,
                "The default color of the bar when not over filled or locked.");


            ColorUtility.TryParseHtmlString("#CE9E00", out Color defaultLockColor);

            OverfillLockColor = Config.Bind<Color>("General", nameof(OverfillLockColor), defaultLockColor, 
                "The color of the storage bars when overfill lock is enabled.");

            OverfillColor = Config.Bind("General", nameof(OverfillColor), Color.red,
                "The color of the storage bars when a container contains more weight than the weight bonus.");

            OverfillLockKey = Config.Bind("General", nameof(OverfillLockKey), new KeyboardShortcut(KeyCode.L), 
                "Toggles limiting containers to only accept cards up to the weight bonus.  For example, A sack can contain 1000 weight, but only 600 weight is 'free'");

            StartOverfillLockEnabled = Config.Bind("General", nameof(StartOverfillLockEnabled), false, 
                "If true, the game will start with the overfill limit enabled.");

            InGameCardBase_CanReceiveInInventoryInstance_Patch.PreventOverfill = StartOverfillLockEnabled.Value;

            Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            harmony.PatchAll();

        }

        public static void LogInfo(string text)
        {
            Plugin.Log.LogInfo(text);
        }

        public static string GetGameObjectPath(GameObject obj)
        {
            GameObject searchObject = obj;

            string path = "/" + searchObject.name;
            while (searchObject.transform.parent != null)
            {
                searchObject = searchObject.transform.parent.gameObject;
                path = "/" + searchObject.name + path;
            }
            return path;
        }

    }
}
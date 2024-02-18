#if MELON_LOADER
using MelonLoader;
#else
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
#endif

using HarmonyLib;
using UnityEngine;

#if MELON_LOADER
[assembly: MelonInfo(typeof(ShowContainerOverfill.Plugin), ShowContainerOverfill.MyPluginInfo.PLUGIN_NAME, ShowContainerOverfill.MyPluginInfo.PLUGIN_VERSION, "NBKRedSpy")]
[assembly: MelonGame("WinterSpring Games", "Card Survival - Tropical Island")]
[assembly: MelonGame("WinterSpringGames", "CardSurvivalTropicalIsland")]
[assembly: MelonGame("winterspringgames", "survivaljourney")]
[assembly: MelonGame("winterspringgames", "survivaljourneydemo")]
[assembly: HarmonyDontPatchAll]
[assembly: MelonPlatformDomain(MelonPlatformDomainAttribute.CompatibleDomains.IL2CPP)]
[assembly: MelonPlatform(MelonPlatformAttribute.CompatiblePlatforms.ANDROID)] // 3 = Android
#endif


namespace ShowContainerOverfill
{
#if MELON_LOADER
    public class Plugin : MelonMod
#else
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
#endif
    {
        public static bool StartOverfillLockEnabled;
        public static Color OverfillLockColor;
        public static Color OverfillColor;
        public static Color DefaultBarColor;
        public static KeyCode OverfillLockKey;

#if MELON_LOADER
        public static MelonLogger.Instance Log;
        public override void OnInitializeMelon()
        {
            Log = LoggerInstance;
            var Config = MelonPreferences.CreateCategory("General");
            Config.SetFilePath("UserData/ShowContainerOverfill.cfg");
            
            // ColorUtility.TryParseHtmlString("#FFBB00", out Color defaultBarColor);
            var defaultBarColor = new Color32(0xFF, 0xBB, 0x00, 0xFF);

            DefaultBarColor = Config.CreateEntry(nameof(DefaultBarColor), defaultBarColor,
                "The default color of the bar when not over filled or locked.").Value;


            // ColorUtility.TryParseHtmlString("#CE9E00", out Color defaultLockColor);
            var defaultLockColor = new Color32(0xCE,0x9E,0x00, 0xFF);

            OverfillLockColor = Config.CreateEntry(nameof(OverfillLockColor), defaultLockColor,
                "The color of the storage bars when overfill lock is enabled.").Value;

            OverfillColor = Config.CreateEntry(nameof(OverfillColor), Color.red,
                "The color of the storage bars when a container contains more weight than the weight bonus.").Value;

            OverfillLockKey = Config.CreateEntry(nameof(OverfillLockKey), KeyCode.L,
                    "Toggles limiting containers to only accept cards up to the weight bonus.  For example, A sack can contain 1000 weight, but only 600 weight is 'free'")
                .Value;

            StartOverfillLockEnabled = Config.CreateEntry(nameof(StartOverfillLockEnabled), false,
                "If true, the game will start with the overfill limit enabled.").Value;

            InGameCardBase_CanReceiveInInventoryInstance_Patch.PreventOverfill = StartOverfillLockEnabled;

            HarmonyLib.Harmony.CreateAndPatchAll(typeof(CardDropLimit_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(ColorPatch_CardGraphics_Update));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(GameManger_LateUpdate_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(InGameCardBase_CanReceiveInInventoryInstance_Patch));
            Config.SaveToFile();
            LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        }
#else
        public static ManualLogSource Log;
        private void Awake()
        {
            Log = Logger;
            ColorUtility.TryParseHtmlString("#FFBB00", out Color defaultBarColor);

            DefaultBarColor = Config.Bind("General", nameof(DefaultBarColor), defaultBarColor,
                "The default color of the bar when not over filled or locked.").Value;


            ColorUtility.TryParseHtmlString("#CE9E00", out Color defaultLockColor);

            OverfillLockColor = Config.Bind("General", nameof(OverfillLockColor), defaultLockColor,
                "The color of the storage bars when overfill lock is enabled.").Value;

            OverfillColor = Config.Bind("General", nameof(OverfillColor), Color.red,
                "The color of the storage bars when a container contains more weight than the weight bonus.").Value;

            OverfillLockKey = Config.Bind("General", nameof(OverfillLockKey), KeyCode.L,
                    "Toggles limiting containers to only accept cards up to the weight bonus.  For example, A sack can contain 1000 weight, but only 600 weight is 'free'")
                .Value;

            StartOverfillLockEnabled = Config.Bind("General", nameof(StartOverfillLockEnabled), false,
                "If true, the game will start with the overfill limit enabled.").Value;

            InGameCardBase_CanReceiveInInventoryInstance_Patch.PreventOverfill = StartOverfillLockEnabled;

            Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            harmony.PatchAll();
            LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        }
#endif

        public static void LogInfo(string text)
        {
#if MELON_LOADER
            Log.Msg(text);
#else
            Log.LogInfo(text);
#endif
        }
    }
}
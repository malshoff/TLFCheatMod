//using BepInEx;
//using BepInEx.Logging;
//using BepInEx.Unity.Mono;
//using HarmonyLib;
//using UnityEngine;

//namespace MyFirstPlugin;

//[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
//public class Plugin : BaseUnityPlugin {
//    internal static new ManualLogSource Logger;

//    private void Awake() {
//        // Plugin startup logic
//        Logger = base.Logger;
//        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

//        // Initialize Harmony and patch methods
//        Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
//        harmony.PatchAll();
//        Logger.LogInfo("Patching methods...");
      
//    }
//}

////[HarmonyPatch(typeof(passive_Ragnar))]
////public static class PassiveRagnarPatch {
////    [HarmonyPatch("FixedUpdate"), HarmonyPrefix]
////    public static void Prefix(passive_Ragnar __instance) {
////        if (__instance.heroScript != null) {
////            // Ensure the hero's maxHp is always 1000
////            __instance.heroScript.maxHp = 9999;
////            __instance.heroScript.Hp = 6789;
////        }

////        foreach (GameObject list_Enemy in __instance.list_Enemies_Script.list_Enemies) {
////            list_Enemy.GetComponent<findClosestAndAttack>().Hp = 0;
////        }
////    }
////}

//[HarmonyPatch(typeof(Players_FindNearestAndAttack))]
//public static class PlayersFindNearestAndAttackPatch {
//    [HarmonyPatch("Update"), HarmonyPrefix]
//    public static void Prefix(Players_FindNearestAndAttack __instance) {
//        // Set maxHp for all instances
//        if (__instance != null) {
//            //__instance.maxHp = 9999;
//            //__instance.Hp = 9999;
//            __instance.critChance = 1;
//            __instance.lifeSteal = 20;
//            __instance.base_critChance = 1;
//            __instance.base_lifeSteal = 20;
//        }
//    }
//}
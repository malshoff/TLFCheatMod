using BepInEx;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using BepInEx.Logging;
using BepInEx.Unity.Mono;

namespace MyFirstPlugin {
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class HeroPatchWithUI : BaseUnityPlugin {
        internal static new ManualLogSource Logger;
        public static HeroPatchWithUI Instance { get; private set; }

        private Harmony harmony;
        public bool enablePatch = false; // Global toggle for the patch
        public Dictionary<Players_FindNearestAndAttack, bool> instanceStates = new Dictionary<Players_FindNearestAndAttack, bool>();
        public Dictionary<Players_FindNearestAndAttack, (int Hp, float critChance, float lifeSteal)> originalStats =
            new Dictionary<Players_FindNearestAndAttack, (int, float, float)>();

        private void Awake() {
            Instance = this;
            Logger = base.Logger;

            // Initialize Harmony
            harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            harmony.PatchAll();

            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        }

        private void OnGUI() {
            GUILayout.BeginArea(new Rect(10, 10, 300, 400), GUI.skin.box);

            GUILayout.Label("Hero Patch Controls");

            enablePatch = GUILayout.Toggle(enablePatch, "Enable Patch");

            GUILayout.Space(10);
            GUILayout.Label("Tracked Instances:");

            foreach (var kvp in instanceStates) {
                string instanceName = kvp.Key != null ? kvp.Key.name : "Destroyed Instance";
                bool state = kvp.Value;

                GUILayout.BeginHorizontal();
                GUILayout.Label(instanceName, GUILayout.Width(200));
                bool newState = GUILayout.Toggle(state, "Enable");
                GUILayout.EndHorizontal();

                if (newState != state && kvp.Key != null) {
                    instanceStates[kvp.Key] = newState;

                    // Restore original stats if toggled off
                    if (!newState) {
                        RestoreOriginalStats(kvp.Key);
                    }
                }
            }

            GUILayout.EndArea();
        }

        public void RegisterInstance(Players_FindNearestAndAttack instance) {
            if (!instanceStates.ContainsKey(instance)) {
                instanceStates.Add(instance, true);

                // Save original stats
                originalStats[instance] = (instance.Hp, instance.critChance, instance.lifeSteal);

                Logger.LogInfo($"Registered instance: {instance.name}");
                Logger.LogInfo($"Original stats: {originalStats[instance]} ");
            }
        }

        public void UnregisterInstance(Players_FindNearestAndAttack instance) {
            if (instanceStates.ContainsKey(instance)) {
                instanceStates.Remove(instance);

                // Remove from original stats
                originalStats.Remove(instance);

                Logger.LogInfo($"Unregistered instance: {instance.name}");
            }
        }

        public void RestoreOriginalStats(Players_FindNearestAndAttack instance) {
            if (originalStats.TryGetValue(instance, out var stats)) {
                // instance.Hp = stats.Hp;
                instance.critChance = stats.critChance;
                instance.lifeSteal = stats.lifeSteal;

                Logger.LogInfo($"Restored original stats for instance: {instance.name}");
            }
        }
    }

    [HarmonyPatch(typeof(Players_FindNearestAndAttack))]
    public static class PlayersFindNearestAndAttackPatch {
        internal static ManualLogSource Logger;

        [HarmonyPatch("Start"), HarmonyPostfix]
        public static void PostfixAwake(Players_FindNearestAndAttack __instance) {
            if (HeroPatchWithUI.Instance != null) {
                HeroPatchWithUI.Instance.RegisterInstance(__instance);
            }
        }

        [HarmonyPatch("OnDestroy"), HarmonyPostfix]
        public static void PostfixOnDestroy(Players_FindNearestAndAttack __instance) {
            if (HeroPatchWithUI.Instance != null) {
                HeroPatchWithUI.Instance.UnregisterInstance(__instance);
            }
        }

        [HarmonyPatch("Update"), HarmonyPrefix]
        public static void PrefixUpdate(Players_FindNearestAndAttack __instance) {
            if (HeroPatchWithUI.Instance != null &&
                HeroPatchWithUI.Instance.instanceStates.TryGetValue(__instance, out bool isEnabled)) {

                if (HeroPatchWithUI.Instance.enablePatch && isEnabled) {
                    // Apply modified stats
                    __instance.Hp = 9999;
                    __instance.critChance = 1; // 100%
                    __instance.lifeSteal = 2; // 200%
                }
                else {
                    // Restore original stats
                    HeroPatchWithUI.Instance.RestoreOriginalStats(__instance);
                }
            }
        }
    }
}

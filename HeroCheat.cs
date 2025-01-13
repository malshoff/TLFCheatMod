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
        public bool enablePatch = false; // Global toggle

        public Dictionary<Players_FindNearestAndAttack, bool> instanceStates = new Dictionary<Players_FindNearestAndAttack, bool>();

        public Dictionary<Players_FindNearestAndAttack, (int Hp, float critChance, float lifeSteal)> originalStats =
            new Dictionary<Players_FindNearestAndAttack, (int, float, float)>();

        public Dictionary<Players_FindNearestAndAttack, (float critAdded, float lifeStealAdded)> modifications =
        new Dictionary<Players_FindNearestAndAttack, (float, float)>();

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

        public void RestoreOriginalStats(Players_FindNearestAndAttack instance) {
            if (originalStats.TryGetValue(instance, out var stats) &&
                modifications.TryGetValue(instance, out var mod)) {
                instance.Hp = instance.maxHp;
                instance.critChance -= mod.critAdded;
                instance.lifeSteal -= mod.lifeStealAdded;

                modifications[instance] = (0, 0);

                Logger.LogInfo($"Restored stats for instance: {instance.name}");
            }
        }

        public void RegisterInstance(Players_FindNearestAndAttack instance) {
            if (!instanceStates.ContainsKey(instance)) {
                instanceStates.Add(instance, true);

                originalStats[instance] = (instance.Hp, instance.critChance, instance.lifeSteal);

                modifications[instance] = (0, 0);

                Logger.LogInfo($"Registered instance: {instance.name}");
            }
        }

        public void UnregisterInstance(Players_FindNearestAndAttack instance) {
            if (instanceStates.ContainsKey(instance)) {
                instanceStates.Remove(instance);
                originalStats.Remove(instance);
                modifications.Remove(instance);

                Logger.LogInfo($"Unregistered instance: {instance.name}");
            }
        }
    }

    [HarmonyPatch(typeof(Players_FindNearestAndAttack))]
    public static class PlayersFindNearestAndAttackPatch {

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
                    // Apply changes only if they haven't been applied yet
                    if (HeroPatchWithUI.Instance.modifications.TryGetValue(__instance, out var mod)) {
                        __instance.Hp = 9999;

                        if (mod.critAdded == 0 && mod.lifeStealAdded == 0) {
                            __instance.critChance += 1; // 100%
                            __instance.lifeSteal += 2;  // 200%

                            HeroPatchWithUI.Instance.modifications[__instance] = (1, 2);
                            HeroPatchWithUI.Logger.LogInfo($"Applied mod changes to instance: {__instance.name}");
                        }
                    }
                }
                else {
                    HeroPatchWithUI.Instance.RestoreOriginalStats(__instance);
                }
            }
        }

    }
}

using HarmonyLib;
using HeroCheats.Plugin;

namespace HeroCheats.Patches {
    [HarmonyPatch(typeof(Players_FindNearestAndAttack))]
    public static class PlayersFindNearestAndAttackPatch {
        [HarmonyPatch("Start"), HarmonyPostfix]
        public static void PostfixStart(Players_FindNearestAndAttack __instance) {
            InstanceManager.RegisterInstance(__instance);
        }

        [HarmonyPatch("OnDestroy"), HarmonyPostfix]
        public static void PostfixOnDestroy(Players_FindNearestAndAttack __instance) {
            InstanceManager.UnregisterInstance(__instance);
        }

    }
}


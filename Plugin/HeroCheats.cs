using BepInEx;
using HarmonyLib;
using BepInEx.Logging;
using BepInEx.Unity.Mono;

namespace HeroCheats.Plugin {
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class HeroCheat : BaseUnityPlugin {
        internal static new ManualLogSource Logger;
        public static HeroCheat Instance { get; private set; }
        private Harmony harmony;
        public bool enablePatch = false;

        private void Awake() {
            Instance = this;
            Logger = base.Logger;

            harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            harmony.PatchAll();

            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        }

        private void OnGUI() {
            UIManager.RenderUI();
        }

        private void OnDestroy() {
            harmony.UnpatchSelf();
        }
    }
}

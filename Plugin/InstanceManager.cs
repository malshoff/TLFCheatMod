using System.Collections.Generic;
using BepInEx.Logging;

namespace HeroCheats.Plugin {
    public static class InstanceManager {
        private static Dictionary<Players_FindNearestAndAttack, bool> _instances = new();

        public static void RegisterInstance(Players_FindNearestAndAttack instance) {
            if (!_instances.ContainsKey(instance)) {
                _instances.Add(instance, true);
                HeroCheat.Logger.LogInfo($"Registered instance: {instance.name}");
            }
        }

        public static void UnregisterInstance(Players_FindNearestAndAttack instance) {
            if (_instances.Remove(instance)) {
                HeroCheat.Logger.LogInfo($"Unregistered instance: {instance.name}");
            }
        }

        public static Dictionary<Players_FindNearestAndAttack, bool> GetInstances() => _instances;
    }
}

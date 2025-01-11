using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace HeroCheats.Plugin {
    public static class UIManager {
        public static void RenderUI() {
            GUILayout.BeginArea(new Rect(10, 10, 300, 400), GUI.skin.box);

            GUILayout.Label("Hero Patch Controls");

            HeroCheat.Instance.enablePatch = GUILayout.Toggle(HeroCheat.Instance.enablePatch, "Enable Patch");

            foreach (var kvp in InstanceManager.GetInstances()) {
                string instanceName = kvp.Key != null ? kvp.Key.name : "Destroyed Instance";
                bool state = kvp.Value;

                GUILayout.BeginHorizontal();
                GUILayout.Label(instanceName, GUILayout.Width(200));
                bool newState = GUILayout.Toggle(state, "Enable");
                GUILayout.EndHorizontal();

                if (newState != state && kvp.Key != null) {
                    InstanceManager.GetInstances()[kvp.Key] = newState;

                    if (newState) {
                        BonusManager.ApplyBonuses(kvp.Key);
                    }
                    else {
                        BonusManager.RevertBonuses(kvp.Key);
                    }
                }
            }

            GUILayout.EndArea();
        }
    }
}

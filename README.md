# **Example TLF Mod**

This mod for The Last Flame games allows you to dynamically adjust stats for in-game heroes. Features include a UI for enabling/disabling the stat bonuses (crit and lifesteal by defualt) globally or per hero. Feel free to reach out for requests or questions, but note that this was a quick proof of concept.

---

## **Requirements**

- **Game**: A Unity game using MonoBehaviours.
- **Modding Framework**: [BepInEx 6.x](https://github.com/BepInEx/BepInEx/releases).

---

## **Installation**

### **Step 1: Install BepInEx 6.x**

1.  Download BepInEx:
    - [BepInEx Releases Page](https://github.com/BepInEx/BepInEx/releases).
    - I used the latest [Bleeding edge build](https://builds.bepinex.dev/projects/bepinex_be)
2.  Extract the contents of the BepInEx ZIP file into the game’s root directory. This is the folder where the game’s main `.exe` file is located.
3.  Run the game once. BepInEx will initialize and create the following folder structure:

    GameRoot
    ├── BepInEx
    │ ├── plugins

### **Step 2: Install the Mod**

1.  Download the mod `.dll` file from releases, or build it locally.
2.  Place the `.dll` file in the `BepInEx/plugins/` directory:

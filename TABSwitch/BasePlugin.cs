using BepInEx;
using HarmonyLib;
using System;
using MTM101BaldAPI.Registers;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI;
using System.Collections;
using MTM101BaldAPI.OptionsAPI;
using BepInEx.Logging;
using UnityEngine;
using MTM101BaldAPI.ObjectCreation;
using System.Linq;
using System.Collections.Generic;

namespace TABSwitch
{
    public class MyPluginInfo
    {
        public const string NAME = "TAB Switch";
        public const string GUID = "rost.moment.baldiplus.tabswitch";
        public const string VERSION = "1.0";
    }
    [BepInPlugin(MyPluginInfo.GUID, MyPluginInfo.NAME, MyPluginInfo.VERSION)]
    public class BasePlugin : BaseUnityPlugin
    {

        public new static ManualLogSource Logger { get; private set; }
        public static AssetManager Asset { get; private set; }
        public static Harmony Harmony { get; private set; }
        public static BasePlugin Instance { get; private set; }

        private void Awake()
        {
            Harmony = new Harmony(MyPluginInfo.GUID);
            Harmony.PatchAll();
            Asset = new AssetManager();
            Logger = base.Logger;
            Instance = this;

        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    TABSwitcher.SwitchToPrevious();
                else
                    TABSwitcher.SwitchToNext();
            }
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                TABSwitcher.Chosen?.Click();
            }
            TABSwitcher.switchers = new HashSet<TABSwitcher>(TABSwitcher.switchers.OrderByDescending(x => x.transform.position.y).ThenBy(x => x.transform.position.x));
        }
    }
}
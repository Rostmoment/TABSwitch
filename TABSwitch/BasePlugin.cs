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

namespace DoNotGetFooled
{
    public class MyPluginInfo
    {
        public const string NAME = "Do Not Get Fooled";
        public const string GUID = "msf.rost.baldiplus.donotgetfooled";
        public const string VERSION = "1.0";
    }
    [BepInPlugin(MyPluginInfo.GUID, MyPluginInfo.NAME, MyPluginInfo.VERSION)]
    [BepInDependency("mtm101.rulerp.bbplus.baldidevapi", BepInDependency.DependencyFlags.HardDependency)]
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
            AssetLoader.LocalizationFromMod(Instance);
            LoadingEvents.RegisterOnAssetsLoaded(Info, LoadMod(), LoadingEventOrder.Pre);
            GeneratorManagement.Register(Instance, GenerationModType.Addend, (name, number, scene) =>
            {
                if (name != "F1")
                    scene.potentialNPCs.Add(new WeightedNPC() { 
                        selection = Asset.Get<KurthNPC>("KurthNPC"), 
                        weight = 250 
                    });
            });
        }
        private IEnumerator LoadMod()
        {
            yield return 1;
            yield return "Creating npc...";
            KurthNPC kurth = new NPCBuilder<KurthNPC>(Info)
                .SetName("Kurth")
                .SetMetaName("Kurth")
                .SetEnum("Kurth")
                .AddTrigger()
                .AddHeatmap()
                .AddLooker()
                .AddSpawnableRoomCategories(RoomCategory.Null)
                .SetPoster(AssetLoader.TextureFromMod(Instance, "KurthPoster.png"), "PST_Kurth_Name", "PST_Kurth_Desc")
                .Build();
            kurth.InitializeNPC();
            Asset.Add<KurthNPC>("KurthNPC", kurth);
        }

    }
}
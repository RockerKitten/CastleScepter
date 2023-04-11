using System.Reflection;
using BepInEx;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using fastJSON;

namespace CastleScepter
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    //[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class CastleScepter : BaseUnityPlugin
    {
        public const string PluginGUID = "com.RockerKitten.CastleScepter";
        public const string PluginName = "CastleScepter";
        public const string PluginVersion = "1.0.0";
        public AssetBundle assetBundle;
        public static CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();

        private void Awake()
        {
            assetBundle = AssetUtils.LoadAssetBundleFromResources("scepter", Assembly.GetExecutingAssembly());
            PrefabManager.OnVanillaPrefabsAvailable += SetupAssets;
            Jotunn.Logger.LogInfo("CastleScepter has landed");
        }

        private void SetupAssets()
        {
            InitializeBuildItConstructionTools();
            PrefabManager.OnVanillaPrefabsAvailable -= SetupAssets;
        }

        private void InitializeBuildItConstructionTools()
        {
            var hammerTableFab = assetBundle.LoadAsset<GameObject>("_RKC_CustomTable");
            var buildTable = new CustomPieceTable(hammerTableFab,
                new PieceTableConfig
                {
                    CanRemovePieces = true,
                    UseCategories = false,
                    UseCustomCategories = true,
                    CustomCategories = new string[]
                    {
                        "Structure","Clutter","Dungeons"
                    }
                });
            PieceManager.Instance.AddPieceTable(buildTable);
            var toolFab = assetBundle.LoadAsset<GameObject>("rkc_scepter");
            var tool = new CustomItem(toolFab, false,
                new ItemConfig
                {
                    Name = "$item_rkcscepter",
                    Description = "Build it castle style with the Scepter of Power!",
                    Amount = 1,
                    Enabled = true,
                    PieceTable = "_RKC_CustomTable",
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 4}
                    }
                });

            ItemManager.Instance.AddItem(tool);
        }
    }
}        

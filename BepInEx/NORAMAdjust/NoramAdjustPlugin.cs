using System.IO;
using BepInEx;
using NOLoader.ModConfig;
using NOLoader.NORAMAdjust;
using UnityEngine;

namespace NORAMAdjust.BepInEx
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public sealed class NoramAdjustPlugin : BaseUnityPlugin
    {
        public const string PluginGuid = "com.at747.noramadjust";
        public const string PluginName = "NORAM Adjust";
        public const string PluginVersion = "1.1.0";

        private const string DisplayVersion = "1.1.0 Build DEV1P3";

        private const string DefaultModIni = @"[NORAM]
enabled=1
memory_reservoir_mb=5300
chunk_mb=64
";

        private void Awake()
        {
            string pluginDir = Path.Combine(Paths.PluginPath, PluginGuid);
            Directory.CreateDirectory(pluginDir);

            ModIniConfig.EnsureDefault(pluginDir, DefaultModIni, "mod.ini");
            NoramConfig.Load(ModIniConfig.Load(pluginDir, "mod.ini"));

            if (!RamReservoir.TryApply())
                return;

            Logger.LogInfo("[NORAMAdjust] reserved=" + RamReservoir.ReservedMb + "MB (" + DisplayVersion + ")");
            Debug.Log("[NORAMAdjust] reserved=" + RamReservoir.ReservedMb + "MB (" + DisplayVersion + ")");
        }

        private void OnDestroy()
        {
            RamReservoir.Release();
        }
    }
}

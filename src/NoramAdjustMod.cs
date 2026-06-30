using NOLoader.API;
using NOLoader.ModConfig;
using UnityEngine;

namespace NOLoader.NORAMAdjust
{
    public sealed class NoramAdjustMod : INOMod
    {
        public const string ModVersion = "1.1.0 Build DEV1P3";

        private const string DefaultModIni = @"[NORAM]
enabled=1
memory_reservoir_mb=5300
chunk_mb=64
";

        public void OnLoad(ref NOModContext ctx)
        {
            ModIniConfig.EnsureDefault(ctx.ModRoot, DefaultModIni, "mod.ini");
            NoramConfig.Load(ModIniConfig.Load(ctx.ModRoot, "mod.ini"));

            if (!RamReservoir.TryApply())
                return;

            Debug.Log("[NORAMAdjust] reserved=" + RamReservoir.ReservedMb + "MB (" + ModVersion + ")");
        }

        public void OnUnload(ref NOModContext ctx)
        {
            RamReservoir.Release();
        }
    }
}

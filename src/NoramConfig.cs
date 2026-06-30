using NOLoader.ModConfig;

namespace NOLoader.NORAMAdjust
{
    internal static class NoramConfig
    {
        private const string Section = "NORAM";
        private const int DefaultReservoirMb = 5300;
        private const int DefaultChunkMb = 64;

        internal static bool Enabled { get; private set; } = true;
        internal static int MemoryReservoirMb { get; private set; } = DefaultReservoirMb;
        internal static int ChunkMb { get; private set; } = DefaultChunkMb;

        internal static void Load(ModIniConfig cfg)
        {
            Enabled = cfg.GetBool(Section, "enabled", true);
            MemoryReservoirMb = cfg.GetInt(Section, "memory_reservoir_mb", DefaultReservoirMb);
            ChunkMb = cfg.GetInt(Section, "chunk_mb", DefaultChunkMb);

            if (MemoryReservoirMb < 0)
                MemoryReservoirMb = 0;

            if (ChunkMb <= 0)
                ChunkMb = DefaultChunkMb;
        }
    }
}

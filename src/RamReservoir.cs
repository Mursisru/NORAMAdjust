using System;
using System.Collections.Generic;
using UnityEngine;

namespace NOLoader.NORAMAdjust
{
    internal static class RamReservoir
    {
        private static readonly List<byte[]> Chunks = new List<byte[]>();
        private static bool _applied;
        private static int _reservedMb;

        internal static int ReservedMb => _reservedMb;
        internal static bool IsApplied => _applied;

        internal static bool TryApply()
        {
            if (_applied || !NoramConfig.Enabled)
                return false;

            if (NoramConfig.MemoryReservoirMb <= 0)
                return false;

            try
            {
                AllocateChunks(NoramConfig.MemoryReservoirMb, NoramConfig.ChunkMb);
                _applied = true;
                return true;
            }
            catch (Exception ex)
            {
                Release();
                Debug.LogWarning("[NORAMAdjust] Reservoir allocation failed: " + ex.Message);
                return false;
            }
        }

        internal static void Release()
        {
            Chunks.Clear();
            _applied = false;
            _reservedMb = 0;
        }

        private static void AllocateChunks(int targetMb, int chunkMb)
        {
            if (Chunks.Count > 0)
                return;

            int fullChunks = targetMb / chunkMb;
            int remainderMb = targetMb % chunkMb;

            for (int i = 0; i < fullChunks; i++)
                AddChunk(chunkMb);

            if (remainderMb > 0)
                AddChunk(remainderMb);
        }

        private static void AddChunk(int sizeMb)
        {
            int sizeBytes = sizeMb * 1024 * 1024;
            if (sizeBytes <= 0)
                return;

            byte[] chunk = new byte[sizeBytes];
            chunk[0] = 1;
            chunk[chunk.Length - 1] = 2;
            Chunks.Add(chunk);
            _reservedMb += sizeMb;
        }
    }
}

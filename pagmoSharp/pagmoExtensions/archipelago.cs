
using System;
using System.Collections.Generic;

namespace pagmo
{
    public partial class archipelago
    {
        public IReadOnlyList<MigrationEntry> MigrationLog
        {
            get
            {
                var native = get_migration_log_entries();
                var list = new List<MigrationEntry>(native.Count);
                for (int i = 0; i < native.Count; ++i)
                    list.Add(native[i]);
                return list;
            }
        }

        public IReadOnlyList<IndividualsGroup> MigrantsDb
        {
            get
            {
                var native = get_migrants_db();
                var list = new List<IndividualsGroup>(native.Count);
                for (int i = 0; i < native.Count; ++i)
                    list.Add(native[i]);
                return list;
            }
        }


        //public island this[uint index] => get(index);

        //public island this[int index] => get_island(checked((uint)index));
        public ulong push_back_island(algorithm algo, problemBase prob, ulong popSize, uint seed)
    => push_back_island(algo, (problemPagomWrapper)prob, popSize, seed);
    }
}

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
                {
                    list.Add(native[i]);
                }

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
                {
                    list.Add(native[i]);
                }

                return list;
            }
        }

        public ulong push_back_island(algorithm algo, IProblem prob, ulong popSize, uint seed)
        {
            prob.throwIfNotThreadSafe();
            var problemPtr = NativeInterop.CreateProblemPointer(prob);
            try
            {
                using var wrappedProblem = new problem(problemPtr, false);
                return push_back_island(algo, wrappedProblem, checked((uint)popSize), seed);
            }
            finally
            {
                NativeInterop.problem_delete(problemPtr);
            }
        }
    }
}

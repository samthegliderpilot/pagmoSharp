using System;
using System.Collections.Generic;

namespace pagmo;

public partial class algorithm
{
    public algorithm(IAlgorithm managedAlgorithm)
        : this(CreateFromManagedAlgorithm(managedAlgorithm), true)
    {
    }

    private static IntPtr CreateFromManagedAlgorithm(IAlgorithm managedAlgorithm)
    {
        if (managedAlgorithm == null)
        {
            throw new ArgumentNullException(nameof(managedAlgorithm));
        }

        return NativeInterop.CreateAlgorithmPointer(managedAlgorithm);
    }

    public IReadOnlyList<IAlgorithmLogLine> GetLogLines()
    {
        return Array.Empty<IAlgorithmLogLine>();
    }
}

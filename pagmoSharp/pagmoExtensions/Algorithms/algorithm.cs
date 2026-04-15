using System;
using System.Collections.Generic;

namespace pagmo;

/// <summary>
/// Type-erased pagmo algorithm wrapper.
/// </summary>
/// <remarks>
/// This constructor is the managed entrypoint for custom <see cref="IAlgorithm"/> implementations
/// to participate in native pagmo orchestration (for example island/archipelago runtime flows).
/// </remarks>
public partial class algorithm
{
    /// <summary>
    /// Builds a type-erased native algorithm from a managed algorithm instance.
    /// </summary>
    /// <param name="managedAlgorithm">Managed algorithm implementation to normalize.</param>
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

    /// <summary>
    /// Returns projected algorithm log lines.
    /// </summary>
    /// <remarks>
    /// The type-erased wrapper itself does not expose typed log projections. Concrete algorithm wrappers
    /// override this where log data is available.
    /// </remarks>
    public IReadOnlyList<IAlgorithmLogLine> GetLogLines()
    {
        return Array.Empty<IAlgorithmLogLine>();
    }
}


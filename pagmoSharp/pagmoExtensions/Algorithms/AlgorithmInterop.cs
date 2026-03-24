using System;

namespace pagmo
{
    internal static class AlgorithmInterop
    {
        internal static algorithm NormalizeToTypeErased(IAlgorithm source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is algorithm typeErased)
            {
                return typeErased;
            }

            if (source is bee_colony beeColony)
            {
                return beeColony.to_algorithm();
            }

            throw new NotSupportedException(
                $"Algorithm type '{source.GetType().Name}' is not currently supported in type-erased contexts. " +
                "Pass pagmo.algorithm directly or add an explicit conversion bridge for this UDA.");
        }
    }
}

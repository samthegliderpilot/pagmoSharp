using System;

namespace pagmo
{
    /// <summary>
    /// Represents struct for SparsityIndex.
    /// </summary>
    public readonly record struct SparsityIndex(uint Row, uint Column);

    internal static class SparsityProjection
    {
        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public static SparsityIndex[] ToEntries(SparsityPattern pattern)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            var entries = new SparsityIndex[pattern.Count];
            for (var i = 0; i < pattern.Count; i++)
            {
                var pair = pattern[i];
                entries[i] = new SparsityIndex(pair.first, pair.second);
            }

            return entries;
        }

        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public static SparsityIndex[][] ToEntries(VectorOfSparsityPattern patterns)
        {
            if (patterns == null)
            {
                throw new ArgumentNullException(nameof(patterns));
            }

            var outer = new SparsityIndex[patterns.Count][];
            for (var i = 0; i < patterns.Count; i++)
            {
                var pattern = patterns[i];
                outer[i] = ToEntries(pattern);
            }

            return outer;
        }
    }

    /// <summary>
    /// Represents problem. Uses pagmo-native semantics. See docs/api-reference.md for upstream links.
    /// </summary>
    public partial class problem
    {
        /// <summary>
        /// Returns gradient sparsity entries projected to managed row/column pairs.
        /// </summary>
        public SparsityIndex[] GetGradientSparsityEntries()
        {
            if (!has_gradient_sparsity())
            {
                return Array.Empty<SparsityIndex>();
            }

            using var pattern = GradientSparsity();
            return SparsityProjection.ToEntries(pattern);
        }

        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public SparsityIndex[][] GetHessiansSparsityEntries()
        {
            if (!has_hessians_sparsity())
            {
                return Array.Empty<SparsityIndex[]>();
            }

            using var patterns = HessiansSparsity();
            return SparsityProjection.ToEntries(patterns);
        }
    }

    /// <summary>
    /// Represents managed_problem. Uses pagmo-native semantics. See docs/api-reference.md for upstream links.
    /// </summary>
    public partial class managed_problem
    {
        /// <summary>
        /// Returns gradient sparsity entries projected to managed row/column pairs.
        /// </summary>
        public SparsityIndex[] GetGradientSparsityEntries()
        {
            if (!has_gradient_sparsity())
            {
                return Array.Empty<SparsityIndex>();
            }

            var raw = gradient_sparsity();
            using var pattern = new SparsityPattern(
                SWIGTYPE_p_std__vectorT_std__pairT_size_t_size_t_t_t.getCPtr(raw).Handle,
                true);

            return SparsityProjection.ToEntries(pattern);
        }

        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public SparsityIndex[][] GetHessiansSparsityEntries()
        {
            if (!has_hessians_sparsity())
            {
                return Array.Empty<SparsityIndex[]>();
            }

            var raw = hessians_sparsity();
            using var patterns = new VectorOfSparsityPattern(
                SWIGTYPE_p_std__vectorT_std__vectorT_std__pairT_size_t_size_t_t_t_t.getCPtr(raw).Handle,
                true);

            return SparsityProjection.ToEntries(patterns);
        }
    }

    /// <summary>
    /// Represents minlp_rastrigin. Uses pagmo-native semantics. See docs/api-reference.md for upstream links.
    /// </summary>
    public partial class minlp_rastrigin
    {
        /// <summary>
        /// Returns Hessian sparsity entries projected to managed row/column pairs.
        /// </summary>
        public SparsityIndex[][] GetHessiansSparsityEntries()
        {
            var raw = hessians_sparsity();
            using var patterns = new VectorOfSparsityPattern(
                SWIGTYPE_p_std__vectorT_std__vectorT_std__pairT_size_t_size_t_t_t_t.getCPtr(raw).Handle,
                true);

            return SparsityProjection.ToEntries(patterns);
        }
    }
}


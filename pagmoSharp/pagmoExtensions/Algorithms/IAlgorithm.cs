using System;
using System.Collections.Generic;

namespace pagmo
{
    /// <summary>
    /// Common managed algorithm contract across generated and handwritten algorithm wrappers.
    /// </summary>
    /// <remarks>
    /// This maps to pagmo UDA behavior. Implementations are expected to mutate/evolve the supplied
    /// population according to algorithm-specific semantics and expose optional logging via <see cref="GetLogLines"/>.
    /// </remarks>
    public interface IAlgorithm : IDisposable
    {
        /// <summary>
        /// Evolves a population and returns the evolved population instance.
        /// </summary>
        /// <param name="pop">Population to evolve.</param>
        /// <returns>Evolved population.</returns>
        population evolve(population pop);

        /// <summary>
        /// Sets the random seed used by the algorithm where supported.
        /// </summary>
        void set_seed(uint seed);

        /// <summary>
        /// Returns the current random seed.
        /// </summary>
        uint get_seed();

        /// <summary>
        /// Returns current verbosity level.
        /// </summary>
        uint get_verbosity();

        /// <summary>
        /// Sets verbosity level.
        /// </summary>
        void set_verbosity(uint level);

        /// <summary>
        /// Returns algorithm name.
        /// </summary>
        string get_name();

        /// <summary>
        /// Returns additional algorithm details.
        /// </summary>
        string get_extra_info();

        /// <summary>
        /// Returns algorithm log lines using a child-type-agnostic shape.
        /// Algorithms that do not expose logs return an empty list.
        /// </summary>
        IReadOnlyList<IAlgorithmLogLine> GetLogLines() => Array.Empty<IAlgorithmLogLine>();
    }
}

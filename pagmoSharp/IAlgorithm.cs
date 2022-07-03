using System;

namespace pagmo
{
    /// <summary>
    /// There are difficulties to getting pagmo hpp files read in by swig.  This is to provide
    /// inheritance on the .net side of the library between all of the algorithms.  
    /// </summary>
    public interface IAlgorithm : IDisposable
    {
        /// <summary>
        /// Evolves the population.
        /// </summary>
        /// <param name="pop">The population to evolve.</param>
        /// <returns>The evolved population.</returns>
		public population evolve(population pop) ;

        /// <summary>
        /// Sets the seed value to use when evolving the population.
        /// </summary>
        /// <param name="seed">The seed value.</param>
        public void set_seed(uint seed);

        /// <summary>
        /// Sets the seed value to use when evolving the population.
        /// </summary>
        /// <returns>The seed value.</returns>
        public uint get_seed() ;

        /// <summary>
        /// Gets the verbosity value.  Defaults to 0.
        /// </summary>
        /// <returns>The verbosity value.</returns>
        public uint get_verbosity() ;

        /// <summary>
        /// Sets the verbosity value.
        /// </summary>
        /// <param name="level">The verbosity value to assign.</param>
        public void set_verbosity(uint level);
 
        /// <summary>
        /// Gets the name of the algorithm.  This is generally hardcoded.
        /// </summary>
        /// <returns>The name of the algorithm.</returns>
        public string get_name() ;

        /// <summary>
        /// If there is extra info associated with the algorithm, this gets it.
        /// </summary>
        /// <returns>Any extra info included with the algorithm.</returns>
        public string get_extra_info() ;
    }
}
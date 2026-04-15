using System;
using System.Linq;

namespace pagmo
{
    /// <summary>
    /// Simple exhaustive grid-search optimizer for educational and coarse-initialization scenarios.
    /// Samples each decision-variable range on a uniform grid and selects the best feasible point.
    /// </summary>
    public sealed class grid_search : IAlgorithm
    {
        private readonly uint[] _stepsPerDimension;
        private uint _seed;
        private uint _verbosity;

        /// <summary>
        /// Creates a grid-search algorithm with one uniform step count applied to each dimension.
        /// </summary>
        public grid_search(uint uniformStepsPerDimension)
            : this(new[] { uniformStepsPerDimension })
        {
        }

        /// <summary>
        /// Creates a grid-search algorithm with per-dimension step counts.
        /// </summary>
        public grid_search(uint[] stepsPerDimension)
        {
            if (stepsPerDimension == null || stepsPerDimension.Length == 0)
            {
                throw new ArgumentException("At least one step value must be provided.", nameof(stepsPerDimension));
            }

            if (stepsPerDimension.Any(step => step == 0))
            {
                throw new ArgumentOutOfRangeException(nameof(stepsPerDimension), "All step values must be >= 1.");
            }

            _stepsPerDimension = (uint[])stepsPerDimension.Clone();
        }

        /// <summary>
        /// Evaluates a uniform grid and updates the population with the best feasible sampled point.
        /// </summary>
        public population evolve(population pop)
        {
            if (pop == null)
            {
                throw new ArgumentNullException(nameof(pop));
            }

            if (pop.size() == 0)
            {
                throw new InvalidOperationException("grid_search requires a non-empty population.");
            }

            using var problem = pop.get_problem();
            if (problem.get_nobj() != 1)
            {
                throw new NotSupportedException("grid_search currently supports only single-objective problems.");
            }

            var nx = checked((int)problem.get_nx());
            if (nx <= 0)
            {
                throw new InvalidOperationException("Problem must have at least one decision variable.");
            }

            var steps = ResolveStepsPerDimension(nx);
            using var lb = problem.get_lb();
            using var ub = problem.get_ub();

            var gridIndex = new uint[nx];
            var candidateX = new double[nx];
            double[] bestX = null;
            double[] bestF = null;
            double bestObjective = double.PositiveInfinity;

            while (true)
            {
                for (var dim = 0; dim < nx; dim++)
                {
                    var lower = lb[dim];
                    var upper = ub[dim];
                    candidateX[dim] = lower + (upper - lower) * (gridIndex[dim] / (double)steps[dim]);
                }

                using (var x = new DoubleVector(candidateX))
                using (var f = problem.fitness(x))
                {
                    if (problem.feasibility_f(f))
                    {
                        var objective = f[0];
                        if (objective < bestObjective)
                        {
                            bestObjective = objective;
                            bestX = candidateX.ToArray();
                            bestF = new double[f.Count];
                            for (var i = 0; i < f.Count; i++)
                            {
                                bestF[i] = f[i];
                            }
                        }
                    }
                }

                var advanced = false;
                for (var dim = nx - 1; dim >= 0; dim--)
                {
                    if (gridIndex[dim] < steps[dim])
                    {
                        gridIndex[dim]++;
                        for (var reset = dim + 1; reset < nx; reset++)
                        {
                            gridIndex[reset] = 0;
                        }

                        advanced = true;
                        break;
                    }
                }

                if (!advanced)
                {
                    break;
                }
            }

            if (bestX == null || bestF == null)
            {
                throw new InvalidOperationException("grid_search did not find any feasible point on the sampled grid.");
            }

            using var bestXVector = new DoubleVector(bestX);
            using var bestFVector = new DoubleVector(bestF);
            for (uint idx = 0; idx < pop.size(); idx++)
            {
                pop.set_xf(idx, bestXVector, bestFVector);
            }

            return pop;
        }

        /// <summary>
        /// Sets the stored algorithm seed value (not used by deterministic grid enumeration).
        /// </summary>
        public void set_seed(uint seed)
        {
            _seed = seed;
        }

        /// <summary>
        /// Gets the stored algorithm seed value.
        /// </summary>
        public uint get_seed()
        {
            return _seed;
        }

        /// <summary>
        /// Gets the current verbosity level.
        /// </summary>
        public uint get_verbosity()
        {
            return _verbosity;
        }

        /// <summary>
        /// Sets the verbosity level.
        /// </summary>
        public void set_verbosity(uint level)
        {
            _verbosity = level;
        }

        /// <summary>
        /// Returns the algorithm display name.
        /// </summary>
        public string get_name()
        {
            return "C# Grid Search";
        }

        /// <summary>
        /// Returns grid configuration details.
        /// </summary>
        public string get_extra_info()
        {
            return $"Grid steps per dimension: [{string.Join(", ", _stepsPerDimension)}]";
        }

        /// <summary>
        /// Disposes the managed algorithm instance.
        /// </summary>
        public void Dispose()
        {
            // No unmanaged resources.
        }

        private uint[] ResolveStepsPerDimension(int nx)
        {
            if (_stepsPerDimension.Length == 1)
            {
                return Enumerable.Repeat(_stepsPerDimension[0], nx).ToArray();
            }

            if (_stepsPerDimension.Length != nx)
            {
                throw new InvalidOperationException(
                    $"grid_search step configuration length ({_stepsPerDimension.Length}) does not match problem dimension ({nx}).");
            }

            return (uint[])_stepsPerDimension.Clone();
        }
    }
}

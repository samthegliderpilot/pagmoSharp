using System;
using System.Runtime.ExceptionServices;
namespace pagmo
{
    /// <summary>
    /// SWIG director adapter that forwards native callbacks to a managed IProblem.
    /// </summary>
    internal sealed class ProblemCallbackAdapter : problem_callback
    {
        private readonly IProblem _problem;
        private ExceptionDispatchInfo _deferredManagedException;

        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public ProblemCallbackAdapter(IProblem problem)
        {
            _problem = problem ?? throw new ArgumentNullException(nameof(problem));
        }

        internal Exception ConsumeDeferredManagedException()
        {
            var captured = _deferredManagedException;
            _deferredManagedException = null;
            return captured == null ? null : captured.SourceException;
        }

        private void CaptureDeferredManagedException(Exception ex)
        {
            // Keep first failure to preserve causal signal if callback is retried.
            _deferredManagedException ??= ExceptionDispatchInfo.Capture(ex);
        }

        private static T RequireNonNullResult<T>(T value, string callbackName) where T : class
        {
            return value ?? throw new InvalidOperationException(
                $"Managed problem callback '{callbackName}' returned null. Callbacks must return non-null values.");
        }

        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public override DoubleVector fitness(DoubleVector x)
        {
            try
            {
                return RequireNonNullResult(_problem.fitness(x), nameof(fitness));
            }
            catch (Exception ex)
            {
                // Boundary rule:
                // Do not let managed exceptions cross the SWIG reverse-callback boundary.
                // Some native teardown paths can become unstable if exceptions escape here.
                // In this callback path we defer/rethrow through interop-owned state.
                // SWIGPendingException remains the primary channel for regular wrapper calls.
                CaptureDeferredManagedException(ex);
                return new DoubleVector();
            }
        }

        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public override DoubleVector batch_fitness(DoubleVector x)
        {
            try
            {
                return RequireNonNullResult(_problem.batch_fitness(x), nameof(batch_fitness));
            }
            catch (Exception ex)
            {
                CaptureDeferredManagedException(ex);
                return new DoubleVector();
            }
        }

        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public override PairOfDoubleVectors get_bounds() => _problem.get_bounds();
        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public override string get_name() => _problem.get_name();
        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public override string get_extra_info() => _problem.get_extra_info();
        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public override uint get_nobj() => _problem.get_nobj();
        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public override uint get_nec() => _problem.get_nec();
        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public override uint get_nic() => _problem.get_nic();
        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public override uint get_nix() => _problem.get_nix();
        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public override bool has_batch_fitness() => _problem.has_batch_fitness();
        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public override bool has_gradient() => _problem.has_gradient();
        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public override DoubleVector gradient(DoubleVector x) => _problem.gradient(x);
        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public override bool has_gradient_sparsity() => _problem.has_gradient_sparsity();
        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public override SWIGTYPE_p_std__vectorT_std__pairT_size_t_size_t_t_t gradient_sparsity()
        {
            var sparsity = _problem.gradient_sparsity();
            return new SWIGTYPE_p_std__vectorT_std__pairT_size_t_size_t_t_t(SparsityPattern.swigRelease(sparsity).Handle, true);
        }

        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public override bool has_hessians() => _problem.has_hessians();
        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public override VectorOfVectorOfDoubles hessians(DoubleVector x) => _problem.hessians(x);
        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public override bool has_hessians_sparsity() => _problem.has_hessians_sparsity();
        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public override SWIGTYPE_p_std__vectorT_std__vectorT_std__pairT_size_t_size_t_t_t_t hessians_sparsity()
        {
            var sparsity = _problem.hessians_sparsity();
            return new SWIGTYPE_p_std__vectorT_std__vectorT_std__pairT_size_t_size_t_t_t_t(VectorOfSparsityPattern.swigRelease(sparsity).Handle, true);
        }

        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public override void set_seed(uint seed) => _problem.set_seed(seed);
        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public override bool has_set_seed() => _problem.has_set_seed();
        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public override thread_safety get_thread_safety() => _problem.get_thread_safety();
    }
}


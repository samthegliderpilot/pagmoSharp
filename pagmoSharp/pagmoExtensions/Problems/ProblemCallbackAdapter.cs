using System;
using System.Runtime.ExceptionServices;
namespace pagmo
{
    /// <summary>
    /// SWIG director adapter that forwards native callbacks to a managed IProblem.
    ///
    /// All overrides catch managed exceptions and defer them through
    /// <see cref="ConsumeDeferredManagedException"/> rather than letting them escape
    /// through the SWIG reverse-callback boundary, which is undefined behaviour.
    /// Safe default values are returned on the exception path so that pagmo's native
    /// call site does not crash before the deferred exception can be rethrown.
    /// </summary>
    internal sealed class ProblemCallbackAdapter : ProblemCallback
    {
        private readonly IProblem _problem;
        private ExceptionDispatchInfo _deferredManagedException;

        public ProblemCallbackAdapter(IProblem problem)
        {
            _problem = problem ?? throw new ArgumentNullException(nameof(problem));
        }

        /// <summary>
        /// Returns and clears any managed exception that was deferred during a callback.
        /// The caller should rethrow this after returning from native code.
        /// </summary>
        internal Exception ConsumeDeferredManagedException()
        {
            var captured = _deferredManagedException;
            _deferredManagedException = null;
            return captured?.SourceException;
        }

        private void Defer(Exception ex)
        {
            // Keep the first failure; pagmo may retry the callback and we want the causal exception.
            _deferredManagedException ??= ExceptionDispatchInfo.Capture(ex);
        }

        private static T RequireNonNull<T>(T value, string callbackName) where T : class =>
            value ?? throw new InvalidOperationException(
                $"Managed problem callback '{callbackName}' returned null. Callbacks must return non-null values.");

        // -------------------------------------------------------------------------
        // Required callbacks
        // -------------------------------------------------------------------------

        public override DoubleVector fitness(DoubleVector x)
        {
            try
            {
                return RequireNonNull(_problem.fitness(x), nameof(fitness));
            }
            catch (Exception ex)
            {
                Defer(ex);
                return new DoubleVector();
            }
        }

        public override PairOfDoubleVectors get_bounds()
        {
            try
            {
                return RequireNonNull(_problem.get_bounds(), nameof(get_bounds));
            }
            catch (Exception ex)
            {
                // Return a minimal 1-dimensional [0,1] bounds so pagmo has something
                // structurally valid to work with. The deferred exception will be
                // rethrown by the caller before the bad bounds are actually used.
                Defer(ex);
                return new PairOfDoubleVectors(new DoubleVector(new[] { 0.0 }), new DoubleVector(new[] { 1.0 }));
            }
        }

        // -------------------------------------------------------------------------
        // Optional metadata
        // -------------------------------------------------------------------------

        public override string get_name()
        {
            try   { return _problem.get_name(); }
            catch (Exception ex) { Defer(ex); return "C# problem"; }
        }

        public override string get_extra_info()
        {
            try   { return _problem.get_extra_info(); }
            catch (Exception ex) { Defer(ex); return string.Empty; }
        }

        // -------------------------------------------------------------------------
        // Dimension accessors — safe default is 1 objective, 0 constraints
        // -------------------------------------------------------------------------

        public override uint get_nobj()
        {
            try   { return _problem.get_nobj(); }
            catch (Exception ex) { Defer(ex); return 1u; }
        }

        public override uint get_nec()
        {
            try   { return _problem.get_nec(); }
            catch (Exception ex) { Defer(ex); return 0u; }
        }

        public override uint get_nic()
        {
            try   { return _problem.get_nic(); }
            catch (Exception ex) { Defer(ex); return 0u; }
        }

        public override uint get_nix()
        {
            try   { return _problem.get_nix(); }
            catch (Exception ex) { Defer(ex); return 0u; }
        }

        // -------------------------------------------------------------------------
        // Optional batch evaluation
        // -------------------------------------------------------------------------

        public override bool has_batch_fitness()
        {
            try   { return _problem.has_batch_fitness(); }
            catch (Exception ex) { Defer(ex); return false; }
        }

        public override DoubleVector batch_fitness(DoubleVector x)
        {
            try
            {
                return RequireNonNull(_problem.batch_fitness(x), nameof(batch_fitness));
            }
            catch (Exception ex)
            {
                Defer(ex);
                return new DoubleVector();
            }
        }

        // -------------------------------------------------------------------------
        // Optional gradient
        // -------------------------------------------------------------------------

        public override bool has_gradient()
        {
            try   { return _problem.has_gradient(); }
            catch (Exception ex) { Defer(ex); return false; }
        }

        public override DoubleVector gradient(DoubleVector x)
        {
            try
            {
                return RequireNonNull(_problem.gradient(x), nameof(gradient));
            }
            catch (Exception ex)
            {
                Defer(ex);
                return new DoubleVector();
            }
        }

        public override bool has_gradient_sparsity()
        {
            try   { return _problem.has_gradient_sparsity(); }
            catch (Exception ex) { Defer(ex); return false; }
        }

        // Return type is SWIGTYPE because this overrides a SWIG-generated virtual
        // whose C++ signature returns pagmo::sparsity_pattern by value. SWIG cannot
        // produce a cleaner return type here; the conversion via swigRelease is correct.
        public override SWIGTYPE_p_std__vectorT_std__pairT_size_t_size_t_t_t gradient_sparsity()
        {
            try
            {
                var sparsity = _problem.gradient_sparsity();
                return new SWIGTYPE_p_std__vectorT_std__pairT_size_t_size_t_t_t(
                    SparsityPattern.swigRelease(sparsity).Handle, true);
            }
            catch (Exception ex)
            {
                Defer(ex);
                return new SWIGTYPE_p_std__vectorT_std__pairT_size_t_size_t_t_t(
                    SparsityPattern.swigRelease(new SparsityPattern()).Handle, true);
            }
        }

        // -------------------------------------------------------------------------
        // Optional hessians
        // -------------------------------------------------------------------------

        public override bool has_hessians()
        {
            try   { return _problem.has_hessians(); }
            catch (Exception ex) { Defer(ex); return false; }
        }

        public override VectorOfVectorOfDoubles hessians(DoubleVector x)
        {
            try
            {
                return RequireNonNull(_problem.hessians(x), nameof(hessians));
            }
            catch (Exception ex)
            {
                Defer(ex);
                return new VectorOfVectorOfDoubles();
            }
        }

        public override bool has_hessians_sparsity()
        {
            try   { return _problem.has_hessians_sparsity(); }
            catch (Exception ex) { Defer(ex); return false; }
        }

        // Same rationale as gradient_sparsity — SWIGTYPE is required by the override signature.
        public override SWIGTYPE_p_std__vectorT_std__vectorT_std__pairT_size_t_size_t_t_t_t hessians_sparsity()
        {
            try
            {
                var sparsity = _problem.hessians_sparsity();
                return new SWIGTYPE_p_std__vectorT_std__vectorT_std__pairT_size_t_size_t_t_t_t(
                    VectorOfSparsityPattern.swigRelease(sparsity).Handle, true);
            }
            catch (Exception ex)
            {
                Defer(ex);
                return new SWIGTYPE_p_std__vectorT_std__vectorT_std__pairT_size_t_size_t_t_t_t(
                    VectorOfSparsityPattern.swigRelease(new VectorOfSparsityPattern()).Handle, true);
            }
        }

        // -------------------------------------------------------------------------
        // Optional stochastic seed
        // -------------------------------------------------------------------------

        public override void set_seed(uint seed)
        {
            try   { _problem.set_seed(seed); }
            catch (Exception ex) { Defer(ex); }
        }

        public override bool has_set_seed()
        {
            try   { return _problem.has_set_seed(); }
            catch (Exception ex) { Defer(ex); return false; }
        }

        // -------------------------------------------------------------------------
        // Thread safety
        // -------------------------------------------------------------------------

        public override ThreadSafety get_thread_safety()
        {
            try   { return _problem.get_thread_safety(); }
            catch (Exception ex) { Defer(ex); return ThreadSafety.None; }
        }
    }
}

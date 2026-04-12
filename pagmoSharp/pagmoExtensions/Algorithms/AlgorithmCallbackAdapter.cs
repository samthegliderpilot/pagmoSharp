using System;
using System.Runtime.ExceptionServices;

namespace pagmo
{
    /// <summary>
    /// SWIG director adapter that forwards native algorithm callbacks to managed IAlgorithm.
    /// </summary>
    internal sealed class AlgorithmCallbackAdapter : algorithm_callback
    {
        private readonly IAlgorithm _algorithm;
        private ExceptionDispatchInfo _deferredManagedException;

        public AlgorithmCallbackAdapter(IAlgorithm algorithm)
        {
            _algorithm = algorithm ?? throw new ArgumentNullException(nameof(algorithm));
        }

        private void CaptureDeferredManagedException(Exception ex)
        {
            _deferredManagedException ??= ExceptionDispatchInfo.Capture(ex);
        }

        private static T RequireNonNullResult<T>(T value, string callbackName) where T : class
        {
            return value ?? throw new InvalidOperationException(
                $"Managed algorithm callback '{callbackName}' returned null. Callbacks must return non-null values.");
        }

        public override population evolve(population pop)
        {
            try
            {
                return RequireNonNullResult(_algorithm.evolve(pop), nameof(evolve));
            }
            catch (Exception ex)
            {
                // Do not let managed exceptions cross reverse-callback boundaries.
                CaptureDeferredManagedException(ex);
                return pop;
            }
        }

        public override void set_seed(uint seed)
        {
            try
            {
                _algorithm.set_seed(seed);
            }
            catch (Exception ex)
            {
                CaptureDeferredManagedException(ex);
            }
        }

        public override bool has_set_seed()
        {
            return true;
        }

        public override void set_verbosity(uint level)
        {
            try
            {
                _algorithm.set_verbosity(level);
            }
            catch (Exception ex)
            {
                CaptureDeferredManagedException(ex);
            }
        }

        public override bool has_set_verbosity()
        {
            return true;
        }

        public override string get_name() => _algorithm.get_name();
        public override string get_extra_info() => _algorithm.get_extra_info();
        public override thread_safety get_thread_safety() => thread_safety.basic;

        public override string consume_deferred_exception()
        {
            var captured = _deferredManagedException;
            _deferredManagedException = null;
            return captured?.SourceException.ToString() ?? string.Empty;
        }
    }
}

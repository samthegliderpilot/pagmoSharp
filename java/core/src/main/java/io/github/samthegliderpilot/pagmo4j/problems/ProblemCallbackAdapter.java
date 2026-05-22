package io.github.samthegliderpilot.pagmo4j.problems;

import io.github.samthegliderpilot.pagmo4j.*;
import java.lang.ref.Reference;

/**
 * SWIG director adapter that forwards native callbacks to a managed {@link IProblem}.
 *
 * <p>All overrides catch managed exceptions and defer them rather than letting them
 * escape through the JNI reverse-callback boundary, which is undefined behaviour.
 */
public final class ProblemCallbackAdapter extends ProblemCallback {

    private final IProblem problem;
    private volatile Throwable deferredEx;

    public ProblemCallbackAdapter(IProblem problem) {
        if (problem == null) throw new NullPointerException("problem");
        this.problem = problem;
    }

    public Throwable consumeDeferredException() {
        Throwable ex = deferredEx;
        deferredEx = null;
        return ex;
    }

    private void defer(Throwable ex) { if (deferredEx == null) deferredEx = ex; }

    @Override
    public DoubleVector fitness(DoubleVector x) {
        try {
            DoubleVector r = problem.fitness(x);
            if (r == null) throw new NullPointerException("fitness() returned null");
            return r;
        } catch (Throwable ex) { defer(ex); return new DoubleVector(); }
    }

    @Override
    public PairOfDoubleVectors get_bounds() {
        try {
            PairOfDoubleVectors r = problem.get_bounds();
            if (r == null) throw new NullPointerException("get_bounds() returned null");
            return r;
        } catch (Throwable ex) {
            defer(ex);
            DoubleVector z = new DoubleVector(); z.add(0.0);
            DoubleVector o = new DoubleVector(); o.add(1.0);
            return new PairOfDoubleVectors(z, o);
        }
    }

    @Override public String get_name()       { try { return problem.get_name();       } catch (Throwable ex) { defer(ex); return "Java problem"; } }
    @Override public String get_extra_info() { try { return problem.get_extra_info(); } catch (Throwable ex) { defer(ex); return ""; } }
    @Override public long get_nobj()         { try { return problem.get_nobj(); } catch (Throwable ex) { defer(ex); return 1L; } }
    @Override public long get_nec()          { try { return problem.get_nec();  } catch (Throwable ex) { defer(ex); return 0L; } }
    @Override public long get_nic()          { try { return problem.get_nic();  } catch (Throwable ex) { defer(ex); return 0L; } }
    @Override public long get_nix()          { try { return problem.get_nix();  } catch (Throwable ex) { defer(ex); return 0L; } }

    @Override public boolean has_batch_fitness()     { try { return problem.has_batch_fitness();     } catch (Throwable ex) { defer(ex); return false; } }
    @Override public boolean has_gradient()          { try { return problem.has_gradient();          } catch (Throwable ex) { defer(ex); return false; } }
    @Override public boolean has_gradient_sparsity() { try { return problem.has_gradient_sparsity(); } catch (Throwable ex) { defer(ex); return false; } }
    @Override public boolean has_hessians()          { try { return problem.has_hessians();          } catch (Throwable ex) { defer(ex); return false; } }
    @Override public boolean has_hessians_sparsity() { try { return problem.has_hessians_sparsity(); } catch (Throwable ex) { defer(ex); return false; } }
    @Override public boolean has_set_seed()          { try { return problem.has_set_seed();          } catch (Throwable ex) { defer(ex); return false; } }

    @Override
    public DoubleVector batch_fitness(DoubleVector x) {
        try { DoubleVector r = problem.batch_fitness(x); if (r == null) throw new NullPointerException(); return r; }
        catch (Throwable ex) { defer(ex); return new DoubleVector(); }
    }

    @Override
    public DoubleVector gradient(DoubleVector x) {
        try { DoubleVector r = problem.gradient(x); if (r == null) throw new NullPointerException(); return r; }
        catch (Throwable ex) { defer(ex); return new DoubleVector(); }
    }

    // gradient_sparsity() and hessians_sparsity() return opaque SWIGTYPE pointers in the
    // generated director base class.  We override them by extracting the raw C++ pointer
    // from SparsityPattern/VectorOfSparsityPattern and wrapping it in the SWIGTYPE.
    // The C++ director copies the value (c_result = *argp) before returning, so it is
    // safe for the Java wrapper to be GC'd after the call returns.
    @Override
    public SWIGTYPE_p_std__vectorT_std__pairT_size_t_size_t_t_t gradient_sparsity() {
        try {
            SparsityPattern sp = problem.gradient_sparsity();
            SWIGTYPE_p_std__vectorT_std__pairT_size_t_size_t_t_t result = NativeInterop.toSwigSparsityPattern(sp);
            // reachabilityFence keeps sp alive until C++ has copied the value (c_result = *argp)
            Reference.reachabilityFence(sp);
            return result;
        } catch (Throwable ex) {
            defer(ex);
            SparsityPattern empty = new SparsityPattern();
            SWIGTYPE_p_std__vectorT_std__pairT_size_t_size_t_t_t result = NativeInterop.toSwigSparsityPattern(empty);
            Reference.reachabilityFence(empty);
            return result;
        }
    }

    @Override
    public SWIGTYPE_p_std__vectorT_std__vectorT_std__pairT_size_t_size_t_t_t_t hessians_sparsity() {
        try {
            VectorOfSparsityPattern vsp = problem.hessians_sparsity();
            SWIGTYPE_p_std__vectorT_std__vectorT_std__pairT_size_t_size_t_t_t_t result = NativeInterop.toSwigVectorOfSparsityPattern(vsp);
            Reference.reachabilityFence(vsp);
            return result;
        } catch (Throwable ex) {
            defer(ex);
            VectorOfSparsityPattern empty = new VectorOfSparsityPattern();
            SWIGTYPE_p_std__vectorT_std__vectorT_std__pairT_size_t_size_t_t_t_t result = NativeInterop.toSwigVectorOfSparsityPattern(empty);
            Reference.reachabilityFence(empty);
            return result;
        }
    }

    @Override
    public VectorOfVectorOfDoubles hessians(DoubleVector x) {
        try { VectorOfVectorOfDoubles r = problem.hessians(x); if (r == null) throw new NullPointerException(); return r; }
        catch (Throwable ex) { defer(ex); return new VectorOfVectorOfDoubles(); }
    }

    // hessians_sparsity() — same rationale as gradient_sparsity() above.

    @Override
    public void set_seed(long seed) {
        try { problem.set_seed(seed); } catch (Throwable ex) { defer(ex); }
    }

    @Override
    public ThreadSafety get_thread_safety() {
        try { return problem.get_thread_safety(); } catch (Throwable ex) { defer(ex); return ThreadSafety.None; }
    }
}

/* PagmoNETSwigInterface.i — SWIG module for the C# / .NET pagmoSharp bindings. */

%module(naturalvar=1, directors="11") pagmo

// ── C# partial-class modifiers ────────────────────────────────────────────────
// These must be declared before %include "shared_core.i" because SWIG applies
// csclassmodifiers when it processes each type definition.
%pragma(csharp) moduleclassmodifiers = "public partial class"
%typemap(csclassmodifiers) pagmoWrap::problem_callback  "public partial class"
%typemap(csclassmodifiers) pagmoWrap::managed_problem   "public partial class"
%typemap(csclassmodifiers) pagmoWrap::algorithm_callback "public partial class"
%typemap(csclassmodifiers) pagmoWrap::managed_algorithm "public partial class"
%typemap(csclassmodifiers) std::vector<double>          "public partial class"

// ── Director exception safety typemaps ───────────────────────────────────────
// These MUST appear before all %feature("director") declarations and %include
// directives that define director classes (i.e. before shared_core.i).
//
// %typemap(csdirectorout) controls the generated SwigDirectorMethod* stubs —
// the C# functions P/Invoke calls back into.  Without try/catch, a managed
// exception propagates through the P/Invoke boundary and aborts the process.
// These typemaps wrap every director callback so exceptions are stored via
// SWIGPendingException and re-thrown as C++ exceptions on the forward path.
//
// For problem_callback and algorithm_callback, ProblemCallbackAdapter /
// AlgorithmCallbackAdapter already catch exceptions internally, so this is a
// second line of defence.  For r_policy_callback and s_policy_callback there
// is no adapter layer, so these typemaps are the primary safety net.

// void-returning director callbacks (e.g. set_seed, set_verbosity)
%typemap(csdirectorout) void %{
  try {
    $cscall;
  } catch (global::System.Exception _exSwigDirector) {
    pagmoPINVOKE.SWIGPendingException.Set(_exSwigDirector);
  }
%}

// For non-void director callbacks, SWIG wraps the typemap body as `return <body>;`
// where <body> has $cscall substituted.  Because try/catch is a statement (not an
// expression) in C#, we use an immediately-invoked lambda as the expression:
//   return ((Func<T>)(() => { try {...} catch {...} }))();

// std::string-returning director callbacks (get_name, get_extra_info)
%typemap(csdirectorout) std::string %{
  ((global::System.Func<string>)(() => {
    try { return $cscall; }
    catch (global::System.Exception _exSwigDirector) {
      pagmoPINVOKE.SWIGPendingException.Set(_exSwigDirector);
      return null;
    }
  }))()
%}

// bool-returning director callbacks (has_gradient, has_set_seed, …)
%typemap(csdirectorout) bool %{
  ((global::System.Func<bool>)(() => {
    try { return $cscall; }
    catch (global::System.Exception _exSwigDirector) {
      pagmoPINVOKE.SWIGPendingException.Set(_exSwigDirector);
      return false;
    }
  }))()
%}

// unsigned int-returning director callbacks (get_nobj, get_nec, get_nic, get_nix)
%typemap(csdirectorout) unsigned int %{
  ((global::System.Func<uint>)(() => {
    try { return $cscall; }
    catch (global::System.Exception _exSwigDirector) {
      pagmoPINVOKE.SWIGPendingException.Set(_exSwigDirector);
      return 0u;
    }
  }))()
%}

// IndividualsGroup-returning director callbacks (r_policy_callback::replace,
// s_policy_callback::select).  The underlying P/Invoke delegate returns IntPtr;
// $cscall invokes the virtual method that returns IndividualsGroup, which
// getCPtr() converts back to an IntPtr handle.
%typemap(csdirectorout) pagmoWrap::IndividualsGroup %{
  ((global::System.Func<global::System.IntPtr>)(() => {
    try { return IndividualsGroup.getCPtr($cscall).Handle; }
    catch (global::System.Exception _exSwigDirector) {
      pagmoPINVOKE.SWIGPendingException.Set(_exSwigDirector);
      return global::System.IntPtr.Zero;
    }
  }))()
%}

// ── Shared core (exception handlers, renames, directors, templates) ───────────
%include "shared_core.i"

// ── C#-correct typemaps for uint64 / unsigned long long ──────────────────────
%typemap(cstype) unsigned long long        "ulong"
%typemap(imtype) unsigned long long        "ulong"
%typemap(cstype) const unsigned long long & "ulong"
%typemap(imtype) const unsigned long long & "ulong"
%typemap(in)  unsigned long long { $1 = (unsigned long long)$input; }
%typemap(out) unsigned long long { $result = $1; }

// Map void* → IntPtr in generated C# P/Invoke signatures.
// Required for the extern-C bridge functions (managed_bridge.cpp) that return
// heap-allocated native objects as void*; without this they become SWIGTYPE_p_void.
%apply void *VOID_INT_PTR { void * }

%include "pagmoWrapper/multi_objective.h"

// ── Sub-module includes ───────────────────────────────────────────────────────
%include swigInterfaceFiles/island.i
%include swigInterfaceFiles/islands\thread_island.i
namespace pagmo {
    %typemap(csclassmodifiers) pagmo::DoubleVector         "public partial class"
    %typemap(csclassmodifiers) pagmo::VectorDoubleVector   "public partial class"
    %typemap(csclassmodifiers) pagmo::PairOfDoubleVectors  "public partial class"
    %typemap(csclassmodifiers) pagmo::ULongLongVector      "public partial class"
    typedef std::vector<double> vector_double;
    typedef std::vector<std::pair<vector_double::size_type, vector_double::size_type>> sparsity_pattern;
    typedef std::vector<std::vector<double>> VectorOfVectorOfDoubles;

    enum class thread_safety { none, basic, constant };
    enum class evolve_status {
        idle = 0, busy = 1, idle_error = 2, busy_error = 3
    };
    enum class migration_type  { p2p, broadcast };
    enum class migrant_handling { preserve, evict };

    %include swigInterfaceFiles/problem.i
    %include swigInterfaceFiles/population.i
    %include swigInterfaceFiles/bfe.i
    %include swigInterfaceFiles/io.i
    %include swigInterfaceFiles/rng.i
    %include swigInterfaceFiles/topology.i
}

%include swigInterfaceFiles/algorithms\cstrs_self_adaptive.i
%include swigInterfaceFiles/algorithms\ihs.i
%include swigInterfaceFiles/algorithms\maco.i
%include swigInterfaceFiles/algorithms\mbh.i
%include swigInterfaceFiles/algorithms\moead.i
%include swigInterfaceFiles/algorithms\moead_gen.i
%include swigInterfaceFiles/algorithms\nsga2.i
%include swigInterfaceFiles/batch_evaluators\default_bfe.i
%include swigInterfaceFiles/batch_evaluators\member_bfe.i
%include swigInterfaceFiles/batch_evaluators\thread_bfe.i
%include swigInterfaceFiles/r_policies\fair_replace.i
%include swigInterfaceFiles/s_policies\select_best.i
%include swigInterfaceFiles/topologies\unconnected.i
%include swigInterfaceFiles/topologies\fully_connected.i
%include swigInterfaceFiles/topologies\ring.i
%include swigInterfaceFiles/topologies\free_form.i
%include swigInterfaceFiles/utils\hv_algos\hv_algorithm.i
%include swigInterfaceFiles/utils\hypervolume.i

namespace pagmo {
    %include swigInterfaceFiles/algorithms\cmaes.i
    %include swigInterfaceFiles/algorithms\compass_search.i
    %include swigInterfaceFiles/algorithms\de.i
    %include swigInterfaceFiles/algorithms\de1220.i
    %include swigInterfaceFiles/algorithms\gaco.i
    %include swigInterfaceFiles/algorithms\gwo.i
    #if defined(PAGMO_WITH_IPOPT)
        %include swigInterfaceFiles/algorithms\ipopt.i
    #endif
    #if defined(PAGMO_WITH_NLOPT)
        %include swigInterfaceFiles/algorithms\nlopt.i
    #endif
    #if defined(PAGMO_WITH_SNOPT7)
        %include swigInterfaceFiles/algorithms\snopt7.i
    #endif
    %include swigInterfaceFiles/algorithms\not_population_based.i
    %include swigInterfaceFiles/algorithms\nspso.i
    %include swigInterfaceFiles/algorithms\null_algorithm.i
    %include swigInterfaceFiles/algorithms\pso.i
    %include swigInterfaceFiles/algorithms\pso_gen.i
    %include swigInterfaceFiles/algorithms\sea.i
    %include swigInterfaceFiles/algorithms\simulated_annealing.i
    %include swigInterfaceFiles/algorithms\sade.i
    %include swigInterfaceFiles/algorithms\sga.i
    %include swigInterfaceFiles/algorithms\xnes.i
}

%include swigInterfaceFiles/problems\ackley.i
%include swigInterfaceFiles/problems\cec2006.i
%include swigInterfaceFiles/problems\cec2009.i
%include swigInterfaceFiles/problems\cec2013.i
%include swigInterfaceFiles/problems\cec2014.i
%include swigInterfaceFiles/problems\decompose.i
%include swigInterfaceFiles/problems\dtlz.i
%include swigInterfaceFiles/problems\hock_schittkowski_71.i
%include swigInterfaceFiles/problems\golomb_ruler.i
%include swigInterfaceFiles/problems\griewank.i
%include swigInterfaceFiles/problems\inventory.i
%include swigInterfaceFiles/problems\lennard_jones.i
%include swigInterfaceFiles/problems\luksan_vlcek1.i
%include swigInterfaceFiles/problems\minlp_rastrigin.i
%include swigInterfaceFiles/problems\null_problem.i
%include swigInterfaceFiles/problems\rosenbrock.i
%include swigInterfaceFiles/problems\schwefel.i
%include swigInterfaceFiles/problems\rastrigin.i
%include swigInterfaceFiles/problems\translate.i
%include swigInterfaceFiles/problems\unconstrain.i
%include swigInterfaceFiles/problems\wfg.i
%include swigInterfaceFiles/problems\zdt.i

// ── to_problem() factory for native problem types ────────────────────────────
%define PAGMONET_PROBLEM_TO_PROBLEM(TYPE_NAME)
%extend pagmo::TYPE_NAME {
    pagmo::problem to_problem() const { return pagmo::problem(*self); }
}
%enddef

PAGMONET_PROBLEM_TO_PROBLEM(ackley)
PAGMONET_PROBLEM_TO_PROBLEM(cec2006)
PAGMONET_PROBLEM_TO_PROBLEM(cec2009)
PAGMONET_PROBLEM_TO_PROBLEM(cec2013)
PAGMONET_PROBLEM_TO_PROBLEM(cec2014)
PAGMONET_PROBLEM_TO_PROBLEM(decompose)
PAGMONET_PROBLEM_TO_PROBLEM(dtlz)
PAGMONET_PROBLEM_TO_PROBLEM(hock_schittkowski_71)
PAGMONET_PROBLEM_TO_PROBLEM(golomb_ruler)
PAGMONET_PROBLEM_TO_PROBLEM(griewank)
PAGMONET_PROBLEM_TO_PROBLEM(inventory)
PAGMONET_PROBLEM_TO_PROBLEM(lennard_jones)
PAGMONET_PROBLEM_TO_PROBLEM(luksan_vlcek1)
PAGMONET_PROBLEM_TO_PROBLEM(minlp_rastrigin)
PAGMONET_PROBLEM_TO_PROBLEM(null_problem)
PAGMONET_PROBLEM_TO_PROBLEM(rosenbrock)
PAGMONET_PROBLEM_TO_PROBLEM(schwefel)
PAGMONET_PROBLEM_TO_PROBLEM(rastrigin)
PAGMONET_PROBLEM_TO_PROBLEM(translate)
PAGMONET_PROBLEM_TO_PROBLEM(unconstrain)
PAGMONET_PROBLEM_TO_PROBLEM(wfg)
PAGMONET_PROBLEM_TO_PROBLEM(zdt)

%include swigInterfaceFiles/utils\multi_objective.i
%include swigInterfaceFiles/algorithm.i
%include swigInterfaceFiles/algorithms\bee_colony.i
%include swigInterfaceFiles/archipelago.i

// Remaining wrapper backlog and prioritization are tracked in .ai/ROADMAP.md.

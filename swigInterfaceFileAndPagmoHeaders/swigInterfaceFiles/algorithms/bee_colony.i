// swigInterfaceFiles/algorithms/bee_colony.i

%{
#include "pagmo/algorithm.hpp"
#include "pagmo/algorithms/bee_colony.hpp"
%}

// We need std::vector support for the wrapped log return.
%include "std_vector.i"

%typemap(csclassmodifiers) pagmo::bee_colony "public partial class"


// -----------------------------------------------------------------------------
// Local tuple adapter for bee_colony::log_type
//
// bee_colony::log_type = std::vector<std::tuple<unsigned, unsigned long long, double, double>>
// SWIG does not like vector<tuple<...>> and does not like returning const refs to these.
//
// We expose a SWIG-friendly struct and return std::vector<that_struct> instead.
// -----------------------------------------------------------------------------

%inline %{
#include <vector>
#include <tuple>

namespace pagmoWrap
{
    struct BeeColonyLogLine
    {
        unsigned gen;
        unsigned long long fevals;
        double best;
        double cur_best;

        BeeColonyLogLine() : gen(0), fevals(0), best(0.0), cur_best(0.0) {}

        BeeColonyLogLine(unsigned g, unsigned long long f, double b, double cb)
            : gen(g), fevals(f), best(b), cur_best(cb) {}
    };

    inline BeeColonyLogLine FromBeeColonyLogTuple(const std::tuple<unsigned, unsigned long long, double, double> &t)
    {
        return BeeColonyLogLine(
            std::get<0>(t),
            std::get<1>(t),
            std::get<2>(t),
            std::get<3>(t)
        );
    }
}
%}


// Make std::vector<pagmoWrap::BeeColonyLogLine> available to C#
namespace std {
    %template(BeeColonyLogLineVector) std::vector<pagmoWrap::BeeColonyLogLine>;
}

// -----------------------------------------------------------------------------
// SWIG-visible class declaration
// -----------------------------------------------------------------------------

// IMPORTANT: bee_colony does NOT inherit from pagmo::algorithm in C++ (it’s a UDA).
// Your original .i file had it inherit from pagmo::algorithm — that’s incorrect.
// The real type that wraps it into an "algorithm" is pagmo::algorithm itself.
// So we wrap bee_colony as its own class, matching the header.
class pagmo::bee_colony
{
public:
    // Constructor
    extern bee_colony(unsigned gen = 1u,
                      unsigned limit = 20u,
                      unsigned seed = pagmo::random_device::next());

    // Core API
    extern pagmo::population evolve(pagmo::population) const;

    extern void set_seed(unsigned);
    extern unsigned get_seed() const;

    extern void set_verbosity(unsigned level);
    extern unsigned get_verbosity() const;

    extern unsigned get_gen() const;

    extern std::string get_name() const;
    extern std::string get_extra_info() const;

    // We will NOT expose the original get_log(), because it returns:
    //   const std::vector<std::tuple<...>>&
    // which SWIG cannot marshal cleanly.
};


// Hide the original tuple-based log getter from SWIG entirely.
%ignore pagmo::bee_colony::get_log() const;


// -----------------------------------------------------------------------------
// SWIG-safe replacement for get_log()
// -----------------------------------------------------------------------------

%extend pagmo::bee_colony {

    std::vector<pagmoWrap::BeeColonyLogLine> get_log_lines() const
    {
        // Get the real log from pagmo
        const auto &log = self->get_log();

        std::vector<pagmoWrap::BeeColonyLogLine> out;
        out.reserve(log.size());

        for (const auto &t : log) {
            out.push_back(pagmoWrap::FromBeeColonyLogTuple(t));
        }

        return out;
    }
}

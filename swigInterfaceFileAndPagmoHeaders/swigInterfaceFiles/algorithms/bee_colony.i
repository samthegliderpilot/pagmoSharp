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

class pagmo::bee_colony
{
public:
    // Constructor
    bee_colony(unsigned gen = 1u,
               unsigned limit = 20u,
               unsigned seed = pagmo::random_device::next());

    // Core API
    pagmo::population evolve(pagmo::population) const;

    void set_seed(unsigned);
    unsigned get_seed() const;

    void set_verbosity(unsigned level);
    unsigned get_verbosity() const;

    unsigned get_gen() const;

    std::string get_name() const;
    std::string get_extra_info() const;

    // We do NOT expose get_log() directly (returns const ref to vector<tuple<...>>)
};


// Hide the original tuple-based log getter from SWIG entirely.
%ignore pagmo::bee_colony::get_log() const;


// -----------------------------------------------------------------------------
// SWIG-safe replacements / extensions
// -----------------------------------------------------------------------------

%extend pagmo::bee_colony {

    // Expose the log as a vector of struct lines.
    std::vector<pagmoWrap::BeeColonyLogLine> get_log_lines() const
    {
        const auto &log = self->get_log();

        std::vector<pagmoWrap::BeeColonyLogLine> out;
        out.reserve(log.size());

        for (const auto &t : log) {
            out.push_back(pagmoWrap::FromBeeColonyLogTuple(t));
        }

        return out;
    }

    // -------------------------------------------------------------------------
    // The key: convert this UDA (bee_colony) into a pagmo::algorithm
    //
    // In C++:
    //   pagmo::algorithm a{pagmo::bee_colony{}};
    //
    // We provide the same conversion for C#/SWIG:
    //   bees.to_algorithm()  -> returns pagmo::algorithm
    // -------------------------------------------------------------------------
    pagmo::algorithm to_algorithm() const
    {
        return pagmo::algorithm(*self);
    }
}

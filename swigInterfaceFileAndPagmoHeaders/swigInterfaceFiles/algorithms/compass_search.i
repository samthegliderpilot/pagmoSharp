%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/algorithm.hpp"
#include "pagmo/algorithms/compass_search.hpp"
%}

%typemap(csclassmodifiers) pagmo::compass_search "public partial class"

class compass_search : public pagmo::algorithm {
public:
    /// Single entry of the log (feval, best fitness, n. constraints violated, violation norm, range)
    typedef std::tuple<unsigned long long, double, vector_double::size_type, double, double> log_line_type;
    /// The log
    typedef std::vector<log_line_type> log_type;

    extern compass_search(unsigned max_fevals = 1, double start_range = .1, double stop_range = .01,
        double reduction_coeff = .5);

    // Algorithm evolve method (juice implementation of the algorithm)
    extern population evolve(population) const;
    extern void set_verbosity(unsigned level);
    extern unsigned get_verbosity() const;
    extern double get_max_fevals() const;
    extern double get_stop_range() const;
    extern double get_start_range() const;
    extern double get_reduction_coeff() const;
    extern std::string get_name() const;
    extern std::string get_extra_info() const;
    extern const log_type& get_log() const;
};


%extend compass_search{
void set_seed(unsigned) const
{
   // do nothing
} };

%extend compass_search{
unsigned get_seed() const
{
   return 0;
} };

%extend compass_search{
unsigned get_gen() const
{
   return 0;
} };
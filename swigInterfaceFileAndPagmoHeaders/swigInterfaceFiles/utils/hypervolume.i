%{
#include "pagmo/utils/hypervolume.hpp"

#include <memory>
#include <vector>

#include "pagmo/detail/visibility.hpp"
#include "pagmo/population.hpp"
#include "pagmo/s11n.hpp"
#include "pagmo/types.hpp"
#include "pagmo/utils/hv_algos/hv_algorithm.hpp"
%}

%typemap(csclassmodifiers) pagmo::hypervolume "public partial class"

class pagmo::hypervolume
{
public:
    extern hypervolume();
    extern hypervolume(const pagmo::population& pop, bool verify = false);
    extern hypervolume(const std::vector<pagmo::vector_double>& points, bool verify = true);

    extern hypervolume(const pagmo::hypervolume&);

    extern pagmo::hypervolume& operator=(const pagmo::hypervolume&);

    extern void set_copy_points(bool);

    extern bool get_copy_points() const;

    extern void set_verify(bool);

    extern bool get_verify() const;

    extern pagmo::vector_double refpoint(double offset = 0.0) const;

    extern const std::vector<pagmo::vector_double>& get_points() const;

    extern std::shared_ptr<pagmo::hv_algorithm> get_best_compute(const pagmo::vector_double&) const;
    extern std::shared_ptr<pagmo::hv_algorithm> get_best_exclusive(const unsigned, const pagmo::vector_double&) const;
    extern std::shared_ptr<pagmo::hv_algorithm> get_best_contributions(const pagmo::vector_double&) const;

    extern double compute(const pagmo::vector_double&) const;

    extern double compute(const pagmo::vector_double&, pagmo::hv_algorithm&) const;

    extern double exclusive(unsigned, const pagmo::vector_double&, pagmo::hv_algorithm&) const;

    extern double exclusive(unsigned, const pagmo::vector_double&) const;

    extern std::vector<double> contributions(const pagmo::vector_double&, pagmo::hv_algorithm&) const;

    extern std::vector<double> contributions(const pagmo::vector_double&) const;

    extern unsigned long long least_contributor(const pagmo::vector_double&, pagmo::hv_algorithm&) const;

    extern unsigned long long least_contributor(const pagmo::vector_double&) const;

    extern unsigned long long greatest_contributor(const pagmo::vector_double&, pagmo::hv_algorithm&) const;

    extern unsigned long long greatest_contributor(const pagmo::vector_double&) const;
};

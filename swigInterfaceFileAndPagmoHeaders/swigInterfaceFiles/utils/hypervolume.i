%module(naturalvar = 1, directors = "1") pagmo
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

class hypervolume
{
public:
    extern hypervolume();
    extern hypervolume(const pagmo::population& pop, bool verify = false);
    extern hypervolume(const std::vector<vector_double>& points, bool verify = true);

    extern hypervolume(const hypervolume&);

    extern hypervolume& operator=(const hypervolume&);

    extern void set_copy_points(bool);

    extern bool get_copy_points() const;

    extern void set_verify(bool);

    extern bool get_verify() const;

    extern vector_double refpoint(double offset = 0.0) const;

    extern const std::vector<vector_double>& get_points() const;

    extern std::shared_ptr<hv_algorithm> get_best_compute(const vector_double&) const;
    extern std::shared_ptr<hv_algorithm> get_best_exclusive(const unsigned, const vector_double&) const;
    extern std::shared_ptr<hv_algorithm> get_best_contributions(const vector_double&) const;

    extern double compute(const vector_double&) const;

    extern double compute(const vector_double&, hv_algorithm&) const;

    extern double exclusive(unsigned, const vector_double&, hv_algorithm&) const;

    extern double exclusive(unsigned, const vector_double&) const;

    extern std::vector<double> contributions(const vector_double&, hv_algorithm&) const;

    extern std::vector<double> contributions(const vector_double&) const;

    extern unsigned long long least_contributor(const vector_double&, hv_algorithm&) const;

    extern unsigned long long least_contributor(const vector_double&) const;

    extern unsigned long long greatest_contributor(const vector_double&, hv_algorithm&) const;

    extern unsigned long long greatest_contributor(const vector_double&) const;

//private:
    //friend class boost::serialization::access;
    //// Object serialization
    //template <typename Archive>
    //void serialize(Archive& ar, unsigned)
    //{
    //    detail::archive(ar, m_points, m_copy_points, m_verify);
    //}

    //// Verify after construct method
    //extern PAGMO_DLL_LOCAL void verify_after_construct() const;

    //// Verify before compute method
    //extern PAGMO_DLL_LOCAL void verify_before_compute(const vector_double&, hv_algorithm&) const;

    //mutable std::vector<vector_double> m_points;
    //extern bool m_copy_points;
    //extern bool m_verify;
};

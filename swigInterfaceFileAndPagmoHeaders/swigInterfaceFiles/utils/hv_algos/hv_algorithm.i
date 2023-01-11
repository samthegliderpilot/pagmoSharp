%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/utils/hv_algos/hv_algorithm.hpp"
#include "pagmo/detail/visibility.hpp"
#include "pagmo/types.hpp"
%}
%typemap(csclassmodifiers) pagmo::hv_algorithm "public partial class"

class hv_algorithm
{
public:
    extern hv_algorithm();

    //extern virtual ~hv_algorithm();

    extern hv_algorithm(const hv_algorithm&);

    //extern hv_algorithm(hv_algorithm&&) noexcept;

    static double volume_between(const vector_double& a, const vector_double& b, vector_double::size_type dim_bound = 0u);

    static double volume_between(double*, double*, vector_double::size_type);

    virtual double compute(std::vector<vector_double>& points, const vector_double& r_point) const = 0;

    virtual double exclusive(unsigned p_idx, std::vector<vector_double>&, const vector_double&) const;

    virtual unsigned long long least_contributor(std::vector<vector_double>&, const vector_double&) const;

    virtual unsigned long long greatest_contributor(std::vector<vector_double>&, const vector_double&) const;

    virtual std::vector<double> contributions(std::vector<vector_double>&, const vector_double&) const;

    virtual void verify_before_compute(const std::vector<vector_double>& points, const vector_double& r_point) const = 0;

    virtual std::shared_ptr<hv_algorithm> clone() const = 0;
    
    virtual std::string get_name() const;

protected:
    void assert_minimisation(const std::vector<vector_double>&, const vector_double&) const;

    enum {
        DOM_CMP_B_DOMINATES_A = 1, ///< second argument dominates the first one
        DOM_CMP_A_DOMINATES_B = 2, ///< first argument dominates the second one
        DOM_CMP_A_B_EQUAL = 3,     ///< both points are equal
        DOM_CMP_INCOMPARABLE = 4   ///< points are incomparable
    };

    static int dom_cmp(double*, double*, vector_double::size_type);

    static int dom_cmp(const vector_double& a, const vector_double& b, vector_double::size_type dim_bound = 0u);

//private:
//    // Compute the extreme contributor
//    PAGMO_DLL_LOCAL unsigned extreme_contributor(std::vector<vector_double>&, const vector_double&,
//        bool (*)(double, double)) const;
};


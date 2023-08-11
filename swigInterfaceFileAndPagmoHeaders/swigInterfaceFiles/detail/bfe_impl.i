%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/detail/visibility.hpp"
#include "pagmo/problem.hpp"
#include "pagmo/types.hpp"
%}

namespace pagmo
{
	namespace detail
	{
		void bfe_check_input_dvs(const problem&, const vector_double&);

		void bfe_check_output_fvs(const problem&, const vector_double&, const vector_double&);

	}; // namespace detail

}; // namespace pagmo

%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/rng.hpp"
%}

%typemap(csclassmodifiers) pagmo::rng "public partial class"
struct random_device {
public :
    static extern unsigned next();
    extern static void set_seed(unsigned seed);
};
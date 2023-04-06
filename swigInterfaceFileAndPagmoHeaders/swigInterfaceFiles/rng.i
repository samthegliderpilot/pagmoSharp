//Doesn't work, disabled in main swig file
%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/rng.hpp"
%}

%typemap(csclassmodifiers) pagmo::rng "public partial class"
class random_device {
public :
    extern static unsigned next();
    extern static void set_seed(unsigned seed);
};
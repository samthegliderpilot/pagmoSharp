%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/rng.hpp"
%}

%typemap(csclassmodifiers) pagmo::random_device "public partial class"
class random_device {
public :
    extern unsigned next();
    extern void set_seed(unsigned seed);
};
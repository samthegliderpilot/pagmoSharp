%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/problems/luksan_vlcek1.hpp"
#include "pagmo/problem.hpp"
%}

%typemap(csclassmodifiers) pagmo::luksan_vlcek1 "public partial class"
class luksan_vlcek1 {
public:
    extern luksan_vlcek1(unsigned dim = 3u);
    extern vector_double fitness(const vector_double&) const;
    extern std::pair<vector_double, vector_double> get_bounds() const;
    extern vector_double::size_type get_nec() const;
    extern vector_double gradient(const vector_double&) const;
    extern std::string get_name() const;
    extern unsigned m_dim;
};

%extend luksan_vlcek1 {
vector_double::size_type get_nic() const
{
   return 0;
} };

%extend luksan_vlcek1 {
vector_double::size_type get_nix() const
{
   return 0;
} };

%extend luksan_vlcek1 {
vector_double::size_type get_nobj() const
{
   return 1;
} };

%extend luksan_vlcek1 {
bool has_batch_fitness() const
{
    return false;
} };

%extend luksan_vlcek1 {
thread_safety get_thread_safety() const
{
    return pagmo::thread_safety::none;
} };

%{
#include "pagmo/algorithm.hpp"
#include "pagmo/algorithms/snopt7.hpp"
%}

%typemap(csclassmodifiers) pagmo::snopt7 "public partial class"
%ignore snopt7::get_log() const;
%ignore snopt7::snopt7(snopt7 &&);
%ignore snopt7::operator=;

class snopt7 {
public:
    typedef std::tuple<unsigned long long, unsigned long long, double, double, bool> log_line_type;
    typedef std::vector<log_line_type> log_type;

    extern snopt7(bool screen_output, const std::string &snopt7_c_lib, unsigned minor_version = 6u);
    extern snopt7(const snopt7 &);
    extern population evolve(population) const;
    extern std::string get_name() const;
    extern std::string get_extra_info() const;
    extern void set_verbosity(unsigned n);
    extern const log_type &get_log() const;

    extern void set_integer_option(const std::string &, int);
    extern void set_numeric_option(const std::string &, double);
    extern void set_string_option(const std::string &, const std::string &);
};

%extend snopt7 {
    std::vector<pagmoWrap::Snopt7LogEntry> get_log_entries() const
    {
        return pagmoWrap::Snopt7_GetLogEntries(*self);
    }

    pagmo::algorithm to_algorithm() const
    {
        return pagmo::algorithm(*self);
    }
}

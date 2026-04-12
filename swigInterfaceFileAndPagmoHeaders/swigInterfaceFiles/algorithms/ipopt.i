%{
#include "pagmo/algorithm.hpp"
#include "pagmo/population.hpp"
#include "pagmo/algorithms/ipopt.hpp"
%}

%typemap(csclassmodifiers) pagmo::ipopt "public partial class"
%ignore ipopt::get_log() const;
%ignore ipopt::get_last_opt_result() const;
%ignore ipopt::set_integer_option(const std::string &, Ipopt::Index);
%ignore ipopt::set_string_options(const std::map<std::string, std::string> &);
%ignore ipopt::set_integer_options(const std::map<std::string, Ipopt::Index> &);
%ignore ipopt::set_numeric_options(const std::map<std::string, double> &);
%ignore ipopt::get_string_options() const;
%ignore ipopt::get_integer_options() const;
%ignore ipopt::get_numeric_options() const;

class ipopt {
public:
    typedef std::tuple<unsigned long, double, vector_double::size_type, double, bool> log_line_type;
    typedef std::vector<log_line_type> log_type;

    extern population evolve(population) const;
    extern Ipopt::ApplicationReturnStatus get_last_opt_result() const;
    extern std::string get_name() const;
    extern std::string get_extra_info() const;
    extern void set_verbosity(unsigned n);
    extern const log_type &get_log() const;

    extern void set_string_option(const std::string &, const std::string &);
    extern void set_integer_option(const std::string &, Ipopt::Index);
    extern void set_numeric_option(const std::string &, double);
    extern void set_string_options(const std::map<std::string, std::string> &);
    extern void set_integer_options(const std::map<std::string, Ipopt::Index> &);
    extern void set_numeric_options(const std::map<std::string, double> &);
    extern std::map<std::string, std::string> get_string_options() const;
    extern std::map<std::string, Ipopt::Index> get_integer_options() const;
    extern std::map<std::string, double> get_numeric_options() const;
    extern void reset_string_options();
    extern void reset_integer_options();
    extern void reset_numeric_options();

    extern thread_safety get_thread_safety() const;
};

%extend ipopt {
    int get_last_opt_result_code() const
    {
        return static_cast<int>(self->get_last_opt_result());
    }

    void set_integer_option_u64(const std::string &name, unsigned long long value)
    {
        self->set_integer_option(name, static_cast<Ipopt::Index>(value));
    }

    std::vector<pagmoWrap::IpoptLogEntry> get_log_entries() const
    {
        return pagmoWrap::Ipopt_GetLogEntries(*self);
    }

    pagmo::algorithm to_algorithm() const
    {
        return pagmo::algorithm(*self);
    }
}

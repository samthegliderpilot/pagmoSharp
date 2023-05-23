%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/io.hpp"

#include <algorithm>
#include <initializer_list>
#include <iostream>
#include <map>
#include <sstream>
#include <stdexcept>
#include <string>
#include <utility>
#include <vector>

#include "pagmo/detail/visibility.hpp"
#include "pagmo/exceptions.hpp"
%}


#if SUPPORT_VARIDEC
template <typename... Args>
inline void stream(std::ostream&, const Args &...);
#endif



namespace detail
{

    template <typename T>
    extern inline void stream_impl(std::ostream& os, const T& x);

    extern inline void stream_impl(std::ostream& os, const bool& b);

    extern unsigned max_stream_output_length();

    // Helper to stream a [begin, end) range.
    template <typename It>
    extern inline void stream_range(std::ostream& os, It begin, It end);

    // Implementation for vector.
    template <typename T>
    extern inline void stream_impl(std::ostream& os, const std::vector<T>& v);

    template <typename T, typename U>
    extern inline void stream_impl(std::ostream& os, const std::pair<T, U>& p);

    template <typename T, typename U>
    extern inline void stream_impl(std::ostream& os, const std::map<T, U>& m);

    #if SUPPORT_VARIDEC
    template <typename T, typename... Args>
    extern inline void stream_impl(std::ostream& os, const T& x, const Args &...args);
	#endif

    // A small helper function that transforms x to string, using internally pagmo::stream.
    template <typename T>
    extern inline std::string to_string(const T& x);

    // Gizmo to create simple ascii tables.
  //  struct PAGMO_DLL_PUBLIC table {
  //      using s_size_t = std::string::size_type;

  //      extern table(std::vector<std::string> headers, std::string indent = "");

		//#if SUPPORT_VARIDEC
  //      template <typename... Args>
  //      void add_row(const Args &...args);
		//#endif
  //  };

    // Print the table to stream.
    std::ostream& operator<<(std::ostream&, const table&);

};

#if SUPPORT_VARIDEC
template <typename... Args>
inline void stream(std::ostream& os, const Args &...args);

template <typename... Args>
inline void print(const Args &...args);
#endif
//};

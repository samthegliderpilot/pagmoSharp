/* Copyright 2017-2020 PaGMO development team

This file is part of the PaGMO library.

The PaGMO library is free software; you can redistribute it and/or modify
it under the terms of either:

  * the GNU Lesser General Public License as published by the Free
    Software Foundation; either version 3 of the License, or (at your
    option) any later version.

or

  * the GNU General Public License as published by the Free Software
    Foundation; either version 3 of the License, or (at your option) any
    later version.

or both in parallel, as here.

The PaGMO library is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received copies of the GNU General Public License and the
GNU Lesser General Public License along with the PaGMO library.  If not,
see https://www.gnu.org/licenses/. */

#ifndef PAGMO_CONFIG_HPP
#define PAGMO_CONFIG_HPP

// NOTE: include this so that we can
// detect _LIBCPP_VERSION below.
#include <ciso646>

// Start of defines instantiated by CMake.
// clang-format off
#define PAGMO_VERSION "2.16.1"
#define PAGMO_VERSION_MAJOR 2
#define PAGMO_VERSION_MINOR 16
#define PAGMO_VERSION_PATCH 1
#define PAGMO_WITH_EIGEN3




// clang-format on
// End of defines instantiated by CMake.

#if defined(__clang__) && defined(_LIBCPP_VERSION)

// When using clang + libc++, prefer the name-based
// extract() implementation for UDx classes. See
// the explanation in typeid_name_extract.hpp.
#define PAGMO_PREFER_TYPEID_NAME_EXTRACT

#endif

#endif

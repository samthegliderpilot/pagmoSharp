cmake_minimum_required(VERSION 3.20)
project(PagmoWrapper VERSION 1.0.0)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# Hide all symbols by default; only PAGMONET_EXPORT-decorated functions are visible.
# This matches the Windows __declspec(dllexport) model — no unintended ABI leakage.
set(CMAKE_CXX_VISIBILITY_PRESET hidden)
set(CMAKE_VISIBILITY_INLINES_HIDDEN ON)

# When building with a static vcpkg triplet (x64-windows-static-md on Windows,
# x64-linux-static-pic on Linux), pagmo2, Boost.Serialization, and TBB are all
# static libraries. Linking them into PagmoWrapper produces a self-contained
# shared library with no pagmo/boost/tbb/nlopt/ipopt runtime dependencies —
# only the standard C++ runtime is required at runtime.
# When pagmo2 is built with IPOPT support, pagmo_export.cmake references
# pagmo::IPOPT::libipopt as a transitive link dependency. Pagmo's own config
# only pre-creates the 'header' component target; we must pre-create
# 'libipopt' so the export file can resolve it during find_package(Pagmo).
if(PAGMONET_WITH_IPOPT)
    find_library(_pagmo_ipopt_lib NAMES ipopt ipopt-3)
    if(_pagmo_ipopt_lib AND NOT TARGET pagmo::IPOPT::libipopt)
        add_library(pagmo::IPOPT::libipopt UNKNOWN IMPORTED)
        set_target_properties(pagmo::IPOPT::libipopt PROPERTIES
            IMPORTED_LOCATION "${_pagmo_ipopt_lib}")
    endif()
    unset(_pagmo_ipopt_lib)
endif()

find_package(Pagmo REQUIRED)
find_package(Boost REQUIRED COMPONENTS serialization)

# nloptGenerated.cxx is always compiled; it is empty when NLopt is not available
# (the SWIG interface guards it with #if defined(PAGMO_WITH_NLOPT)).
add_library(PagmoWrapper SHARED
    GeneratedWrappers.cxx
    managed_bridge.cpp
    nloptGenerated.cxx
)

target_include_directories(PagmoWrapper PRIVATE
    ${CMAKE_CURRENT_SOURCE_DIR}
    ${CMAKE_CURRENT_SOURCE_DIR}/../swig/pagmo
)

target_link_libraries(PagmoWrapper PRIVATE
    Pagmo::pagmo
    Boost::serialization
)

# Pagmo::pagmo exports PAGMO_WITH_NLOPT/IPOPT as INTERFACE compile definitions
# reflecting how upstream pagmo was built, not what we have available. Explicitly
# undefine them so our wrapper only enables optional-solver code when we have
# verified the solver is present (PAGMONET_WITH_* options below).
if(MSVC)
    target_compile_options(PagmoWrapper PRIVATE /UPAGMO_WITH_NLOPT /UPAGMO_WITH_IPOPT)
else()
    target_compile_options(PagmoWrapper PRIVATE -UPAGMO_WITH_NLOPT -UPAGMO_WITH_IPOPT)
endif()

# On Linux, set the SONAME so consumers can find the library by its short name.
if(NOT WIN32)
    set_target_properties(PagmoWrapper PROPERTIES
        VERSION   ${PROJECT_VERSION}
        SOVERSION 1
    )
endif()

# Optional solver support. Use PAGMONET_WITH_* names to avoid collision with
# pagmo's own cmake-exported PAGMO_WITH_* variables (which reflect how the
# upstream pagmo library was built, not how we want to build our wrapper).
option(PAGMONET_WITH_NLOPT  "Build PagmoWrapper with NLopt support"  OFF)
option(PAGMONET_WITH_IPOPT  "Build PagmoWrapper with IPOPT support"  OFF)
option(PAGMONET_WITH_SNOPT7 "Build PagmoWrapper with SNOPT7 support" OFF)

if(PAGMONET_WITH_NLOPT)
    find_package(NLopt CONFIG REQUIRED)
    target_compile_definitions(PagmoWrapper PRIVATE PAGMO_WITH_NLOPT)
    target_link_libraries(PagmoWrapper PRIVATE NLopt::nlopt)
endif()

if(PAGMONET_WITH_IPOPT)
    # Try cmake config first — installed by vcpkg (coin-or-ipopt) on both Windows and Linux.
    find_package(Ipopt CONFIG QUIET)
    if(TARGET Ipopt::Ipopt)
        target_compile_definitions(PagmoWrapper PRIVATE PAGMO_WITH_IPOPT)
        target_link_libraries(PagmoWrapper PRIVATE Ipopt::Ipopt)
    else()
        # Fall back to pkg-config for Linux builds against the system apt ipopt package.
        find_package(PkgConfig REQUIRED)
        pkg_check_modules(IPOPT REQUIRED ipopt)
        target_compile_definitions(PagmoWrapper PRIVATE PAGMO_WITH_IPOPT)
        target_link_libraries(PagmoWrapper PRIVATE ${IPOPT_LINK_LIBRARIES})
        target_include_directories(PagmoWrapper PRIVATE ${IPOPT_INCLUDE_DIRS})
        target_link_directories(PagmoWrapper PRIVATE ${IPOPT_LIBRARY_DIRS})
    endif()
endif()

if(PAGMONET_WITH_SNOPT7)
    target_compile_definitions(PagmoWrapper PRIVATE PAGMO_WITH_SNOPT7)
endif()

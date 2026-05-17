cmake_minimum_required(VERSION 3.22)
project(pagmo4j VERSION 1.0.0)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# ── JNI headers ───────────────────────────────────────────────────────────────
find_package(JNI REQUIRED)

# ── pagmo2 + dependencies via vcpkg ──────────────────────────────────────────
# pagmo_export.cmake references pagmo::IPOPT::libipopt when pagmo was built with
# IPOPT support. Pre-create the imported target so find_package(Pagmo) succeeds
# even when we are not linking IPOPT ourselves.
find_library(_pagmo_ipopt_lib NAMES ipopt ipopt-3)
if(_pagmo_ipopt_lib AND NOT TARGET pagmo::IPOPT::libipopt)
    add_library(pagmo::IPOPT::libipopt UNKNOWN IMPORTED)
    set_target_properties(pagmo::IPOPT::libipopt PROPERTIES
        IMPORTED_LOCATION "${_pagmo_ipopt_lib}")
endif()
unset(_pagmo_ipopt_lib)

find_package(Pagmo REQUIRED)
find_package(NLopt CONFIG QUIET)

# ── Shared C++ sources from monorepo pagmoWrapper/ ───────────────────────────
# CMAKE_SOURCE_DIR = java/pagmoWrapper/ → ../../ = monorepo root
set(MONOREPO_ROOT "${CMAKE_SOURCE_DIR}/../..")
set(PAGMOWRAPPER_SRC "${MONOREPO_ROOT}/pagmoWrapper")

file(GLOB PAGMOWRAPPER_SOURCES "${PAGMOWRAPPER_SRC}/*.cpp")

# SWIG-generated JNI glue — produced by scripts/regen-swig.ps1
set(JNI_WRAP_SRC "${CMAKE_SOURCE_DIR}/generated/pagmo4j_wrap.cxx")

add_library(pagmo4j SHARED
    ${PAGMOWRAPPER_SOURCES}
    ${JNI_WRAP_SRC}
)

target_include_directories(pagmo4j PRIVATE
    ${JNI_INCLUDE_DIRS}
    ${PAGMOWRAPPER_SRC}
    "${MONOREPO_ROOT}/swig"
)

target_link_libraries(pagmo4j PRIVATE
    Pagmo::pagmo
    $<$<TARGET_EXISTS:NLopt::nlopt>:NLopt::nlopt>
)

# On Windows, ipopt.lib requires BLAS/LAPACK (Fortran symbols like dsyev_, dgetrf_).
# Intel MKL (sequential LP64 variant) provides them without DllMain conflicts.
if(WIN32)
    find_library(PAGMO4J_MKL_LP64 NAMES mkl_intel_lp64)
    find_library(PAGMO4J_MKL_SEQ  NAMES mkl_sequential)
    find_library(PAGMO4J_MKL_CORE NAMES mkl_core)
    if(PAGMO4J_MKL_LP64 AND PAGMO4J_MKL_SEQ AND PAGMO4J_MKL_CORE)
        target_link_libraries(pagmo4j PRIVATE
            "${PAGMO4J_MKL_LP64}" "${PAGMO4J_MKL_SEQ}" "${PAGMO4J_MKL_CORE}")
    endif()
endif()

# Mirror the .NET wrapper's compile flags:
# - UPAGMO_WITH_NLOPT/IPOPT: undo the INTERFACE_COMPILE_DEFINITIONS from Pagmo::pagmo that
#   set PAGMO_WITH_NLOPT/IPOPT (which affects pagmo internals, not our wrapper).
# - bigobj: the JNI wrap file is large.
# - PAGMO_STATIC_BUILD: suppress __declspec(dllimport) on pagmo symbols; pagmo.lib is a
#   static library and its own config.hpp sets this, but we ensure it's explicit here.
if(MSVC)
    target_compile_options(pagmo4j PRIVATE /UPAGMO_WITH_NLOPT /UPAGMO_WITH_IPOPT /bigobj)
    target_compile_definitions(pagmo4j PRIVATE PAGMO_STATIC_BUILD)
else()
    target_compile_options(pagmo4j PRIVATE -UPAGMO_WITH_NLOPT -UPAGMO_WITH_IPOPT)
    target_compile_definitions(pagmo4j PRIVATE PAGMO_STATIC_BUILD)
endif()

if(APPLE)
    set_target_properties(pagmo4j PROPERTIES
        BUILD_WITH_INSTALL_RPATH ON
        INSTALL_RPATH "@loader_path"
        MACOSX_RPATH ON
    )
elseif(UNIX)
    set_target_properties(pagmo4j PROPERTIES
        BUILD_WITH_INSTALL_RPATH ON
        INSTALL_RPATH "$ORIGIN"
    )
endif()

if(WIN32)
    set_target_properties(pagmo4j PROPERTIES PREFIX "")
endif()

install(TARGETS pagmo4j
    LIBRARY DESTINATION lib
    RUNTIME DESTINATION bin
)

cmake_minimum_required(VERSION 3.20)
project(PagmoWrapper VERSION 1.0.0)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# Hide all symbols by default; only PAGMONET_EXPORT-decorated functions are visible.
# This matches the Windows __declspec(dllexport) model — no unintended ABI leakage.
set(CMAKE_CXX_VISIBILITY_PRESET hidden)
set(CMAKE_VISIBILITY_INLINES_HIDDEN ON)

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

# On Linux, set the SONAME so consumers can find the library by its short name.
set_target_properties(PagmoWrapper PROPERTIES
    VERSION   ${PROJECT_VERSION}
    SOVERSION 1
)

# Optional: NLopt support (mirrors Windows PAGMO_WITH_NLOPT define)
option(PAGMO_WITH_NLOPT  "Build with NLopt support"  OFF)
option(PAGMO_WITH_IPOPT  "Build with IPOPT support"  OFF)
option(PAGMO_WITH_SNOPT7 "Build with SNOPT7 support" OFF)

if(PAGMO_WITH_NLOPT)
    find_package(NLopt REQUIRED)
    target_compile_definitions(PagmoWrapper PRIVATE PAGMO_WITH_NLOPT)
    target_link_libraries(PagmoWrapper PRIVATE NLopt::nlopt)
endif()

if(PAGMO_WITH_IPOPT)
    find_package(IPOPT REQUIRED)
    target_compile_definitions(PagmoWrapper PRIVATE PAGMO_WITH_IPOPT)
    target_link_libraries(PagmoWrapper PRIVATE ${IPOPT_LIBRARIES})
    target_include_directories(PagmoWrapper PRIVATE ${IPOPT_INCLUDE_DIRS})
endif()

if(PAGMO_WITH_SNOPT7)
    target_compile_definitions(PagmoWrapper PRIVATE PAGMO_WITH_SNOPT7)
endif()

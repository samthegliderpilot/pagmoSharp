cmake_minimum_required(VERSION 3.22)
project(pagmo4j VERSION 1.0.0)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# ── JNI headers ───────────────────────────────────────────────────────────────
find_package(JNI REQUIRED)

# ── pagmo2 + dependencies via vcpkg ──────────────────────────────────────────
find_package(pagmo CONFIG REQUIRED)
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
    pagmo::pagmo
    $<$<TARGET_EXISTS:NLopt::nlopt>:NLopt::nlopt>
)

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

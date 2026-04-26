vcpkg_from_github(
    OUT_SOURCE_PATH SOURCE_PATH
    REPO coin-or/Ipopt
    REF ec43e37a06054246764fb116e50e3e30c9ada089
    SHA512 f5b30e81b4a1a178e9a0e2b51b4832f07441b2c3e9a2aa61a6f07807f94185998e985fcf3c34d96fbfde78f07b69f2e0a0675e1e478a4e668da6da60521e0fd6
    HEAD_REF master
)

file(COPY "${CURRENT_INSTALLED_DIR}/share/coin-or-buildtools/" DESTINATION "${SOURCE_PATH}")

set(ENV{ACLOCAL} "aclocal -I \"${SOURCE_PATH}/BuildTools\"")

# On Windows with MSVC static libraries, autotools pkg-config LAPACK detection
# fails because lapack.pc emits "-llapack -llibf2c" but omits "-lopenblas";
# LAPACK routines call into BLAS (openblas), so the dsyev link test fails.
# Passing --with-lapack=<flags> directly bypasses pkg-config and uses the
# explicit flags. CXXLIBS is also unset to suppress the unrelated CXXLIBS
# detection warning that can poison subsequent link tests.
if(VCPKG_TARGET_IS_WINDOWS AND VCPKG_LIBRARY_LINKAGE STREQUAL "static")
    set(LAPACK_OPTION "--with-lapack=-llapack -llibf2c -lopenblas")
    set(CXXLIBS_OPTION "CXXLIBS=")
else()
    set(LAPACK_OPTION "--with-lapack")
    set(CXXLIBS_OPTION "")
endif()

vcpkg_configure_make(
    SOURCE_PATH "${SOURCE_PATH}"
    AUTOCONFIG
    OPTIONS
      --without-spral
      --without-hsl
      --without-asl
      ${LAPACK_OPTION}
      --without-mumps
      --enable-relocatable
      --disable-f77
      --disable-java
    OPTIONS_RELEASE
      ${CXXLIBS_OPTION}
    OPTIONS_DEBUG
      ${CXXLIBS_OPTION}
)

vcpkg_install_make()
vcpkg_copy_pdbs()
vcpkg_fixup_pkgconfig()

file(REMOVE_RECURSE "${CURRENT_PACKAGES_DIR}/debug/include")
file(REMOVE_RECURSE "${CURRENT_PACKAGES_DIR}/debug/share")

file(INSTALL "${SOURCE_PATH}/LICENSE" DESTINATION "${CURRENT_PACKAGES_DIR}/share/${PORT}" RENAME copyright)

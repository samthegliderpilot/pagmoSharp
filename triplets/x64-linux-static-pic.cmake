# Custom vcpkg triplet: static libraries compiled with -fPIC.
# Required when linking static dependencies into a shared library (libPagmoWrapper.so).
# The standard x64-linux-static triplet omits -fPIC, which causes linker errors when
# the static .a files are incorporated into a .so.
set(VCPKG_TARGET_ARCHITECTURE x64)
set(VCPKG_CRT_LINKAGE dynamic)
set(VCPKG_LIBRARY_LINKAGE static)
set(VCPKG_CMAKE_SYSTEM_NAME Linux)
set(VCPKG_CXX_FLAGS "-fPIC")
set(VCPKG_C_FLAGS "-fPIC")

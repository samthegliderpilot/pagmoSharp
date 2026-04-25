# Static libraries with the dynamic CRT (/MD).
# Use this when building PagmoWrapper.dll on Windows: pagmo2, Boost.Serialization, TBB,
# NLopt, and IPOPT are all linked statically into the DLL so consumers need no additional
# runtime DLLs beyond the standard Visual C++ runtime.
#
# /MD (dynamic CRT) is used because it matches the default for Visual C++ projects and
# the .NET CLR host. Using /MT (static CRT, as in x64-windows-static) would cause
# CRT mismatch link errors when the DLL is loaded by a /MD consumer.
set(VCPKG_TARGET_ARCHITECTURE x64)
set(VCPKG_CRT_LINKAGE dynamic)
set(VCPKG_LIBRARY_LINKAGE static)

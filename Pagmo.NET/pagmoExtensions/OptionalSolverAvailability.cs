using System;
using System.Runtime.InteropServices;

namespace pagmo
{
    /// <summary>
    /// Provides native availability checks for optional solvers that may not be compiled
    /// into every build of the PagmoWrapper native library. Use these before attempting
    /// to construct or call optional solver types to avoid <see cref="EntryPointNotFoundException"/>.
    /// </summary>
    public static class OptionalSolverAvailability
    {
        private static readonly IntPtr _libraryHandle = LoadLibrary();

        private static IntPtr LoadLibrary()
        {
            NativeLibrary.TryLoad("PagmoWrapper", typeof(algorithm).Assembly, null, out var handle);
            return handle;
        }

        /// <summary>
        /// Gets whether NLopt is available in the native PagmoWrapper library.
        /// When <see langword="false"/>, the <c>pagmo.nlopt</c> managed type exists but its
        /// native entry points are absent and cannot be called.
        /// </summary>
        public static bool IsNloptAvailable { get; } = HasExport("CSharp_pagmo_new_nlopt__SWIG_0");

        /// <summary>
        /// Gets whether IPOPT is available in the native PagmoWrapper library.
        /// When <see langword="false"/>, the <c>pagmo.ipopt</c> managed type exists but its
        /// native entry points are absent and cannot be called.
        /// </summary>
        public static bool IsIpoptAvailable { get; } = HasExport("CSharp_pagmo_new_ipopt");

        private static bool HasExport(string symbol)
            => _libraryHandle != IntPtr.Zero && NativeLibrary.TryGetExport(_libraryHandle, symbol, out _);
    }
}

using System;
using System.Runtime.InteropServices;

namespace pagmo
{
    public partial class problem
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void FitnessCallback([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2), In] double[] state, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3), Out] double[] answer, [In] int sizeOfState, [In] int sizeOfAnswer);

        internal static class PagmoProblemExternCaller
        {
            //__declspec(dllexport) void RegisterFitnessCallback(PagmoProblem* problem, double* __stdcall operation(double*));
            [DllImport(@"pagmoWrapper.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr RegisterFitnessCallback(HandleRef problem, FitnessCallback callback);
        }

        public void RegisterFitnessCallback(FitnessCallback callback)
        {
            PagmoProblemExternCaller.RegisterFitnessCallback(swigCPtr, callback);
        }
    }
}

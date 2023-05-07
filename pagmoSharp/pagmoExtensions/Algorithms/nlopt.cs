namespace pagmo
{
    public partial class nlopt : IAlgorithm
    {
        public void set_seed(uint seed)
        {
        }

        public uint get_seed()
        {
            return 0;
        }

        public uint get_verbosity()
        {
            return 0;
        }

        public void set_local_optimizer(nlopt other)
        {
            nloptInvoke.nlopt_set_local_optimizer(swigCPtr, nlopt.getCPtr(other));
        }
    }

    public class nloptInvoke
    {
        [global::System.Runtime.InteropServices.DllImport("pagmoWrapper.dll", EntryPoint = "CSharp_pagmo_nlopt_set_local_optimizer")]
        public static extern void nlopt_set_local_optimizer(global::System.Runtime.InteropServices.HandleRef jarg1, global::System.Runtime.InteropServices.HandleRef jarg2);

    }
}
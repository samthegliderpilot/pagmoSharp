using System;
using System.Runtime.InteropServices;

namespace pagmo
{
    public partial class problem
    {
        public static implicit operator problemBase(problem d) => d.getBaseProblem();
    }
}

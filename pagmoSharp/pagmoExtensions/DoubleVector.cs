using System.Linq;

namespace pagmo
{
    public partial class DoubleVector
    {
        public DoubleVector(params double[] values)
        :this(values.AsEnumerable()) //TODO: Better performance somehow?
        {
            
        }
    }
}

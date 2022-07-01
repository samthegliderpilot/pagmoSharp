using System;

namespace pagmo
{
    public interface IAlgorithm : IDisposable
    {
		public population evolve(population pop) ;
        public void set_seed(uint seed);
        public uint get_seed() ;
        public uint get_verbosity() ;
        public void set_verbosity(uint level);
        public uint get_gen() ;
        public string get_name() ;
        public string get_extra_info() ;
    }
}
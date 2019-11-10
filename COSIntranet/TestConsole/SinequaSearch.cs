using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    public class SinequaSearch
    {
        public string QueryText { get; set; }
        public string ResultId { get; set; }
        public int ItemCount { get; set; }

        public List<SinequaDcoument> DocumentItems { get; } = new List<SinequaDcoument>();

    }
}

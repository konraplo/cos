using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    public class SinequaProfile
    {
        public string Title { get; set; }
        public List<SinequaSearch> SearchItems { get; } = new List<SinequaSearch>();

    }
}

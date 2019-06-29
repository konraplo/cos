using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime grandOpening = DateTime.Now;
            DateTime firstDelivery = grandOpening.AddDays(-13);
            DateTime secondDelivery = grandOpening.AddDays(-7);
            Console.WriteLine(string.Format("{0},{1},{2}", string.Format("{0:MMMM dd, yyyy}", grandOpening), string.Format("{0:dd-MM-yyyy}", firstDelivery), string.Format("{0:dd-MM-yyyy}", secondDelivery)));

        }
    }
}

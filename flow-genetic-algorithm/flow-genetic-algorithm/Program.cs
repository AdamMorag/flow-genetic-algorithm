using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroService4Net;

namespace flow_genetic_algorithm
{
    class Program
    {
        static void Main(string[] args)
        {            
            var microService = new MicroService();
            microService.Run(args);
        }
    }
}

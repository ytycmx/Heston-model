using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalAssignment1;

namespace ConsoleApp1
{
    
        static void Main()
        {
            double r = 0.025;
            double theta = 0.0398;
            double k = 1.5768;
            double sigma = 0.5751;
            double rho = -0.5711;
            double nv = 0.0175;
            double S = 100;
            double K = 100;
            double T = 1;
            Hestonformula r1 = new Hestonformula();
            return r1 = Hestonformula.CalculateCallOptionPrice(S, K, r, T, nv, k, theta, rho, sigma);

        }
    
}

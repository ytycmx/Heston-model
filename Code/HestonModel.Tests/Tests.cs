using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HestonModel.Tests
{
    public class Tests
    {
        //Tests will be added here after the deadline.
        //If you wish to add your own tests then please add a new class or project.
        /*[Test]
        public void Task2_2()
        {
            double kappa = 1.5768;
            double theta = 0.0398;
            double sigma = 0.5751;
            double v0 = 0.0175;
            double rho = -0.5711;
            double riskFreeRate = 0.025;
            double strikePrice = 100;
            double[] exerciseTime = new double[] { 1, 2, 3, 4, 15 };
            double initialStockPrice = 100;
            int type = 1;// 1 is call, other is put
            double[] callOptionPrice = new double[exerciseTime.Count()];
            for (int i = 0; i < exerciseTime.Count(); i++)
            {
                IHestonModelParameters returnIHestonParameters = new HestonModelParameters(kappa, theta, sigma, v0, rho, initialStockPrice, riskFreeRate);
                IEuropeanOption returnIEuropeanOption = new EuropeanOption(strikePrice, type, exerciseTime[i]);
                callOptionPrice[i] = Heston.HestonEuropeanOptionPrice(returnIHestonParameters, returnIEuropeanOption);
            }
            for (int i = 0; i < exerciseTime.Count(); i++)
            {
                Console.WriteLine("Exercise Time = {0} , Call Option Price = {1}", exerciseTime[i], callOptionPrice[i]);
            }

        }*/
    }
}

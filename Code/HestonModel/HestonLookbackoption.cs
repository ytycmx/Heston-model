using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalAssignment3;

namespace FinalAssignment5
{
    /// <summary>
    /// Calculate Heston Look back option price by Monte Carlo
    /// </summary>
    public class HestonLookbackoption
    {
        /// <summary>
        /// the whole process to calculate
        /// </summary>
        /// <param name="S">Initial Stock Price</param>
        /// <param name="r">Risk Free Rate</param>
        /// <param name="T">Maturity</param>
        /// <param name="nv">parameter in model</param>
        /// <param name="k">parameter in model</param>
        /// <param name="theta">parameter in model</param>
        /// <param name="rho">parameter in model</param>
        /// <param name="sigma">parameter in model</param>
        /// <param name="N">Number Of Time Steps</param>
        /// <param name="n">Number Of Trials</param>
        /// <returns>the price of look back option</returns>
        public double CalculateLookbackCallOptionPrice(double S, double r, double T, double nv, double k, double theta, double rho, double sigma, int N, int n)
        {
            if (S <= 0 || T <= 0)
                throw new System.ArgumentException("Need S, K, T > 0");
            if (2.0 * k * theta <= sigma * sigma)
                throw new System.ArgumentException("need to meet Feller condition");
            else
            {
                double Priceitself = 0.0;
                double[] callprice = new double[n];
                for (int i = 0; i < n; i++)
                {
                    callprice[i] = Math.Exp(-r*T)*(GeneratePath.Generateallpath(S, r, T, nv, k, theta, rho, sigma/*, tau*/, N, n)[N] - GeneratePath.Generateallpath(S, r, T, nv, k, theta, rho, sigma, N, n).Min());
                }
                for (int j = 0; j < n; j++)
                {
                    Priceitself = Priceitself + callprice[j];
                }
                return Priceitself / n;
            }
        }
    }
}

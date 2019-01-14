using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalAssignment3;

namespace FinalAssignment7
{
    /// <summary>
    /// calculate the Heston Rainbow option price
    /// </summary>
    public class HestonRainbowoptionprice
    {
        /// <summary>
        /// how to calculate the price by using the Monte Carlo and payoff formula
        /// </summary>
        /// <param name="S">Initial Stock Price</param>
        /// <param name="r">Risk Free Rate</param>
        /// <param name="T">Maturity</param>
        /// <param name="K">Strike Price</param>
        /// <param name="nv">parameter in model</param>
        /// <param name="k">parameter in model</param>
        /// <param name="theta">parameter in model</param>
        /// <param name="rho">parameter in model</param>
        /// <param name="sigma">parameter in model</param>
        /// <param name="N">Number Of Time Steps</param>
        /// <param name="n">Number Of Trials</param>
        /// <returns>calculate the price from the payoff formula</returns>
        public double CalculateRainbowOptionPrice(double S, double K, double r, double T, double nv, double k, double theta, double rho, double sigma, int N, int n)
        {
            if (S <= 0 || T <= 0)
                throw new System.ArgumentException("Need S, K, T > 0");
            if (2.0 * k * theta <= sigma * sigma)
                throw new System.ArgumentException("need to meet Feller condition");
            else
            {
                double priceitself;
                double[] price = new double[n];
                for (int j = 0; j < n; j++)
                {
                    price[j] = Math.Exp(-r * T) * Math.Max(GeneratePath.Generateallpath(S, r, T, nv, k, theta, rho, sigma, N, n).Max(), K);
                }
                priceitself = price.Sum();
                return priceitself / n;
            }
        }
    }
}

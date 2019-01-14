using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Distributions;
using FinalAssignment3;

namespace FinalAssignment2
{
    /// <summary>
    /// how to use Monte Carlo
    /// </summary>
    public class HestonMC
    {
        /// <summary>
        /// For how to calculate the European option price by MC
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
        /// <returns>the price we generating from Monte Carlo method</returns>
        public double CalculateEuropeanCallOptionPrice(double S, double K, double r, double T, double nv, double k, double theta, double rho, double sigma, int N, int n)
        {
            if (S <= 0 || K <= 0 || T <= 0)
                throw new System.ArgumentException("Need S, K, T > 0");
            if (2 * k * theta <= sigma * sigma)
                throw new System.ArgumentException("need to meet Feller condition");
            else
            {
                double[] callprice = new double[n];
                for (int i = 0; i < n; i++)
                {
                    callprice[i] = GeneratePath.Generateallpath(S, r, T, nv, k, theta, rho, sigma, N, n)[N];
                }
                double Price = 0.0;
                int j = 0;
                double[] aaa = new double[n];
                for (j = 0; j < n; j++)
                {
                    Price = Price + Math.Exp(-r * T) * Math.Max(callprice[j] - K, 0);
                    aaa[j] = Math.Max(callprice[j] - K, 0);
                }
                return Price/n;
            }
        }

        public double CalculatePutOptionPrice(double S, double K, double r, double T, double nv, double k, double theta, double rho, double sigma, int N, int n)
        {
            if (S <= 0 || K <= 0 || T <= 0)
                throw new System.ArgumentException("Need S, K, T > 0");
            if (2 * k * theta <= sigma * sigma)
                throw new System.ArgumentException("need to meet Feller condition");
            return CalculateEuropeanCallOptionPrice(S, K, r, T, nv, k, theta, rho, sigma, N, n) - S + K * Math.Exp(-r * (T));
        }

    }
}

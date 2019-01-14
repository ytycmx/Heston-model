using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Distributions;
using FinalAssignment3;

namespace FinalAssignment4
{
    /// <summary>
    /// calculate Heston Asian call and put Price
    /// </summary>
    public class HestonAsianoption
    {
        
        private void CheckAsianOptionInputs(IEnumerable<double> T, double exerciseT)
        {
            if (T.Count() == 0)
                throw new System.ArgumentException("Need at least one monitoring date for Asian option.");

            if (T.ElementAt(0) <= 0)
                throw new System.ArgumentException("The first monitoring date must be positive.");


            for (int i = 1; i < T.Count(); ++i)
            {
                if (T.ElementAt(i-1) >= T.ElementAt(i))
                    throw new System.ArgumentException("Monitoring dates must be increasing.");
            }

            if (T.ElementAt(T.Count()-1) > exerciseT)
                throw new System.ArgumentException("Last monitoring dates must not be greater than the exercise time.");

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="S">Initial Stock Price</param>
        /// <param name="r">Risk Free Rate</param>
        /// <param name="T">Maturity</param>
        /// <param name="K">Strike Price</param>
        /// <param name="exerciseT">Monitoring Times</param>
        /// <param name="nv">parameter in model</param>
        /// <param name="k">parameter in model</param>
        /// <param name="theta">parameter in model</param>
        /// <param name="rho">parameter in model</param>
        /// <param name="sigma">parameter in model</param>
        /// <param name="N">Number Of Time Steps</param>
        /// <param name="n">Number Of Trials</param>
        /// <returns>the price of Asian option</returns>
        public double CalculateAsianCallOptionPrice(double S, double K, double r, IEnumerable<double> T, double exerciseT,  double nv, double k, double theta, double rho, double sigma, int N, int n)
        {
            if (S <= 0 || K <= 0 || exerciseT <= 0)
                throw new System.ArgumentException("Need S, K, T > 0");

            CheckAsianOptionInputs(T, exerciseT);

            if (2.0 * k * theta <= sigma * sigma)
                throw new System.ArgumentException("need to meet Feller condition");
            else
            {
                int M = T.Count();
                double tau = exerciseT / N;
                double[] givenprice = new double[M];
                double[] callprice = new double[n];
                double[] path = new double[N];
                int number;
                double priceitself;
                double price;
                double Price;
                for (int u = 0; u < n; u++)
                {
                    path = GeneratePath.Generateallpath(S, r, exerciseT, nv, k, theta, rho, sigma/*, tau*/, N, n);
                    for (int i = 0; i < M; i++)
                    {
                        number = (int)(T.ElementAt(i) / tau);
                        givenprice[i] = path[number];
                    }
                    priceitself = givenprice.Sum();
                    price = Math.Exp(-r * exerciseT) * Math.Max(priceitself / M - K, 0);
                    callprice[u] = price;
                }
                Price = callprice.Sum();
                return Price / n;
            }
        }
        public double CalculateAsianPutOptionPrice(double S, double K, double r, IEnumerable<double> T, double exerciseT, double nv, double k, double theta, double rho, double sigma, int N, int n)
        {
            if (S <= 0 || K <= 0 || exerciseT <= 0)
                throw new System.ArgumentException("Need S, K, T > 0");

            CheckAsianOptionInputs(T, exerciseT);

            if (2.0 * k * theta <= sigma * sigma)
                throw new System.ArgumentException("need to meet Feller condition");
            else
            {
                int M = T.Count();
                double tau = exerciseT / N;
                double[] givenprice = new double[M];
                double[] callprice = new double[n];
                double[] path = new double[N];
                int number;
                double priceitself;
                double price;
                double Price;
                for (int u = 0; u < n; u++)
                {
                    path = GeneratePath.Generateallpath(S, r, exerciseT, nv, k, theta, rho, sigma/*, tau*/, N, n);
                    for (int i = 0; i < M; i++)
                    {
                        number = (int)(T.ElementAt(i) / tau);
                        givenprice[i] = path[number];
                    }
                    priceitself = givenprice.Sum();
                    price = Math.Exp(-r * exerciseT) * Math.Max(K - priceitself / M, 0);
                    callprice[u] = price;
                }
                Price = callprice.Sum();
                return Price / n;
            }
        }


    }
}
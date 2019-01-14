using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;


namespace FinalAssignment3
{
    /// <summary>
    /// this class is for generate MC paths
    /// </summary>
    public class GeneratePath
    {
        public double r;
        public double T;
        public double S;
        public int N;
        public double k;
        public double theta;
        public double rho;
        public double sigma;
        public double nv;
        public double tau;
        public int n;
        public GeneratePath(double S, double r, double T, double nv, double k, double theta, double rho, double sigma, int N, int n)
        {
            if (sigma <= 0 || N <= 0)
                throw new System.ArgumentException("Need sigma >0, N > 0.");
            this.S = S;
            this.T = T;
            this.nv = nv;
            this.k = k;
            this.theta = theta;
            this.rho = rho;
            this.sigma = sigma;
            this.r = r;
            this.N = N;
            this.n = n;
        }

        /// <summary>
        /// this is for generate all the items on one path
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
        /// <returns>a double[] which contains all items we generated</returns>
        public static double[] Generateallpath(double S, double r, double T, double nv, double k, double theta, double rho, double sigma, int N, int n)
        {
            double alpha = (4 * k * theta - sigma * sigma) / 8;
            double beta = -k / 2;
            double gamma = sigma / 2;
            int i;
            if (N <= 0 || S <= 0 || T <= 0)
                throw new System.ArgumentException("Need N, S, T > 0");
            double tau = T / N;
            double[] s = new double[N + 1];
            double[] y = new double[N + 1];
            s[0] = S;
            y[0] = Math.Sqrt(nv);
            double[] DeltaZ1 = new double[N+1];
            double[] DeltaZ2 = new double[N+1];
            double[] x1 = new double[N];
            double[] x2 = new double[N];
            Normal.Samples(x1, 0, 1);
            Normal.Samples(x2, 0, 1);
            for (i = 0; i < N; i++)
            {
                DeltaZ1[i+1] = Math.Sqrt(tau) * x1[i];
                DeltaZ2[i+1] = Math.Sqrt(tau) * (rho * x1[i] + Math.Sqrt(1 - rho * rho) * x2[i]);
                y[i + 1] = (y[i] + gamma * DeltaZ2[i + 1]) / (2 * (1 - beta * tau)) + Math.Sqrt(Math.Pow(y[i] + gamma * DeltaZ2[i + 1], 2.0) / (4 * Math.Pow(1 - beta * tau, 2.0)) + alpha * tau / (1 - beta * tau));
                s[i + 1] = s[i] + r * s[i] * tau + y[i] * s[i] * DeltaZ1[i + 1];
            }
            return s;

        }
    }
}

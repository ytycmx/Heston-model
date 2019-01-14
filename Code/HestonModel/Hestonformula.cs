using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Integration;
using System.Numerics;
using SolversAndIntegrators;

namespace FinalAssignment1
{
    /// <summary>
    /// calculate Heston European option price by formula
    /// </summary>
    public class Hestonformula
    {
        public double u1 = 0.5;
        public double u2 = -0.5;
        public Complex i = new Complex(0.0, 1.0);

        public const int numModelParams = 5;

        private const int nvIndex = 0;
        private const int kIndex = 1;
        private const int thetaIndex = 2;
        private const int rhoIndex = 3;
        private const int sigmaIndex = 4;

        public double k;
        public double theta;
        public double rho;
        public double sigma;
        public double nv;
        public double phi;


        public Hestonformula()
        {
        }

        public Hestonformula(Hestonformula otherModel) :
            this(otherModel.nv, otherModel.k, otherModel.theta, otherModel.rho, otherModel.sigma)
        {
        }

        public Hestonformula(double nv, double k, double theta, double rho, double sigma)
        {
            this.nv = nv;
            this.k = k;
            this.theta = theta;
            this.rho = rho;
            this.sigma = sigma;
        }

        public Hestonformula(double[] paramsArray)
            : this(paramsArray[nvIndex], paramsArray[kIndex], paramsArray[thetaIndex], paramsArray[rhoIndex], paramsArray[sigmaIndex])
        {
        }

        public double GetNv() { return nv; }
        public double GetK() { return k; }
        public double GetTheta() { return theta; }
        public double GetRho() { return rho; }
        public double GetSigma() { return sigma; }


        /// <summary>
        /// the formula for calculating
        /// </summary>
        /// <param name="S">Initial Stock Price</param>
        /// <param name="r">Risk Free Rate</param>
        /// <param name="T">Maturity</param>
        /// <param name="K">Strike Price</param>
        /// <returns>the final price of Euorpean option</returns>
        public double CalculateCallOptionPrice(double S, double K, double r, double T)
        {
            
            double b1 = k - rho * sigma;
            double b2 = k;
            double a = k * theta;
            if (sigma <= 0 || T <= 0 || K <= 0 || S <= 0)
                throw new System.ArgumentException("Need sigma > 0, T > 0, K > 0 and S > 0.");
            Complex d1(double phi) { return Complex.Sqrt(Complex.Pow(rho * sigma * phi * i - b1, 2.0) - sigma * sigma * (2.0 * u1 * phi * i - phi * phi)); }
            Complex d2(double phi) { return Complex.Sqrt(Complex.Pow(rho * sigma * phi * i - b2, 2.0) - sigma * sigma * (2.0 * u2 * phi * i - phi * phi)); }
            Complex g1(double phi) { return (b1 - rho * sigma * phi * i - d1(phi)) / (b1 - rho * sigma * phi * i + d1(phi)); }
            Complex g2(double phi) { return (b2 - rho * sigma * phi * i - d2(phi)) / (b2 - rho * sigma * phi * i + d2(phi)); }
            Complex C1(double tau, double phi) { return r * phi * i * tau + (a / Math.Pow(sigma, 2.0)) * ((b1 - rho * sigma * phi * i - d1(phi)) * tau - 2.0 * Complex.Log((1.0 - g1(phi) * Complex.Exp(-tau * d1(phi))) / (1.0 - g1(phi)))); }
            Complex C2(double tau, double phi) { return r * phi * i * tau + (a / Math.Pow(sigma, 2.0)) * ((b2 - rho * sigma * phi * i - d2(phi)) * tau - 2.0 * Complex.Log((1.0 - g2(phi) * Complex.Exp(-tau * d2(phi))) / (1.0 - g2(phi)))); }
            Complex D1(double tau, double phi) { return ((b1 - rho * sigma * phi * i - d1(phi)) / Math.Pow(sigma, 2.0)) * ((1.0 - Complex.Exp(-tau * d1(phi))) / (1.0 - g1(phi) * Complex.Exp(-tau * d1(phi)))); }
            Complex D2(double tau, double phi) { return ((b2 - rho * sigma * phi * i - d2(phi)) / Math.Pow(sigma, 2.0)) * ((1.0 - Complex.Exp(-tau * d2(phi))) / (1.0 - g2(phi) * Complex.Exp(-tau * d2(phi)))); }
            Complex Phi1(double x, double phi) { return Complex.Exp(C1(T, phi) + D1(T, phi) * nv + i * phi * x); }
            Complex Phi2(double x, double phi) { return Complex.Exp(C2(T, phi) + D2(T, phi) * nv + i * phi * x); }
            Complex integrand1(double x, double phi) { return (Complex.Exp(-i * phi * Math.Log(K)) * Phi1(x, phi)) / (i * phi); }
            Complex integrand2(double x, double phi) { return (Complex.Exp(-i * phi * Math.Log(K)) * Phi2(x, phi)) / (i * phi); }

            double P1(double x)
            {

                Func<double, double> IntegrateFormula
                = (phi) => {
                    Complex aj = integrand1(x, phi);
                    return aj.Real;
                };
                CompositeIntegrator integrator = new CompositeIntegrator(4);
                /*double integral = integrator.Integrate(IntegrateFormula, 0.0001, 10000, 10000);*/
                double integral = NewtonCotesTrapeziumRule.IntegrateComposite(IntegrateFormula, 0.0001, 10000, 10000);
                /*double integral = DoubleExponentialTransformation.Integrate(IntegrateFormula, 0.01, 1e3, 1e-5);*/
                return 0.50 + (1 / Math.PI) * integral;
            }
            double P2(double x)
            {

                Func<double, double> IntegrateFormula
                = (phi) => {
                    Complex al = integrand2(x, phi);
                    return al.Real;
                };
                CompositeIntegrator integrator = new CompositeIntegrator(4);
                /*double integral = integrator.Integrate(IntegrateFormula, 0.0001, 10000, 10000);*/
                double integral2 = NewtonCotesTrapeziumRule.IntegrateComposite(IntegrateFormula, 0.0001, 10000, 10000);
                /*double integral = DoubleExponentialTransformation.Integrate(IntegrateFormula, 0.01, 1e3, 1e-5);*/
                return 0.50 + (1 / Math.PI) * integral2;
            }
            return S * P1(Math.Log(S)) - Math.Exp(-r * T) * K * P2(Math.Log(S));


        }

        public double GetPutFromCallPrice(double S, double K, double r, double T, double callPrice)
        {
            return callPrice - S + K * Math.Exp(-r * T);
        }

        public double CalculatePutOptionPrice(double S, double K, double r, double T)
        {
            if (sigma <= 0 || T <= 0 || K <= 0 || S <= 0)
                throw new System.ArgumentException("Need sigma > 0, T > 0, K > 0 and S > 0.");

            return GetPutFromCallPrice(S, K, r, T, CalculateCallOptionPrice(S, K, r, T));
        }


        // For use in calibration
        public double[] ConvertHestonModelCalibrationParamsToArray()
        {
            double[] paramsArray = new double[Hestonformula.numModelParams];
            paramsArray[nvIndex] = nv;
            paramsArray[kIndex] = k;
            paramsArray[thetaIndex] = theta;
            paramsArray[rhoIndex] = rho;
            paramsArray[sigmaIndex] = sigma;
            return paramsArray;
        }

    }
}
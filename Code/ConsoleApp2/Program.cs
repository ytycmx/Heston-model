using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalAssignment1;
using FinalAssignment2;
using FinalAssignment4;
using FinalAssignment5;
using FinalAssignment6;
using FinalAssignment7;
using System.Xml;
using System.Xml.Linq;

namespace ConsoleApp2
{
    /// <summary>
    /// give certain value of all parameters and have the final results
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            double r = 0.10;
            double theta = 0.06;
            double k = 2.00;
            double sigma = 0.40;
            double rho = 0.5;
            double nv = 0.04;
            /*double r = 0.025;
            double theta = 0.0398;
            double k = 1.5768;
            double sigma = 0.5751;
            double rho = -0.5711;
            double nv = 0.0175;*/


            double S = 100;
            double T1 = 1.0;
            double T2 = 2.0;
            double T3 = 3.0;
            double T4 = 4.0;
            double T5 = 15.0;
            double K = 100.0;
            double C1, C2, C3, C4, C5, C6, C7, C8, C9, C10;
            double[] parameters = new double[5];
            parameters[0] = nv;
            parameters[1] = k;
            parameters[2] = theta;
            parameters[3] = rho;
            parameters[4] = sigma;
            /*Hestonformula r1 = new Hestonformula(parameters);
            Console.WriteLine("These are from Heston formula call option:");
            C1 = r1.CalculateCallOptionPrice(S, K, r, T1);
            Console.WriteLine(C1);
            C2 = r1.CalculateCallOptionPrice(S, K, r, T2);
            Console.WriteLine(C2);
            C3 = r1.CalculateCallOptionPrice(S, K, r, T3);
            Console.WriteLine(C3);
            C4 = r1.CalculateCallOptionPrice(S, K, r, T4);
            Console.WriteLine(C4);
            C5 = r1.CalculateCallOptionPrice(S, K, r, T5);
            Console.WriteLine(C5);
            Console.WriteLine("These are from Heston formula put option:");
            C6 = r1.CalculatePutOptionPrice(S, K, r, T1);
            Console.WriteLine(C6);
            C7 = r1.CalculatePutOptionPrice(S, K, r, T2);
            Console.WriteLine(C7);
            C8 = r1.CalculatePutOptionPrice(S, K, r, T3);
            Console.WriteLine(C8);
            C9 = r1.CalculatePutOptionPrice(S, K, r, T4);
            Console.WriteLine(C9);
            C10 = r1.CalculatePutOptionPrice(S, K, r, T5);
            Console.WriteLine(C10);*/


            int n = 100000;
            int N = 365;
            double tau1 = T1 / N;
            double tau2 = T2 / N;
            double tau3 = T3 / N;
            double tau4 = T4 / N;
            double tau5 = T5 / N;
            double A1, A2, A3, A4, A5, A6, A7, A8, A9, A10;
            /*HestonMC p1 = new HestonMC();
            Console.WriteLine("These are from Heston European call option MC:");
            A1 = p1.CalculateEuropeanCallOptionPrice(S, K, r, T1, nv, k, theta, rho, sigma, N, n);
            Console.WriteLine(A1);
            A2 = p1.CalculateEuropeanCallOptionPrice(S, K, r, T2, nv, k, theta, rho, sigma, N, n);
            Console.WriteLine(A2);
            A3 = p1.CalculateEuropeanCallOptionPrice(S, K, r, T3, nv, k, theta, rho, sigma, N, n);
            Console.WriteLine(A3);
            A4 = p1.CalculateEuropeanCallOptionPrice(S, K, r, T4, nv, k, theta, rho, sigma, N, n);
            Console.WriteLine(A4);
            A5 = p1.CalculateEuropeanCallOptionPrice(S, K, r, T5, nv, k, theta, rho, sigma, N, n);
            Console.WriteLine(A5);
            Console.WriteLine("These are from Heston European put option MC:");
            A6 = p1.CalculatePutOptionPrice(S, K, r, T1, nv, k, theta, rho, sigma, N, n);
            Console.WriteLine(A6);
            A7 = p1.CalculatePutOptionPrice(S, K, r, T2, nv, k, theta, rho, sigma, N, n);
            Console.WriteLine(A7);
            A8 = p1.CalculatePutOptionPrice(S, K, r, T3, nv, k, theta, rho, sigma, N, n);
            Console.WriteLine(A8);
            A9 = p1.CalculatePutOptionPrice(S, K, r, T4, nv, k, theta, rho, sigma, N, n);
            Console.WriteLine(A9);
            A10 = p1.CalculatePutOptionPrice(S, K, r, T5, nv, k, theta, rho, sigma, N, n);
            Console.WriteLine(A10);*/


            double t1 = 1.0;
            double t2 = 3.0;
            double t3 = 5.0;
            double t4 = 7.0;
            double t5 = 9.0;
            double K1, K2, K3, K4, K5;
            /*HestonLookbackoption w1 = new HestonLookbackoption();
            Console.WriteLine("These are from Look back option MC:");
            K1 = w1.CalculateLookbackCallOptionPrice(S, r, t1, nv, k, theta, rho, sigma, N, n);
            Console.WriteLine(K1);
            K2 = w1.CalculateLookbackCallOptionPrice(S, r, t2, nv, k, theta, rho, sigma, N, n);
            Console.WriteLine(K2);
            K3 = w1.CalculateLookbackCallOptionPrice(S, r, t3, nv, k, theta, rho, sigma, N, n);
            Console.WriteLine(K3);
            K4 = w1.CalculateLookbackCallOptionPrice(S, r, t4, nv, k, theta, rho, sigma, N, n);
            Console.WriteLine(K4);
            K5 = w1.CalculateLookbackCallOptionPrice(S, r, t5, nv, k, theta, rho, sigma, N, n);
            Console.WriteLine(K5);*/


            /*double[] T11 = { 0.75, 1.00 };
            double[] T22 = { 0.25, 0.50, 0.75, 1.00, 1.25, 1.50, 1.75 };
            double[] T33 = { 1.00, 2.00, 3.00 };*/
            /*IEnumerable<double> T11 = new double[] { 0.75, 1.00 };
            IEnumerable<double> T22 = new double[] { 0.25, 0.50, 0.75, 1.00, 1.25, 1.50, 1.75 };
            IEnumerable<double> T33 = new double[] { 1.00, 2.00, 3.00 };
            double O1, O2, O3, O4, O5, O6;
            HestonAsianoption v1 = new HestonAsianoption();
            Console.WriteLine("These are from Asian call option MC:");
            O1 = v1.CalculateAsianCallOptionPrice(S, K, r, T11, T1, nv, k, theta, rho, sigma, N, n);
            Console.WriteLine(O1);
            O2 = v1.CalculateAsianCallOptionPrice(S, K, r, T22, T2, nv, k, theta, rho, sigma, N, n);
            Console.WriteLine(O2);
            O3 = v1.CalculateAsianCallOptionPrice(S, K, r, T33, T3, nv, k, theta, rho, sigma, N, n);
            Console.WriteLine(O3);*/
            /*Console.WriteLine("These are from Asian put option MC:");
            O4 = v1.CalculateAsianPutOptionPrice(S, K, r, T11, T1, nv, k, theta, rho, sigma, N, n);
            Console.WriteLine(O4);
            O5 = v1.CalculateAsianPutOptionPrice(S, K, r, T22, T2, nv, k, theta, rho, sigma, N, n);
            Console.WriteLine(O5);
            O6 = v1.CalculateAsianPutOptionPrice(S, K, r, T33, T3, nv, k, theta, rho, sigma, N, n);
            Console.WriteLine(O6);*/


            double r0 = 0.025;
            double S0 = 100;
            double theta1 = 0.0398;
            double k1 = 1.5768;
            double sigma1 = 0.5751;
            double rho1 = -0.5711;
            double nv1 = 0.0175;

            Hestonformula model = new Hestonformula(nv1, k1, theta1, rho1, sigma1);
            double[] optionExerciseTimes = new double[] { 1, 1, 2, 2, 1.5 };
            double[] optionStrikes = new double[] { 80, 90, 80, 100, 100 };
            double[] prices = new double[] { 25.72, 18.93, 30.49, 19.36, 16.58};

            HestonCalibrator calibrator = new HestonCalibrator(r0, S0, 1e-3, 1000);
            calibrator.SetGuessParameters(nv1, k1, theta1, rho1, sigma1);
            for (int i = 0; i < optionExerciseTimes.Length; ++i)
            {
                calibrator.AddObservedOption(optionExerciseTimes[i], optionStrikes[i], prices[i]);
            }
            calibrator.Calibrate();
            double error = 0;
            CalibrationOutcome outcome = CalibrationOutcome.NotStarted;
            calibrator.GetCalibrationStatus(ref outcome, ref error);
            Console.WriteLine("Calibration outcome: {0} and error: {1}", outcome, error);


            /*double U1, U2, U3, U4, U5;
            HestonRainbowoptionprice e1 = new HestonRainbowoptionprice();
            Console.WriteLine("These are from Rainbow option MC:");
            U1 = e1.CalculateRainbowOptionPrice(S, K, r, T1, nv, k, theta, rho, sigma, N, n);
            Console.WriteLine(U1);
            U2 = e1.CalculateRainbowOptionPrice(S, K, r, T2, nv, k, theta, rho, sigma, N, n);
            Console.WriteLine(U2);
            U3 = e1.CalculateRainbowOptionPrice(S, K, r, T3, nv, k, theta, rho, sigma, N, n);
            Console.WriteLine(U3);
            U4 = e1.CalculateRainbowOptionPrice(S, K, r, T4, nv, k, theta, rho, sigma, N, n);
            Console.WriteLine(U4);
            U5 = e1.CalculateRainbowOptionPrice(S, K, r, T5, nv, k, theta, rho, sigma, N, n);
            Console.WriteLine(U5);*/

            Console.ReadKey();
        }
    }
}

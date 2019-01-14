using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalAssignment1;

namespace HestonModel
{
    public class CalibrationFailedException : Exception
    {
        public CalibrationFailedException()
        {
        }
        public CalibrationFailedException(string message)
            : base(message)
        {
        }
    }


    public enum CalibrationOutcome
    {
        NotStarted,
        FinishedOK,
        FailedMaxItReached,
        FailedOtherReason
    };


    public struct HMCallOptionMarketData
    {
        public double optionExercise;
        public double strike;
        public double marketMidPrice;
    }

    /// <summary>
    /// calculate the error, outcome type and the parameters we calibrated
    /// </summary>
    public class HestonCalibrator
    {
        private const double defaultAccuracy = 10e-3;
        private const int defaultMaxIterations = 1000;
        private double accuracy;
        private int maxIterations;

        private LinkedList<HMCallOptionMarketData> marketOptionsList;
        private double r; // initial interest rate, this is observed, no need to calibrate to options
        private double S;
        public double nv;
        public double k;
        public double theta;
        public double rho;
        public double sigma;
        private CalibrationOutcome outcome;

        private double[] calibratedParams;

        public HestonCalibrator()
        {
            accuracy = defaultAccuracy;
            maxIterations = defaultMaxIterations;
            marketOptionsList = new LinkedList<HMCallOptionMarketData>();
            r = 0.025;
            S = 100;
            calibratedParams = new double[] { 0.0175, 1.5768, 0.0398, -0.5711, 0.5751 };
        }

        public HestonCalibrator(double r, double S, double accuracy, int maxIterations)
        {
            this.r = r;
            this.S = S;
            this.accuracy = accuracy;
            this.maxIterations = maxIterations;
            marketOptionsList = new LinkedList<HMCallOptionMarketData>();
            calibratedParams = new double[] { 0.0175, 1.5768, 0.0398, -0.5711, 0.5751 };
        }

        public void SetGuessParameters(double nv, double k, double theta, double rho, double sigma)
        {
            Hestonformula m = new Hestonformula(nv, k, theta, rho, sigma);
            calibratedParams = m.ConvertHestonModelCalibrationParamsToArray();
        }
        public void AddObservedOption(double optionExercise, double strike, double mktMidPrice)
        {
            HMCallOptionMarketData observedOption;
            observedOption.optionExercise = optionExercise;
            observedOption.strike = strike;
            observedOption.marketMidPrice = mktMidPrice;
            marketOptionsList.AddLast(observedOption);
        }

        private const int nvIndex = 0;
        private const int kIndex = 1;
        private const int thetaIndex = 2;
        private const int rhoIndex = 3;
        private const int sigmaIndex = 4;

        //now we figure out the difference
        /// <summary>
        /// calculate the mean square error
        /// </summary>
        /// <param name="m">the array contained the parameters</param>
        /// <returns>the mean square error between Model and Market</returns>
        public double CalcMeanSquareErrorBetweenModelAndMarket(Hestonformula m)
        {
            double meanSqErr = 0;
            foreach (HMCallOptionMarketData option in marketOptionsList)
            {
                calibratedParams[0] = nv;
                calibratedParams[1] = k;
                calibratedParams[2] = theta;
                calibratedParams[3] = rho;
                calibratedParams[4] = sigma;
                double optionExercise = option.optionExercise;
                double strike = option.strike;
                double modelPrice = m.CalculateCallOptionPrice(S, strike, r, optionExercise);
                double difference = modelPrice - option.marketMidPrice;
                meanSqErr += difference * difference;
            }
            return meanSqErr;
        }

        // Used by Alglib minimisation algorithm
        public void CalibrationObjectiveFunction(double[] paramsArray, ref double func, object obj)
        {
            Hestonformula m = new Hestonformula(paramsArray);
            func = CalcMeanSquareErrorBetweenModelAndMarket(m);
        }
       
        public void Calibrate()
        {
            outcome = CalibrationOutcome.NotStarted;
            double[] initialParams = new double[Hestonformula.numModelParams];
            calibratedParams.CopyTo(initialParams, 0);  // a reasonable starting guees
            double epsg = accuracy;
            double epsf = accuracy; //1e-4;
            double epsx = accuracy;
            double diffstep = 1.0e-6;
            int maxits = maxIterations;
            double stpmax = 0.05;

            alglib.minlbfgsstate state;
            alglib.minlbfgsreport rep;
            alglib.minlbfgscreatef(1, initialParams, diffstep, out state);
            alglib.minlbfgssetcond(state, epsg, epsf, epsx, maxits);
            alglib.minlbfgssetstpmax(state, stpmax);

            // this will do the work
            alglib.minlbfgsoptimize(state, CalibrationObjectiveFunction, null, null);
            double[] resultParams = new double[Hestonformula.numModelParams];
            alglib.minlbfgsresults(state, out resultParams, out rep);

            Console.WriteLine("Termination type: {0}", rep.terminationtype);
            Console.WriteLine("Num iterations {0}", rep.iterationscount);
            Console.WriteLine("{0}", alglib.ap.format(resultParams, 5));

            if (rep.terminationtype == 1            // relative function improvement is no more than EpsF.
                || rep.terminationtype == 2         // relative step is no more than EpsX.
                || rep.terminationtype == 4)
            {    	// gradient norm is no more than EpsG
                outcome = CalibrationOutcome.FinishedOK;
                // we update the ''inital parameters''
                calibratedParams = resultParams;
            }
            else if (rep.terminationtype == 5)
            {	// MaxIts steps was taken
                outcome = CalibrationOutcome.FailedMaxItReached;
                // we update the ''inital parameters'' even in this case
                calibratedParams = resultParams;

            }
            else
            {
                outcome = CalibrationOutcome.FailedOtherReason;
                throw new CalibrationFailedException("Heston model calibration failed badly.");
            }
        }


        public void GetCalibrationStatus(ref CalibrationOutcome calibOutcome, ref double pricingError)
        {
            calibOutcome = outcome;
            Hestonformula m = new Hestonformula(calibratedParams);
            pricingError = CalcMeanSquareErrorBetweenModelAndMarket(m);
        }

        public Hestonformula GetCalibratedModel()
        {
            Hestonformula m = new Hestonformula(calibratedParams);
            return m;
        }
    }
}

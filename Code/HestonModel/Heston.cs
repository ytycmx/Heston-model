using System;
using System.Collections.Generic;
using HestonModel.Interfaces;
using HestonModel;
using FinalAssignment1;
using FinalAssignment2;
using FinalAssignment4;
using FinalAssignment5;

namespace HestonModel
{

    /// <summary> 
    /// This class will be used for grading. 
    /// Don't remove any of the methods and don't modify their signatures. Don't change the namespace. 
    /// Your code should be implemented in other classes (or even projects if you wish), and the relevant functionality should only be called here and outputs returned.
    /// You don't need to implement the interfaces that have been provided if you don't want to.
    /// </summary>
    public static class Heston
    {
        /// <summary>
        /// Method for calibrating the heston model.
        /// </summary>
        /// <param name="guessModelParameters">Object implementing IHestonModelParameters interface containing the risk-free rate, initial stock price
        /// and initial guess parameters to be used in the calibration.</param>
        /// <param name="referenceData">A collection of objects implementing IOptionMarketData<IEuropeanOption> interface. These should contain the reference data used for calibration.</param>
        /// <param name="calibrationSettings">An object implementing ICalibrationSettings interface.</param>
        /// <returns>Object implementing IHestonCalibrationResult interface which contains calibrated model parameters and additional diagnostic information</returns>
        public static IHestonCalibrationResult CalibrateHestonParameters(IHestonModelParameters guessModelParameters, 
                                                                         IEnumerable<IOptionMarketData<IEuropeanOption>> referenceData, 
                                                                         ICalibrationSettings calibrationSettings)
        {
            HestonCalibrator calibrator = new HestonCalibrator(guessModelParameters.RiskFreeRate,
                                                               guessModelParameters.InitialStockPrice,
                                                               calibrationSettings.Accuracy,
                                                               calibrationSettings.MaximumNumberOfIterations);
            calibrator.SetGuessParameters(guessModelParameters.VarianceParameters.V0,
                                          guessModelParameters.VarianceParameters.Kappa,
                                          guessModelParameters.VarianceParameters.Theta,
                                          guessModelParameters.VarianceParameters.Rho,
                                          guessModelParameters.VarianceParameters.Sigma);
            foreach (IOptionMarketData<IEuropeanOption> M in referenceData)
            {
                calibrator.AddObservedOption(M.Option.StrikePrice, M.Option.Maturity, M.Price);
            }
            calibrator.Calibrate();
            double error = 0;
            CalibrationOutcome outcome = CalibrationOutcome.NotStarted;
            calibrator.GetCalibrationStatus(ref outcome, ref error);
            Console.WriteLine("Calibration outcome: {0} and error: {1}", outcome, error);
            IHestonCalibrationResult sss;
            /*foreach (CalibrationOutcome MinimizerStatus in sss)*/
            /*object[,] ansTable =*/
            return sss;
        }
        /// <summary>
        /// Price a European option in the Heston model using the Heston formula. This should be accurate to 5 decimal places
        /// </summary>
        /// <param name="parameters">Object implementing IHestonModelParameters interface, containing model parameters.</param>
        /// <param name="europeanOption">Object implementing IEuropeanOption interface, containing the option parameters.</param>
        /// <returns>Option price</returns>
        public static double HestonEuropeanOptionPrice(IHestonModelParameters parameters, 
                                                       IEuropeanOption europeanOption)
        {
            Hestonformula r1 = new Hestonformula( parameters.VarianceParameters.V0, 
                                                  parameters.VarianceParameters.Kappa, 
                                                  parameters.VarianceParameters.Theta, 
                                                  parameters.VarianceParameters.Rho, 
                                                  parameters.VarianceParameters.Sigma);
            if (europeanOption.Type == PayoffType.Call)
            {
                return Math.Round(r1.CalculateCallOptionPrice(parameters.InitialStockPrice,
                                                   europeanOption.StrikePrice,
                                                   parameters.RiskFreeRate,
                                                   europeanOption.Maturity), 5);
            }
            else
            {
                return Math.Round(r1.CalculatePutOptionPrice(parameters.InitialStockPrice,
                                                   europeanOption.StrikePrice,
                                                   parameters.RiskFreeRate,
                                                   europeanOption.Maturity), 5);
            }
        }

        /// <summary>
        /// Price a European option in the Heston model using the Monte-Carlo method. Accuracy will depend on number of time steps and samples
        /// </summary>
        /// <param name="parameters">Object implementing IHestonModelParameters interface, containing model parameters.</param>
        /// <param name="europeanOption">Object implementing IEuropeanOption interface, containing the option parameters.</param>
        /// <param name="monteCarloSimulationSettings">An object implementing IMonteCarloSettings object and containing simulation settings.</param>
        /// <returns>Option price</returns>
        public static double HestonEuropeanOptionPriceMC(IHestonModelParameters parameters, 
                                                         IEuropeanOption europeanOption, 
                                                         IMonteCarloSettings monteCarloSimulationSettings)
        {
            HestonMC p1 = new HestonMC();
            if (europeanOption.Type == PayoffType.Call)
            {
                return p1.CalculateEuropeanCallOptionPrice(parameters.InitialStockPrice,
                                                           europeanOption.StrikePrice,
                                                           parameters.RiskFreeRate,
                                                           europeanOption.Maturity,
                                                           parameters.VarianceParameters.V0,
                                                           parameters.VarianceParameters.Kappa,
                                                           parameters.VarianceParameters.Theta,
                                                           parameters.VarianceParameters.Rho,
                                                           parameters.VarianceParameters.Sigma,
                                                           monteCarloSimulationSettings.NumberOfTimeSteps,
                                                           monteCarloSimulationSettings.NumberOfTrials);
            }
            else
            {
                return p1.CalculatePutOptionPrice(parameters.InitialStockPrice,
                                                  europeanOption.StrikePrice,
                                                  parameters.RiskFreeRate,
                                                  europeanOption.Maturity,
                                                  parameters.VarianceParameters.V0,
                                                  parameters.VarianceParameters.Kappa,
                                                  parameters.VarianceParameters.Theta,
                                                  parameters.VarianceParameters.Rho,
                                                  parameters.VarianceParameters.Sigma,
                                                  monteCarloSimulationSettings.NumberOfTimeSteps,
                                                  monteCarloSimulationSettings.NumberOfTrials);
            }
        }

        /// <summary>
        /// Price a Asian option in the Heston model using the 
        /// Monte-Carlo method. Accuracy will depend on number of time steps and samples</summary>
        /// <param name="parameters">Object implementing IHestonModelParameters interface, containing model parameters.</param>
        /// <param name="asianOption">Object implementing IAsian interface, containing the option parameters.</param>
        /// <param name="monteCarloSimulationSettings">An object implementing IMonteCarloSettings object and containing simulation settings.</param>
        /// <returns>Option price</returns>
        public static double HestonAsianOptionPriceMC(IHestonModelParameters parameters, 
                                                      IAsianOption asianOption, 
                                                      IMonteCarloSettings monteCarloSimulationSettings)
        {
            HestonAsianoption v1 = new HestonAsianoption();
            if (asianOption.Type == PayoffType.Call)
            {
                return v1.CalculateAsianCallOptionPrice(parameters.InitialStockPrice,
                                                        asianOption.StrikePrice,
                                                        parameters.RiskFreeRate,
                                                        asianOption.MonitoringTimes,
                                                        asianOption.Maturity,
                                                        parameters.VarianceParameters.V0,
                                                        parameters.VarianceParameters.Kappa,
                                                        parameters.VarianceParameters.Theta,
                                                        parameters.VarianceParameters.Rho,
                                                        parameters.VarianceParameters.Sigma,
                                                        monteCarloSimulationSettings.NumberOfTimeSteps,
                                                        monteCarloSimulationSettings.NumberOfTrials);
            }
            else
            {
                return v1.CalculateAsianPutOptionPrice(parameters.InitialStockPrice,
                                                       asianOption.StrikePrice,
                                                       parameters.RiskFreeRate,
                                                       asianOption.MonitoringTimes,
                                                       asianOption.Maturity,
                                                       parameters.VarianceParameters.V0,
                                                       parameters.VarianceParameters.Kappa,
                                                       parameters.VarianceParameters.Theta,
                                                       parameters.VarianceParameters.Rho,
                                                       parameters.VarianceParameters.Sigma,
                                                       monteCarloSimulationSettings.NumberOfTimeSteps,
                                                       monteCarloSimulationSettings.NumberOfTrials);
            }
        }

        /// <summary>
        /// Price a lookback option in the Heston model using the  
        /// a Monte-Carlo method. Accuracy will depend on number of time steps and samples </summary>
        /// <param name="parameters">Object implementing IHestonModelParameters interface, containing model parameters.</param>
        /// <param name="maturity">An object implementing IOption interface and containing option's maturity</param>
        /// <param name="monteCarloSimulationSettings">An object implementing IMonteCarloSettings object and containing simulation settings.</param>
        /// <returns>Option price</returns>
        public static double HestonLookbackOptionPriceMC(IHestonModelParameters parameters, 
                                                         IOption maturity, 
                                                         IMonteCarloSettings monteCarloSimulationSettings)
        {
            HestonLookbackoption w1 = new HestonLookbackoption();
            return w1.CalculateLookbackCallOptionPrice(parameters.InitialStockPrice,
                                                       parameters.RiskFreeRate,
                                                       maturity.Maturity,
                                                       parameters.VarianceParameters.V0,
                                                       parameters.VarianceParameters.Kappa,
                                                       parameters.VarianceParameters.Theta,
                                                       parameters.VarianceParameters.Rho,
                                                       parameters.VarianceParameters.Sigma,
                                                       monteCarloSimulationSettings.NumberOfTimeSteps,
                                                       monteCarloSimulationSettings.NumberOfTrials);
        }
    }
}

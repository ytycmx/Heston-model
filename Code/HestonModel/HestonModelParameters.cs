using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HestonModel.Interfaces;
using HestonModel;
using FinalAssignment1;
using FinalAssignment2;
using FinalAssignment4;
using FinalAssignment5;


namespace HestonModel
{
    public class Hestonoption : IOption
    {
        private double T;
        public Hestonoption() { this.T = 1; }
        public Hestonoption(double T) { this.T = T; }
        public double Maturity { get { return T; } }
    }

    public class Hestonparameter : IVarianceProcessParameters
    {
        private double k;
        private double theta;
        private double sigma;
        private double nv;
        private double rho;
        public Hestonparameter()
        {
            this.k = 1.5768;
            this.theta = 0.0398;
            this.sigma = 0.5751;
            this.nv = 0.0175;
            this.rho = -0.5711;
        }
        public Hestonparameter(double k, double theta, double sigma, double nv, double rho)
        {
            this.k = k;
            this.theta = theta;
            this.sigma = sigma;
            this.nv = nv;
            this.rho = rho;
        }
        public double Kappa { get { return k; } }
        public double Theta { get { return theta; } }
        public double Sigma { get { return sigma; } }
        public double V0 { get { return nv; } }
        public double Rho { get { return rho; }
        }
    }

    public class HestonMCsetting : IMonteCarloSettings
    {
        private int n;
        private int N;
        public HestonMCsetting()
        {
            this.n = 100000;
            this.N = 365;
        }
        public HestonMCsetting(int n, int N)
        {
            this.n = n;
            this.N = N;
        }
        public int NumberOfTrials { get { return n; } }
        public int NumberOfTimeSteps { get { return N; } }
    }

    public class HestonCalibrationSetting : ICalibrationSettings
    {
        private double accuracy;
        private int maximumNumberOfIterations;
        public HestonCalibrationSetting()
        {
            this.accuracy = 0.001;
            this.maximumNumberOfIterations = 1000;
        }
        public HestonCalibrationSetting(double accuracy, int maximumNumberOfIterations)
        {
            this.accuracy = accuracy;
            this.maximumNumberOfIterations = maximumNumberOfIterations;
        }
        public double Accuracy { get { return accuracy; } }
        public int MaximumNumberOfIterations { get { return maximumNumberOfIterations; } }
    }

    public class Hestonoptionmarketdata : IOptionMarketData<IOption>
    {
        private IOption option;
        private double Marketprice;
        public Hestonoptionmarketdata()
        {
            this.option = new Hestonoption(1);
            this.Marketprice = 25.72;
        }
        public Hestonoptionmarketdata(IOption option, double Marketprice)
        {
            this.option = option;
            this.Marketprice = Marketprice;
        }
        public IOption Option { get { return option; } }
        public double Price { get { return Marketprice; } }
    }

    public class Hestonmodelparameter : IHestonModelParameters
    {
        private double S;
        private double r;
        private IVarianceProcessParameters varianceParameters;
        public Hestonmodelparameter()
        {
            this.S = 100;
            this.r = 0.1;
            this.varianceParameters = new Hestonparameter(2, 0.06, 0.4, 0.04, 0.5);
        }
        public Hestonmodelparameter(double S, double r, IVarianceProcessParameters varianceParameters)
        {
            this.S = S;
            this.r = r;
            this.varianceParameters = varianceParameters;
        }
        public double InitialStockPrice { get { return S; } }
        public double RiskFreeRate { get { return r; } }
        public IVarianceProcessParameters VarianceParameters { get { return varianceParameters; } }
    }

    public class HestonEuro : IEuropeanOption
    {
        private PayoffType type;
        private double K;
        private double T;
        public HestonEuro()
        {
            this.type = PayoffType.Call;
            this.K = 100;
            this.T = 1;
        }
        public HestonEuro(PayoffType type, double K, double T)
        {
            this.type = type;
            this.K = K;
            this.T = T;
        }
        public PayoffType Type { get { return type; } }
        public double StrikePrice { get { return K; } }
        public double Maturity { get { return T; } }
    }

    public class HestonAsian : IAsianOption
    {
        private IEnumerable<double> Tm;
        private PayoffType type;
        private double K;
        private double T;
        public HestonAsian()
        {
            this.Tm = new double[] { 0.75, 1.00 };
            this.type = PayoffType.Call;
            this.K = 100;
            this.T = 1;
        }
        public HestonAsian(IEnumerable<double> Tm, PayoffType type, double K, double T)
        {
            this.Tm = Tm;
            this.type = type;
            this.K = K;
            this.T = T;
        }
        public IEnumerable<double> MonitoringTimes { get { return Tm; } }
        public PayoffType Type { get { return type; } }
        public double StrikePrice { get { return K; } }
        public double Maturity { get { return T; } }
    }
    public class Hestoncalibrationresult : ICalibrationResult
    {
        private CalibrationOutcome minimizerStatus;
        private double error;
        public Hestoncalibrationresult()
        {
            this.minimizerStatus = CalibrationOutcome.FinishedOK;
            this.error = 0.11000;
        }
        public Hestoncalibrationresult(CalibrationOutcome minimizerStatus, double error)
        {
            this.minimizerStatus = minimizerStatus;
            this.error = error;
        }
        public CalibrationOutcome MinimizerStatus { get { return minimizerStatus; } }
        public double PricingError { get { return error; } }
    }
    public class HestonCalibrationResult : IHestonCalibrationResult
    {
        private CalibrationOutcome minimizerStatus;
        private double error;
        private IHestonModelParameters parameters;
        public HestonCalibrationResult()
        {
            /*this.minimizerStatus = CalibrationOutcome.FinishedOK;
            this.error = 0.11000;
            this.parameters = ;*/
        }
        public HestonCalibrationResult(CalibrationOutcome minimizerStatus, double error, IHestonModelParameters parameters)
        {
            this.minimizerStatus = minimizerStatus;
            this.error = error;
            this.parameters = parameters;
        }
        public CalibrationOutcome MinimizerStatus { get { return minimizerStatus; } }
        public double PricingError { get { return error; } }
        public IHestonModelParameters Parameters { get { return parameters; } }
    }

    /*public class CalibrateHestonParameters : IHestonCalibrationResult
    {
        public CalibrateHestonParameters（double kappaindex, double thetaindex, double maturity, double optionCallprice, double K, double r, IHestonModelParameters Parameters）
            {
            }
    }*/












}

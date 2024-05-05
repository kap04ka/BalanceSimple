using balanceSimple.Calculators;
using balanceSimple.Controllers;
using balanceSimple.Models;
using balanceSimple.Services;
using System.ComponentModel.DataAnnotations;

namespace balanceSimple.Tests
{
    public class UnitTest1
    {
        private BalanceInput generateTestData()
        {
            BalanceInput balanceInput = new BalanceInput();

            var flows = new List<Flow>()
            {
               new Flow {Id = 1, Name = "x1", Value = 10.005, Tols = 0.200, IsUsed = true, LowerBound = 0, UpperBound = 1000, SourceNode = -1, DestNode = 1 },
               new Flow {Id = 2, Name = "x2", Value = 10.005, Tols = 0.200, IsUsed = true, LowerBound = 0, UpperBound = 1000, SourceNode = 1, DestNode = 2 },
               new Flow {Id = 3, Name = "x3", Value = 10.005, Tols = 0.200, IsUsed = true, LowerBound = 0, UpperBound = 1000, SourceNode = 2, DestNode = -1 }
            };
            
            balanceInput.Id = 1;
            balanceInput.flows = flows;

            return balanceInput;
        }

        [Fact]
        public void positive_test_data()
        {
            ICalculator calc = new Calculator();

            int iterCount = 2000;

            double[,] Ab = new double[,] {
                {1,  -1,   -1,    0,   0,  0,    0,    0},
                {0,   0,    1,    -1, -1,  0,    0,    0},
                {0,   0,    0,    0,   1,  -1,  -1,    0},
            };

            double[] x0 = new double[] { 10.005, 3.033, 6.831, 1.985, 5.093, 4.057, 0.991 };
            double[] errors = new double[] { 0.200, 0.121, 0.683, 0.040, 0.102, 0.081, 0.020 };
            byte[] I = new byte[] { 1, 1, 1, 1, 1, 1, 1 };
            double[] lb = new double[] { 0, 0, 0, 0, 0, 0, 0 };
            double[] ub = new double[] { 10000, 10000, 10000, 10000, 10000, 10000, 10000 };

            List<double> result = new List<double>();
            result = calc.Calculate(iterCount, Ab, x0, errors, I, lb, ub);

            bool isAppropriate = true;

            double sum = 0;
            for (int i = 0; i < Ab.GetLength(0); i++)
            {
                sum = 0;
                for (int j = 0; j < Ab.GetLength(1) - 1; j++)
                {
                    sum += Ab[i, j] * result[j];
                }

                if (Math.Round(sum, 2) != 0)
                {
                    isAppropriate = false;
                    break;
                }
            }

            Assert.True(isAppropriate);
        }


        [Fact]
        public void positive_nechet_var_data()
        {
            ICalculator calc = new Calculator();

            int iterCount = 2000;

            double[,] Ab = new double[,] {
                {1,  -1,   -1,    0,   0,  0,    0,    -1,  0},
                {0,   0,    1,    -1, -1,  0,    0,    0,   0},
                {0,   0,    0,    0,   1,  -1,  -1,    0,   0},
            };

            double[] x0 = new double[] { 10.005, 3.033, 6.831, 1.985, 5.093, 4.057, 0.991, 6.667 };
            double[] errors = new double[] { 0.200, 0.121, 0.683, 0.040, 0.102, 0.081, 0.020, 0.667 };
            byte[] I = new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 };
            double[] lb = new double[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            double[] ub = new double[] { 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000 };

            List<double> result = new List<double>();
            result = calc.Calculate(iterCount, Ab, x0, errors, I, lb, ub);

            bool isAppropriate = true;

            double sum = 0;
            for (int i = 0; i < Ab.GetLength(0); i++)
            {
                sum = 0;
                for (int j = 0; j < Ab.GetLength(1) - 1; j++)
                {
                    sum += Ab[i, j] * result[j];
                }
                if (Math.Round(sum, 3) != 0)
                {
                    isAppropriate = false;
                    break;
                }
            }

            Assert.True(isAppropriate);
        }

        [Fact]
        public void positive_x1_eq_10x2_data()
        {
            ICalculator calc = new Calculator();

            int iterCount = 2000;

            double[,] Ab = new double[,] {
                {1,  -1,   -1,    0,   0,  0,    0,    -1,  0},
                {0,   0,    1,    -1, -1,  0,    0,    0,   0},
                {0,   0,    0,    0,   1,  -1,  -1,    0,   0},
                {1,   -10,  0,    0,   0,  0,    0,    0,   0},
            };

            double[] x0 = new double[] { 10.005, 3.033, 6.831, 1.985, 5.093, 4.057, 0.991, 6.667 };
            double[] errors = new double[] { 0.200, 0.121, 0.683, 0.040, 0.102, 0.081, 0.020, 0.667 };
            byte[] I = new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 };
            double[] lb = new double[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            double[] ub = new double[] { 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000 };

            List<double> result = new List<double>();
            result = calc.Calculate(iterCount, Ab, x0, errors, I, lb, ub);

            bool isAppropriate = true;

            double sum = 0;
            for (int i = 0; i < Ab.GetLength(0); i++)
            {
                sum = 0;
                for (int j = 0; j < Ab.GetLength(1) - 1; j++)
                {
                    sum += Ab[i, j] * result[j];
                }
                if (Math.Round(sum, 2) != 0)
                {
                    isAppropriate = false;
                    break;
                }
            }

            if (Math.Round(result[0], 2) != 10 * Math.Round(result[1], 3))
            {
                isAppropriate = false;
            }

            Assert.True(isAppropriate);
        }

        [Fact]
        public void positive_controller_return_data()
        {
            var data = generateTestData();
            ICalculatorService calculatorService = new CalculatorService();
            var conroller = new BalanceController(calculatorService);

            var result = conroller.balanceCalculate(data);

            Assert.NotNull(result);
        }

        [Fact]
        public void negative_service_throw_exception()
        {
            BalanceInput data = new BalanceInput();
            var flows = new List<Flow>();

            data.Id = 0;
            data.flows = flows;

            ICalculatorService calculatorService = new CalculatorService();
            var conroller = new BalanceController(calculatorService);

            Assert.Throws<ValidationException>(() => conroller.balanceCalculate(data));
        }
    }
}
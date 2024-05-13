using balanceSimple.Calculators;


namespace balanceSimple.Tests
{
    public class CalculatorTests
    {
     
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
    }
}
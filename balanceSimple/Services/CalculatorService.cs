using balanceSimple.Calculators;
using balanceSimple.Models;

namespace balanceSimple.Services
{
    public class CalculatorService : ICalculatorService
    {
        public BalanceOutput Calculate(BalanceInput balanceInput)
        {
            // Экземпляр класса калькулятор для вычислений
            ICalculator calculator = new Calculator();

            // Эхземпляр выходых данных
            var outputData = new BalanceOutput();

            // Переменные для имен и начальных значений потока
            List<string> names = new List<string>();
            List<double> startResults = new List<double>();


            // Количество итераций
            int iterCount = 2000;

            // Массивы для расчета
            double[] x0 = new double[balanceInput.flows.Count];
            double[] errors = new double[balanceInput.flows.Count];
            byte[] I = new byte[balanceInput.flows.Count];
            double[] lb = new double[balanceInput.flows.Count];
            double[] ub = new double[balanceInput.flows.Count];

            int i = 0;
            int size = 0;

            foreach (var flow in balanceInput.flows.OrderBy(w => w.Name))
            {
                int.TryParse(string.Join("", flow.Name.Where(c => char.IsDigit(c))), out i);
                i--;

                names.Add(flow.Name);
                startResults.Add(flow.Value);

                // Заполнение данных, которые есть всегда
                x0[i] = flow.Value;
                errors[i] = flow.Tols;
                if (flow.IsUsed) I[i] = 1;
                
                lb[i] = (double)flow.LowerBound;
                ub[i] = (double)flow.UpperBound;

                // Узнаем размер матрицы AB
                if (flow.DestNode > size) size = (int)flow.DestNode;

            }

            double[,] Ab = new double[size, balanceInput.flows.Count + 1];
            // Заполнение Ab
            foreach (var flow in balanceInput.flows.OrderBy(w => w.Name))
            {
                int.TryParse(string.Join("", flow.Name.Where(c => char.IsDigit(c))), out i);
                i--;

                if (flow.SourceNode != -1) Ab[(int)flow.SourceNode - 1, i] = -1;
                if (flow.DestNode != -1) Ab[(int)flow.DestNode - 1, i] = 1;
            }

            List<double> results = calculator.Calculate(iterCount, Ab, x0, errors, I, lb, ub);

            outputData.FlowsNames = names;
            outputData.InitValues = startResults;
            outputData.FinalValues = results;

            return outputData;
        }

    }
}

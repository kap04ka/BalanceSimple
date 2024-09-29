using balanceSimple.Calculators;
using balanceSimple.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace balanceSimple.Services
{
    public class CalculatorService : ICalculatorService
    {
        private readonly ILogger<CalculatorService> _logger;

        public CalculatorService(ILogger<CalculatorService> logger)
        {
            _logger = logger;
        }

        public BalanceOutput Calculate(BalanceInput balanceInput)
        {
            _logger.LogInformation("Начата обработка данных");

            if (balanceInput.flows.Count == 0) {
                _logger.LogError("Массив потоков пустой!");
                throw new ValidationException("Message: Flow array is empty!");
            }

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

            foreach (var flow in balanceInput.flows.OrderBy(w => w.Id))
            {
                if (flow.LowerBound > flow.UpperBound)
                {
                    _logger.LogError($"Верхняя границе меньше чем нижняя для потока {i + 1}!");
                    throw new ValidationException($"Message: Upper bound less then lower bound in flow {i + 1}!");
                }
                if (flow.Id != i)
                {
                    _logger.LogError($"Поток {i + 1} отсутсвует!");
                    throw new ValidationException($"Message: Flow {i + 1} is missing!");
                }
                if (flow.Value < 0 || flow.Tols < 0)
                {
                    _logger.LogError($"Значение или ошибка неверная для потока {i + 1}!");
                    throw new ValidationException($"Message: Value or tols are incorrect in {i + 1} flow");
                }
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
                i++;
            }

            double[,] Ab = new double[size, balanceInput.flows.Count + 1];
            i = 0;
            // Заполнение Ab
            foreach (var flow in balanceInput.flows.OrderBy(w => w.Id))
            {
                if (flow.SourceNode != -1) Ab[(int)flow.SourceNode - 1, i] = -1;
                if (flow.DestNode != -1) Ab[(int)flow.DestNode - 1, i] = 1;
                i++;
            }

            List<double> results = calculator.Calculate(iterCount, Ab, x0, errors, I, lb, ub);

            outputData.FlowsNames = names;
            outputData.InitValues = startResults;
            outputData.FinalValues = results;
            outputData.IsBalanced = checkBalanced(Ab, results);

            if (outputData.IsBalanced) _logger.LogInformation("Баланс сведен");
            else _logger.LogInformation("Баланс не сведен");

            _logger.LogInformation("Обработка данных завершена успешно");

            return outputData;
        }

        public bool checkBalanced(double[,] Ab, List<double> result)
        {
            bool isAppropriate = true;

            double sum = 0;
            for (int i = 0; i < Ab.GetLength(0); i++)
            {
                sum = 0;
                for (int j = 0; j < Ab.GetLength(1) - 1; j++)
                {
                    sum += Ab[i, j] * result[j];
                }

                if (Math.Round(sum, 1) != 0)
                {
                    isAppropriate = false;
                    break;
                }
            }

            return isAppropriate;
        }

    }
}

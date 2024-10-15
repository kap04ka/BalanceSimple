using Accord.Math;
using Accord.Math.Optimization;
using System;
using System.Collections.Generic;

namespace balanceSimple.Calculators
{
    public class CalculatorAccord : ICalculator
    {
        public List<double> Calculate(int iterCount, double[,] Ab, double[] x0, double[] errors, byte[] I, double[] lb, double[] ub)
        {
            // решение
            double[] x = { };
            // Получение размерности
            int n = errors.GetLength(0);
            int m = Ab.GetLength(0);

            // Формирование матрицы H = W * I, W = 1/error[i]^2
            double[,] H = new double[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                    {
                        H[i, j] = I[i] / Math.Pow(errors[i], 2);
                    }

                    else
                    {
                        H[i, j] = 0;
                    }
                }
            }

            for (int iter = 0; iter < iterCount; iter++)
            {
                // Формирование вектора d = -H * x0
                double[] d = new double[n];

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        d[i] += -H[i, j] * x0[j];
                    }
                }

                try
                {
                    var constraints = new List<LinearConstraint>();

                    // Добавляем ограничения из матрицы Ab (Ax = 0)
                    for (int i = 0; i < m; i++)
                    {
                        var combined = new double[n];
                        for (int j = 0; j < n; j++)
                        {
                            combined[j] = Ab[i, j];
                        }

                        // Ограничение для текущей строки Ab, Ax = 0
                        constraints.Add(new LinearConstraint(numberOfVariables: n)
                        {
                            CombinedAs = combined,
                            ShouldBe = ConstraintType.EqualTo, // Условие Ax = 0
                            Value = 0 // Равенство нулю
                        });
                    }

                    // Задаем нижние границы для переменных
                    for (int i = 0; i < n; i++)
                    {
                        constraints.Add(new LinearConstraint(numberOfVariables: 1)
                        {
                            VariablesAtIndices = new[] { i },
                            ShouldBe = ConstraintType.GreaterThanOrEqualTo,
                            Value = lb[i] // Нижняя граница
                        });
                    }

                    // Задаем верхние границы для переменных
                    for (int i = 0; i < n; i++)
                    {
                        constraints.Add(new LinearConstraint(numberOfVariables: 1)
                        {
                            VariablesAtIndices = new[] { i },
                            ShouldBe = ConstraintType.LesserThanOrEqualTo,
                            Value = ub[i] // Верхняя граница
                        });
                    }

                    var solver = new GoldfarbIdnani(
                        function: new QuadraticObjectiveFunction(H, d),
                        constraints: constraints);

                    bool success = solver.Minimize();

                    // Получаем решение
                    if (success)
                    {
                        x = solver.Solution;
                    }
                    else
                    {
                        Console.WriteLine("Решение не найдено.");
                    }
                    x0 = solver.Solution;
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Optimization error: " + ex.Message);
                }

                for (int i = 0; i < n; i++)
                {
                    x0[i] = x[i];
                }
            }

            // Округление результата
            for (int i = 0; i < n; i++)
            {
                x0[i] = Math.Round(x0[i], 3);
            }

            return x.ToList();
        }
    }
}

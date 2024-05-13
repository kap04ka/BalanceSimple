﻿using balanceSimple.Controllers;
using balanceSimple.Models;
using balanceSimple.Services;
using System.ComponentModel.DataAnnotations;


namespace balanceSimple.Tests
{
    public class ControllerTests
    {
        private BalanceInput generateTestData()
        {
            BalanceInput balanceInput = new BalanceInput();

            var flows = new List<Flow>()
            {
               new Flow {Id = 0, Name = "x1", Value = 10.005, Tols = 0.200, IsUsed = true, LowerBound = 0, UpperBound = 1000, SourceNode = -1, DestNode = 1 },
               new Flow {Id = 1, Name = "x2", Value = 10.005, Tols = 0.200, IsUsed = true, LowerBound = 0, UpperBound = 1000, SourceNode = 1, DestNode = 2 },
               new Flow {Id = 2, Name = "x3", Value = 10.005, Tols = 0.200, IsUsed = true, LowerBound = 0, UpperBound = 1000, SourceNode = 2, DestNode = -1 }
            };

            balanceInput.Id = 0;
            balanceInput.flows = flows;

            return balanceInput;
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
        public void negative_service_throw_exc_when_flow_array_empty()
        {
            BalanceInput data = new BalanceInput();
            var flows = new List<Flow>();

            data.Id = 0;
            data.flows = flows;

            ICalculatorService calculatorService = new CalculatorService();
            var conroller = new BalanceController(calculatorService);

            Assert.Throws<ValidationException>(() => conroller.balanceCalculate(data));
        }

        [Fact]
        public void positive_big_data()
        {
            BalanceInput data = new BalanceInput();
            ICalculatorService calculatorService = new CalculatorService();
            var conroller = new BalanceController(calculatorService);

            var flows = new List<Flow>();
            int flowsCount = 100;

            for(int i = 0; i < flowsCount; i++)
            {

                int src = i + 1;
                int dst = i + 2;

                if(i == 0)
                {
                    src = -1;
                    dst = i + 1;
                }

                if (i == flowsCount - 1)
                {
                    src = i + 1;
                    dst = -1;
                }
                
                flows.Add(new Flow
                {
                    Id = i,
                    Name = $"x{i + 1}",
                    Value = flowsCount - i + 1,
                    Tols = 0.200,
                    IsUsed = true,
                    LowerBound = 0,
                    UpperBound = 10000,
                    SourceNode = src,
                    DestNode = dst
                });
            }

            data.Id = 0;
            data.flows = flows;

            var result = conroller.balanceCalculate(data);
            Assert.NotNull(result);
        }
    }
}

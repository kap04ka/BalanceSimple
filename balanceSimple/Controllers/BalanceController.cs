using balanceSimple.Models;
using balanceSimple.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;

namespace balanceSimple.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BalanceController : ControllerBase
    {
        private ICalculatorService _calculatorService;

        public BalanceController(ICalculatorService calcServ)
        {
            _calculatorService = calcServ;

        }

        [EnableCors]
        [HttpPost]
        public BalanceOutput balanceCalculate(BalanceInput inputFlows)
        {
            return _calculatorService.Calculate(inputFlows);
        }
    }
}
using balanceSimple.Models;
using balanceSimple.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;

namespace balanceSimple.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BalanceController : ControllerBase
    {
        private ICalculatorService _calculatorService;
        private readonly ILogger<BalanceController> _logger;

        public BalanceController(ICalculatorService calcServ, ILogger<BalanceController> logger)
        {
            _calculatorService = calcServ;
            _logger = logger;

        }

        [EnableCors]
        [HttpPost]
        public ActionResult balanceCalculate(BalanceInput inputFlows)
        {
            _logger.LogInformation("Обращение к balanceCalculator");
            try
            {
                var resultData = _calculatorService.Calculate(inputFlows);
                _logger.LogInformation("Данные обработаны успешно");
                return Ok(resultData);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Возникла ошибка");
                return BadRequest(ex.Message);
            }
        }
    }
}
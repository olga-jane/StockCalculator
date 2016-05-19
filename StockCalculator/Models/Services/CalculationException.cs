using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockCalculator.Models.Services
{
    public class CalculationException : Exception
    {
        public CalculationException(string message, Exception innerException) : base(message, innerException)
        {
        }
        public CalculationException(string message) : base(message)
        {
        }
    }
}
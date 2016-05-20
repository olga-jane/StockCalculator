using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockCalculator.Models.Services
{
    public class FinanceCalculator : IFinanceCalculator
    {
        private bool IsCorrect(Stock stock)
        {
            // TODO: it would be good to take highest possible values from customer and check them here

            return stock != null && stock.Percentage >= 0 && stock.Price >= 0 && stock.Quantity >= 0 && stock.Years >= 0;
        }

        // TODO: to know rules of rounding for financial calculations
        // Now used so-called Banker's rounding: the value is rounded to the nearest even number
        private decimal BankersRound(decimal number)
        {
            decimal number100 = number * 100;
            decimal result = decimal.Ceiling(number100);
            if (result % 2 != 0)
            {
                result = decimal.Floor(number100);
            }
            return result/100;
        }

        public IEnumerable<StockYearValue> Calculate(Stock stock)
        {
            List<StockYearValue> values = null;
            try
            {
                if (IsCorrect(stock))
                {
                    values = new List<StockYearValue>();
                    decimal baseValue = stock.Price * stock.Quantity;

                    for (int year = 0; year <= stock.Years; year++)
                    {
                        StockYearValue value = new StockYearValue();
                        value.Id = value.Year = year;
                        baseValue = value.Value = BankersRound(baseValue + (year <= 0 ? 0 : baseValue * stock.Percentage / 100));
                        values.Add(value);
                    }
                }
            }
            catch (Exception e)
            {
                string message = "Finance calculator can not calculate the stock data";
                if (stock != null)
                {
                    message += String.Format(": {0} {1} {2} {3} {4}", stock.Name, stock.Price, stock.Quantity, stock.Percentage, stock.Years); 
                }
                throw new CalculationException(message, e);
            }
            return values;
        }
    }
}
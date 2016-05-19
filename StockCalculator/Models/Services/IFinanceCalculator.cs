using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockCalculator.Models.Services
{
    public interface IFinanceCalculator
    {
        IEnumerable<StockYearValue> Calculate(Stock stock);
    }
}

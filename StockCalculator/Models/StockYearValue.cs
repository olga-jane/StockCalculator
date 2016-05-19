using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockCalculator.Models
{
    public class StockYearValue
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public decimal Value { get; set; }
    }
}
